using elearning_b1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace elearning_b1.Controllers
{
    public class VocabulariesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VocabulariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy danh sách tất cả các topic
            var topics = _context.Topics.ToList();
            return View(topics); // Gửi danh sách topics về View
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

        // Action để hiển thị bài tập từ VocabExercise
        public async Task<IActionResult> Exercise(int id)
        {
            // Tìm bài tập theo ID
            var exercise = await _context.Exercises
                .Include(e => e.Topic)
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy bài tập
            }

            // Trả về View chứa bài tập và câu hỏi
            return View(exercise);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int id, Dictionary<int, int> answers)
        {
            // Tìm bài tập theo ID
            var exercise = await _context.Exercises
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy bài tập
            }

            // Tạo danh sách kết quả
            foreach (var question in exercise.Questions)
            {
                // Đánh dấu lựa chọn của người dùng và xác định đáp án đúng
                foreach (var option in question.Options)
                {
                    option.IsUserChoice = answers.TryGetValue(question.Id, out int userAnswerId) && userAnswerId == option.Id;
                    option.IsCorrect = option.IsCorrect;
                }
            }

            // Tính số câu trả lời đúng
            int correctCount = exercise.Questions.Count(q =>
                answers.TryGetValue(q.Id, out int userAnswerId) &&
                q.Options.Any(o => o.Id == userAnswerId && o.IsCorrect));

            // Tính điểm
            int totalQuestions = exercise.Questions.Count;
            double score = (double)correctCount / totalQuestions * 100;

            // Truyền dữ liệu vào View
            ViewData["Score"] = score;
            ViewData["CorrectCount"] = correctCount;
            ViewData["TotalQuestions"] = totalQuestions;
            return View("Result", exercise);
        }

    }
}
