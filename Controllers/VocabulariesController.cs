using elearning_b1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace elearning_b1.Controllers
{
    public class VocabulariesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public VocabulariesController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            // Đặt số lượng item trên mỗi trang
            int pageSize = 3;

            // Lấy tất cả các chủ đề và đếm số lượng từ vựng cho mỗi chủ đề
            var topics = await _context.Topics
                .Include(t => t.Vocabularies) // Đảm bảo lấy luôn danh sách từ vựng của mỗi chủ đề
                .ToListAsync();

            // Truyền vào view với số lượng từ vựng cho mỗi chủ đề
            var topicWithVocabularyCount = topics.Select(t => new
            {
                Topic = t,
                VocabularyCount = t.Vocabularies.Count // Đếm số lượng từ vựng của chủ đề
            }).ToList();

            // Lấy số lượng trang tổng cộng
            var totalPages = (int)Math.Ceiling((double)topicWithVocabularyCount.Count / pageSize);

            // Lấy các phần tử của trang hiện tại
            var pagedItems = topicWithVocabularyCount
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Truyền vào View
            ViewBag.Topics = pagedItems;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View();
        }


        public IActionResult Flashcards(int topicId)
        {

            // Lấy danh sách từ vựng theo topicId
            var vocabularies = _context.Vocabularies
                .Where(v => v.TopicID == topicId)
                .Include(v => v.Topic)
                .ToList();

            foreach (var vocab in vocabularies)
            {
                if (!string.IsNullOrEmpty(vocab.PartOfSpeech.ToString()))
                {
                    vocab.PartOfSpeech = (PartOfSpeech)Enum.Parse(typeof(PartOfSpeech), vocab.PartOfSpeech.ToString());
                }
            }

            // Gửi danh sách từ vựng về View
            return View(vocabularies);
        }

        public async Task<IActionResult> VocabularyList(int topicId)
        {
            var topic = _context.Topics.FirstOrDefault(t => t.TopicID == topicId); 
            ViewBag.TopicName = topic?.TopicName?? "Chủ đề chưa được xác định";
            // Lấy danh sách từ vựng cho chủ đề với topicId
            var vocabList = await _context.Vocabularies
                .Where(v => v.TopicID == topicId)
                .ToListAsync();

            if (vocabList == null || vocabList.Count == 0)
            {
                return NotFound(); // Nếu không tìm thấy từ vựng
            }

            return View(vocabList); // Trả về view chứa danh sách từ vựng
        }

        // Action để hiển thị bài tập từ Topic
        public async Task<IActionResult> Exercise(int id)
        {
            // Tìm chủ đề và các câu hỏi của chủ đề đó
            var topic = await _context.Topics
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(t => t.TopicID == id);

            if (topic == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy chủ đề
            }

            // Trả về View chứa bài tập và câu hỏi
            return View(topic); // Truyền chủ đề và các câu hỏi vào view
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int id, Dictionary<int, int> answers)
        {
            // Tìm chủ đề và các câu hỏi
            var topic = await _context.Topics
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(t => t.TopicID == id);

            if (topic == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy chủ đề
            }

            // Tạo danh sách kết quả cho các câu hỏi
            foreach (var question in topic.Questions)
            {
                // Đánh dấu lựa chọn của người dùng và xác định đáp án đúng
                foreach (var option in question.Options)
                {
                    option.IsUserChoice = answers.TryGetValue(question.Id, out int userAnswerId) && userAnswerId == option.Id;
                    // Không cần phải set lại option.IsCorrect ở đây vì dữ liệu này đã được xác định trong cơ sở dữ liệu
                }
            }

            // Tính số câu trả lời đúng
            int correctCount = topic.Questions.Count(q =>
                answers.TryGetValue(q.Id, out int userAnswerId) &&
                q.Options.Any(o => o.Id == userAnswerId && o.IsCorrect));

            // Tính điểm
            int totalQuestions = topic.Questions.Count;
            double score = (double)correctCount / totalQuestions * 100;

            // Truyền dữ liệu vào View để hiển thị kết quả
            ViewData["Score"] = score;
            ViewData["CorrectCount"] = correctCount;
            ViewData["TotalQuestions"] = totalQuestions;

            return View("Result", topic); // Trả về kết quả
        }

       


    }
}
