using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Questions.Include(v => v.Exercise);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/VocabQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabQuestion = await _context.Questions
                .Include(v => v.Exercise)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocabQuestion == null)
            {
                return NotFound();
            }

            return View(vocabQuestion);
        }

        // GET: Admin/VocabQuestions/Create
        public IActionResult Create()
        {
            ViewData["ExerciseId"] = new SelectList(_context.Exercises, "Id", "Id");
            return View();
        }

        // POST: Admin/VocabQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,ExerciseId")] VocabQuestion vocabQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vocabQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseId"] = new SelectList(_context.Exercises, "Id", "Id", vocabQuestion.ExerciseId);
            return View(vocabQuestion);
        }

        // GET: Admin/VocabQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["ExerciseId"] = new SelectList(_context.Exercises, "Id", "Id", vocabQuestion.ExerciseId);
            return View(vocabQuestion);
        }

        // POST: Admin/VocabQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,ExerciseId")] VocabQuestion vocabQuestion)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseId"] = new SelectList(_context.Exercises, "Id", "Id", vocabQuestion.ExerciseId);
            return View(vocabQuestion);
        }

        // GET: Admin/VocabQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabQuestion = await _context.Questions
                .Include(v => v.Exercise)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocabQuestion == null)
            {
                return NotFound();
            }

            return View(vocabQuestion);
        }

        // POST: Admin/VocabQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vocabQuestion = await _context.Questions.FindAsync(id);
            if (vocabQuestion != null)
            {
                _context.Questions.Remove(vocabQuestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VocabQuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
