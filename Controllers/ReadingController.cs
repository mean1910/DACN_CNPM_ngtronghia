using elearning_b1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace elearning_b1.Controllers
{
    public class ReadingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReadingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            ViewData["CurrentController"] = "Skills";
            ViewData["CurrentAction"] = "Reading";
            var readingSkills = from r in _context.ReadingSkills
                                select r;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                readingSkills = readingSkills.Where(r => r.Title.Contains(searchQuery) || r.Content.Contains(searchQuery));
            }

            return View(await readingSkills.ToListAsync());
        }


        public async Task<IActionResult> Details(int id)
        {
            var readingSkill = _context.ReadingSkills
                .Include(rs => rs.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefault(rs => rs.Id == id);

            if (readingSkill == null)
            {
                return NotFound();
            }

            return View(readingSkill);
        }


        public IActionResult Test(int id)
        {
            var readingSkill = _context.ReadingSkills
                .Include(rs => rs.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefault(rs => rs.Id == id);

            if (readingSkill == null)
            {
                return NotFound();
            }

            return View(readingSkill);
        }

        // Action để hiển thị bài tập đọc
        public async Task<IActionResult> Exercise(int id)
        {
            // Tìm bài tập đọc theo ID
            var exercise = await _context.ReadingSkills
                .Include(r => r.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(r => r.Id == id);

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
            // Tìm bài tập đọc theo ID
            var readingSkill = await _context.ReadingSkills
                .Include(r => r.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (readingSkill == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy bài tập
            }

            // Tạo danh sách kết quả
            foreach (var question in readingSkill.Questions)
            {
                // Đánh dấu lựa chọn của người dùng và xác định đáp án đúng
                foreach (var option in question.Options)
                {
                    option.IsUserChoice = answers.TryGetValue(question.Id, out int userAnswerId) && userAnswerId == option.Id;
                }
            }

            // Tính số câu trả lời đúng
            int correctCount = readingSkill.Questions.Count(q =>
                answers.TryGetValue(q.Id, out int userAnswerId) &&
                q.Options.Any(o => o.Id == userAnswerId && o.IsCorrect));

            // Tính điểm
            int totalQuestions = readingSkill.Questions.Count;
            double score = (double)correctCount / totalQuestions * 100;

            // Truyền dữ liệu vào View để hiển thị kết quả
            ViewData["Score"] = score;
            ViewData["CorrectCount"] = correctCount;
            ViewData["TotalQuestions"] = totalQuestions;

            return View("Result", readingSkill);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetWordDefinition(string word)
        //{
        //    if (string.IsNullOrEmpty(word))
        //    {
        //        return Json(new { success = false, definition = "Vui lòng nhập từ cần tra cứu." });
        //    }

        //    string apiUrl = $"https://api.dictionaryapi.dev/api/v2/entries/en/{word}";

        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            HttpResponseMessage response = await client.GetAsync(apiUrl);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var json = await response.Content.ReadAsStringAsync();

        //                // Kiểm tra nếu phản hồi không phải là dữ liệu JSON hợp lệ
        //                if (string.IsNullOrWhiteSpace(json))
        //                {
        //                    return Json(new { success = false, definition = "Không tìm thấy nghĩa của từ này." });
        //                }

        //                var dictionaryData = JsonConvert.DeserializeObject<dynamic>(json);

        //                if (dictionaryData != null && dictionaryData.Count > 0)
        //                {
        //                    var firstMeaning = dictionaryData[0].meanings[0].definitions[0].definition.ToString();
        //                    return Json(new { success = true, definition = firstMeaning });
        //                }
        //                else
        //                {
        //                    return Json(new { success = false, definition = "Không tìm thấy nghĩa của từ này." });
        //                }
        //            }
        //            else
        //            {
        //                return Json(new { success = false, definition = "Không thể kết nối tới từ điển." });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { success = false, definition = "Đã xảy ra lỗi khi kết nối đến từ điển: " + ex.Message });
        //        }
        //    }
        //}



    }
}
