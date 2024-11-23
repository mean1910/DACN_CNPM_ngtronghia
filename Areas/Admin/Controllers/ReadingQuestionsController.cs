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
    public class ReadingQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReadingQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ReadingQuestions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ReadingQuestions.Include(r => r.ReadingSkill);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/ReadingQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingQuestion = await _context.ReadingQuestions
                .Include(r => r.ReadingSkill)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (readingQuestion == null)
            {
                return NotFound();
            }

            return View(readingQuestion);
        }

        // GET: Admin/ReadingQuestions/Create
        public IActionResult Create()
        {
            ViewData["ReadingSkillId"] = new SelectList(_context.ReadingSkills, "Id", "Id");
            return View();
        }

        // POST: Admin/ReadingQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,ReadingSkillId")] ReadingQuestion readingQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(readingQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReadingSkillId"] = new SelectList(_context.ReadingSkills, "Id", "Id", readingQuestion.ReadingSkillId);
            return View(readingQuestion);
        }

        // GET: Admin/ReadingQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingQuestion = await _context.ReadingQuestions.FindAsync(id);
            if (readingQuestion == null)
            {
                return NotFound();
            }
            ViewData["ReadingSkillId"] = new SelectList(_context.ReadingSkills, "Id", "Id", readingQuestion.ReadingSkillId);
            return View(readingQuestion);
        }

        // POST: Admin/ReadingQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,ReadingSkillId")] ReadingQuestion readingQuestion)
        {
            if (id != readingQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(readingQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReadingQuestionExists(readingQuestion.Id))
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
            ViewData["ReadingSkillId"] = new SelectList(_context.ReadingSkills, "Id", "Id", readingQuestion.ReadingSkillId);
            return View(readingQuestion);
        }

        // GET: Admin/ReadingQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingQuestion = await _context.ReadingQuestions
                .Include(r => r.ReadingSkill)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (readingQuestion == null)
            {
                return NotFound();
            }

            return View(readingQuestion);
        }

        // POST: Admin/ReadingQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var readingQuestion = await _context.ReadingQuestions.FindAsync(id);
            if (readingQuestion != null)
            {
                _context.ReadingQuestions.Remove(readingQuestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReadingQuestionExists(int id)
        {
            return _context.ReadingQuestions.Any(e => e.Id == id);
        }
    }
}
