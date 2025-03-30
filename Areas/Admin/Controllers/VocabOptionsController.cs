using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using elearning_b1.Models;

namespace elearning_b1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VocabOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VocabOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/VocabOptions
        public IActionResult Index(int questionId)
        {
            var vocabOptions = _context.Options
                .AsNoTracking()
                .Where(o => o.QuestionId == questionId)
                .OrderBy(o => o.Id)
                .ToList();

            ViewData["QuestionId"] = questionId;
            ViewData["TopicId"] = _context.Questions
               .Where(q => q.Id == questionId)
               .Select(q => q.TopicId)
               .FirstOrDefault();
            return View(vocabOptions);
        }

        // GET: Admin/VocabOptions/Create
        public IActionResult Create(int questionId)
        {
            LoadQuestionSelectList(questionId);
            return View();
        }

        // POST: Admin/VocabOptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,IsCorrect,QuestionId")] VocabOption vocabOption)
        {
            if (ModelState.IsValid)
            {
                var existingEntity = _context.Options.Local.FirstOrDefault(o => o.Id == vocabOption.Id);
                if (existingEntity == null)
                {
                    _context.Add(vocabOption);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Option created successfully!";
                }
                else
                {
                    TempData["Error"] = "This option already exists.";
                }
                return RedirectToAction(nameof(Index), new { questionId = vocabOption.QuestionId });
            }

            // Nếu có lỗi, truyền lại SelectList
            LoadQuestionSelectList(vocabOption.QuestionId);
            return View(vocabOption);
        }

        // GET: Admin/VocabOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabOption = await _context.Options.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
            if (vocabOption == null)
            {
                return NotFound();
            }

            LoadQuestionSelectList(vocabOption.QuestionId);
            return View(vocabOption);
        }

        // POST: Admin/VocabOptions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,IsCorrect,QuestionId")] VocabOption vocabOption)
        {
            if (id != vocabOption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingOption = await _context.Options.FindAsync(id);
                    if (existingOption != null)
                    {
                        // Cập nhật các trường thay đổi
                        existingOption.Text = vocabOption.Text;
                        existingOption.IsCorrect = vocabOption.IsCorrect;

                        // Không cần gọi Update vì thực thể đang được theo dõi
                        await _context.SaveChangesAsync();
                        TempData["Message"] = "Option updated successfully!";
                    }
                    else
                    {
                        TempData["Error"] = "Option not found.";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VocabOptionExists(vocabOption.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index), new { questionId = vocabOption.QuestionId });
            }

            // Nếu có lỗi, truyền lại SelectList
            LoadQuestionSelectList(vocabOption.QuestionId);
            return View(vocabOption);
        }

        // GET: Admin/VocabOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabOption = await _context.Options.AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (vocabOption == null)
            {
                return NotFound();
            }

            ViewData["QuestionId"] = vocabOption.QuestionId;
            return View(vocabOption);
        }

        // POST: Admin/VocabOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vocabOption = await _context.Options.FindAsync(id);
            if (vocabOption != null)
            {
                var questionId = vocabOption.QuestionId;
                _context.Options.Remove(vocabOption);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Option deleted successfully!";
                return RedirectToAction(nameof(Index), new { questionId = questionId });
            }

            TempData["Error"] = "Option not found.";
            return RedirectToAction(nameof(Index));
        }

        private bool VocabOptionExists(int id)
        {
            return _context.Options.Any(o => o.Id == id);
        }

        // Phương thức phụ để tải danh sách câu hỏi vào SelectList
        private void LoadQuestionSelectList(int? selectedQuestionId = null)
        {
            var questions = _context.Questions.AsNoTracking().ToList();
            ViewData["QuestionId"] = new SelectList(questions, "Id", "Text", selectedQuestionId);
        }
    }
}
