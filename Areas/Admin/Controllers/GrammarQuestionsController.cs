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
    public class GrammarQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GrammarQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/GrammarQuestions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GrammarQuestions.Include(g => g.Grammar);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/GrammarQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grammarQuestion = await _context.GrammarQuestions
                .Include(g => g.Grammar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grammarQuestion == null)
            {
                return NotFound();
            }

            return View(grammarQuestion);
        }

        // GET: Admin/GrammarQuestions/Create
        public IActionResult Create()
        {
            ViewData["GrammarId"] = new SelectList(_context.Grammars, "Id", "Id");
            return View();
        }

        // POST: Admin/GrammarQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,GrammarId")] GrammarQuestion grammarQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grammarQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrammarId"] = new SelectList(_context.Grammars, "Id", "Id", grammarQuestion.GrammarId);
            return View(grammarQuestion);
        }

        // GET: Admin/GrammarQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grammarQuestion = await _context.GrammarQuestions.FindAsync(id);
            if (grammarQuestion == null)
            {
                return NotFound();
            }
            ViewData["GrammarId"] = new SelectList(_context.Grammars, "Id", "Id", grammarQuestion.GrammarId);
            return View(grammarQuestion);
        }

        // POST: Admin/GrammarQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,GrammarId")] GrammarQuestion grammarQuestion)
        {
            if (id != grammarQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grammarQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrammarQuestionExists(grammarQuestion.Id))
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
            ViewData["GrammarId"] = new SelectList(_context.Grammars, "Id", "Id", grammarQuestion.GrammarId);
            return View(grammarQuestion);
        }

        // GET: Admin/GrammarQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grammarQuestion = await _context.GrammarQuestions
                .Include(g => g.Grammar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grammarQuestion == null)
            {
                return NotFound();
            }

            return View(grammarQuestion);
        }

        // POST: Admin/GrammarQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grammarQuestion = await _context.GrammarQuestions.FindAsync(id);
            if (grammarQuestion != null)
            {
                _context.GrammarQuestions.Remove(grammarQuestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GrammarQuestionExists(int id)
        {
            return _context.GrammarQuestions.Any(e => e.Id == id);
        }
    }
}
