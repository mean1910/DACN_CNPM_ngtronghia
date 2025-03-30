using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using elearning_b1.Models;

namespace elearning_b1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VocabQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VocabQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/VocabQuestions
        public async Task<IActionResult> Index(int topicId)
        {
            // Lọc câu hỏi từ vựng theo TopicID
            var vocabQuestions = await _context.Questions
                .Where(v => v.TopicId == topicId)
                .ToListAsync();

            // Lấy thông tin tên chủ đề
            ViewBag.TopicId = topicId;
            ViewBag.TopicName = await _context.Topics
                .Where(t => t.TopicID == topicId)
                .Select(t => t.TopicName)
                .FirstOrDefaultAsync();

            return View(vocabQuestions);
        }

        // GET: Admin/VocabQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Tìm câu hỏi
            var vocabQuestion = await _context.Questions
                .Include(q => q.Options)  // Đảm bảo các lựa chọn được load
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vocabQuestion == null)
            {
                return NotFound();
            }

            ViewData["QuestionId"] = vocabQuestion.Id;
            return View(vocabQuestion);
        }


        // GET: Admin/VocabQuestions/Create
        public IActionResult Create(int topicId)
        {
            // Lấy tên chủ đề
            var topicName = _context.Topics
                .Where(t => t.TopicID == topicId)
                .Select(t => t.TopicName)
                .FirstOrDefault();

            ViewBag.TopicId = topicId;
            ViewBag.TopicName = topicName; // Thêm tên chủ đề
            return View();
        }



        // POST: Admin/VocabQuestions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,TopicId")] VocabQuestion vocabQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vocabQuestion);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { topicId = vocabQuestion.TopicId });
            }

            ViewBag.TopicId = vocabQuestion.TopicId;
            return View(vocabQuestion);
        }

        // GET: Admin/VocabQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id, int topicId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabQuestion = await _context.Questions.FindAsync(id);
            if (vocabQuestion == null)
            {
                return NotFound();
            }

            // Lấy tên chủ đề
            var topicName = _context.Topics
                .Where(t => t.TopicID == topicId)
                .Select(t => t.TopicName)
                .FirstOrDefault();

            ViewBag.TopicId = topicId;
            ViewBag.TopicName = topicName; // Thêm tên chủ đề
            return View(vocabQuestion);
        }


        // POST: Admin/VocabQuestions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,TopicId")] VocabQuestion vocabQuestion)
        {
            if (id != vocabQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vocabQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VocabQuestionExists(vocabQuestion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index), new { topicId = vocabQuestion.TopicId });
            }

            ViewBag.TopicId = vocabQuestion.TopicId;
            return View(vocabQuestion);
        }

        // GET: Admin/VocabQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id, int topicId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabQuestion = await _context.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocabQuestion == null)
            {
                return NotFound();
            }

            ViewBag.TopicId = topicId;
            return View(vocabQuestion);
        }

        // POST: Admin/VocabQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int topicId)
        {
            var vocabQuestion = await _context.Questions.FindAsync(id);
            if (vocabQuestion != null)
            {
                _context.Questions.Remove(vocabQuestion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { topicId = topicId });

        }

        private bool VocabQuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
