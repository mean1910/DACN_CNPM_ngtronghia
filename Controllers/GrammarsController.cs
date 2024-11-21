using elearning_b1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace elearning_b1.Controllers
{
    public class GrammarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GrammarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Grammars/Index - Hiển thị danh sách và tìm kiếm bài học
        public async Task<IActionResult> Index(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                // Hiển thị toàn bộ bài học nếu không có từ khóa tìm kiếm
                var grammars = await _context.Grammars.ToListAsync();
                return View(grammars);
            }

            // Tìm kiếm bài học theo tiêu đề
            var searchResults = await _context.Grammars
                .Where(g => g.Title.Contains(query))
                .ToListAsync();

            return View(searchResults);
        }



        // GET: Grammars/View/5 - Hiển thị chi tiết bài học
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Nếu không có ID, trả về lỗi 404
            }

            // Tìm bài học Grammar theo ID
            var grammar = await _context.Grammars.FirstOrDefaultAsync(m => m.Id == id);
            if (grammar == null)
            {
                return NotFound(); // Nếu không tìm thấy, trả về lỗi 404
            }

            return View(grammar); // Trả dữ liệu bài học sang View
        }

        // Action để hiển thị bài tập ngữ pháp
        public async Task<IActionResult> Exercise(int id)
        {
            // Tìm bài tập theo ID
            var exercise = await _context.Grammars
                .Include(g => g.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (exercise == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy bài tập
            }

            // Trả về View chứa bài tập và câu hỏi
            return View(exercise);
        }

        // Action để nhận kết quả người dùng và tính điểm
        [HttpPost]
        public async Task<IActionResult> Submit(int id, Dictionary<int, int> answers)
        {
            // Tìm bài tập theo ID
            var grammar = await _context.Grammars
                .Include(g => g.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (grammar == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy bài tập
            }

            // Tạo danh sách kết quả
            foreach (var question in grammar.Questions)
            {
                // Đánh dấu lựa chọn của người dùng và xác định đáp án đúng
                foreach (var option in question.Options)
                {
                    option.IsUserChoice = answers.TryGetValue(question.Id, out int userAnswerId) && userAnswerId == option.Id;
                }
            }

            // Tính số câu trả lời đúng
            int correctCount = grammar.Questions.Count(q =>
                answers.TryGetValue(q.Id, out int userAnswerId) &&
                q.Options.Any(o => o.Id == userAnswerId && o.IsCorrect));

            // Tính điểm
            int totalQuestions = grammar.Questions.Count;
            double score = (double)correctCount / totalQuestions * 100;

            // Truyền dữ liệu vào View để hiển thị kết quả
            ViewData["Score"] = score;
            ViewData["CorrectCount"] = correctCount;
            ViewData["TotalQuestions"] = totalQuestions;

            return View("Result", grammar);
        }

    }
}
