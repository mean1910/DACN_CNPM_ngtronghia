using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using elearning_b1.Models;
using elearning_b1.ViewModels;

namespace elearning_b1.Controllers
{
    public class ListeningController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListeningController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Listening
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewData["CurrentController"] = "Skills";
            ViewData["CurrentAction"] = "Listening";
            // Tổng số phần tử trong danh sách Listenings
            int pageSize = 6;
            int totalItems = await _context.Listenings.CountAsync();

            // Tính toán tổng số trang
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Giới hạn số trang
            if (page < 1) page = 1; // Trang bắt đầu là 1
            if (page > totalPages) page = totalPages;

            // Lấy dữ liệu của trang hiện tại
            var listenings = await _context.Listenings
                .OrderBy(l => l.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Trả về View với dữ liệu và thông tin phân trang
            ViewBag.Listenings = listenings;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(listenings);
        }



        // GET: Listening/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy bài nghe và các câu hỏi liên quan
            var listening = await _context.Listenings
                .Include(l => l.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (listening == null)
            {
                return NotFound();
            }

            // Tách transcript thành từng câu
            var sentences = string.IsNullOrEmpty(listening.Transcript)
                ? new List<string>()
                : listening.Transcript.Split(new[] { "Speaker " }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(part =>
                                      {
                                          var speakerIndex = part.IndexOf(": ");
                                          if (speakerIndex > 0)
                                          {
                                              var speaker = "Speaker " + part.Substring(0, speakerIndex);
                                              var content = part.Substring(speakerIndex + 2).Trim();
                                              return $"<strong>{speaker}:</strong> {content}";
                                          }
                                          return part.Trim();
                                      })
                                      .Where(s => !string.IsNullOrWhiteSpace(s))
                                      .ToList();

            var model = new ListeningUserViewModel
            {
                Id = listening.Id,
                Title = listening.Title,
                AudioUrl = $"https://drive.google.com/file/d/{listening.AudioUrl}/preview",
                Sentences = sentences,
                Questions = listening.Questions?.ToList() ?? new List<ListeningQuestion>()
            };

            return View(model);
        }

        

        [HttpPost]
        public async Task<IActionResult> Submit(int id, Dictionary<int, int> answers)
        {
            // Lấy bài tập Listening theo ID và các câu hỏi liên quan
            var listening = await _context.Listenings
                .Include(l => l.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (listening == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy bài tập
            }

            // Đánh dấu các câu trả lời của người dùng
            foreach (var question in listening.Questions)
            {
                foreach (var option in question.Options)
                {
                    option.IsUserChoice = answers.TryGetValue(question.Id, out int userAnswerId) && userAnswerId == option.Id;
                }
            }

            // Tính số câu trả lời đúng
            int correctCount = listening.Questions.Count(q =>
                answers.TryGetValue(q.Id, out int userAnswerId) &&
                q.Options.Any(o => o.Id == userAnswerId && o.IsCorrect));

            // Tính điểm
            int totalQuestions = listening.Questions.Count;
            double score = (double)correctCount / totalQuestions * 100;

            // Tạo view model cho kết quả
            var resultModel = new ListeningUserViewModel
            {
                Id = listening.Id,
                Title = listening.Title,
                AudioUrl = $"https://drive.google.com/file/d/{listening.AudioUrl}/preview",
                Sentences = string.IsNullOrEmpty(listening.Transcript)
                    ? new List<string>()
                    : listening.Transcript.Split(new[] { "Speaker " }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(part => part.Trim())
                                          .Where(s => !string.IsNullOrWhiteSpace(s))
                                          .ToList(),
                Questions = listening.Questions?.ToList() ?? new List<ListeningQuestion>()
            };

            // Truyền dữ liệu vào View để hiển thị kết quả
            ViewData["Score"] = score;
            ViewData["CorrectCount"] = correctCount;
            ViewData["TotalQuestions"] = totalQuestions;

            return View("Result", resultModel);
        }


    }
}
