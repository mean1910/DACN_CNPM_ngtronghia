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
    public class GrammarOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GrammarOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/GrammarOptions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GrammarOptions.Include(g => g.Question);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/GrammarOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grammarOption = await _context.GrammarOptions
                .Include(g => g.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grammarOption == null)
            {
                return NotFound();
            }

            return View(grammarOption);
        }

        // GET: Admin/GrammarOptions/Create
        public IActionResult Create()
        {
            ViewData["GrammarQuestionId"] = new SelectList(_context.GrammarQuestions, "Id", "Id");
            return View();
        }

        // POST: Admin/GrammarOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,IsCorrect,GrammarQuestionId")] GrammarOption grammarOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grammarOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrammarQuestionId"] = new SelectList(_context.GrammarQuestions, "Id", "Id", grammarOption.GrammarQuestionId);
            return View(grammarOption);
        }

        // GET: Admin/GrammarOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grammarOption = await _context.GrammarOptions.FindAsync(id);
            if (grammarOption == null)
            {
                return NotFound();
            }
            ViewData["GrammarQuestionId"] = new SelectList(_context.GrammarQuestions, "Id", "Id", grammarOption.GrammarQuestionId);
            return View(grammarOption);
        }

        // POST: Admin/GrammarOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,IsCorrect,GrammarQuestionId")] GrammarOption grammarOption)
        {
            if (id != grammarOption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grammarOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrammarOptionExists(grammarOption.Id))
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
            ViewData["GrammarQuestionId"] = new SelectList(_context.GrammarQuestions, "Id", "Id", grammarOption.GrammarQuestionId);
            return View(grammarOption);
        }

        // GET: Admin/GrammarOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grammarOption = await _context.GrammarOptions
                .Include(g => g.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grammarOption == null)
            {
                return NotFound();
            }

            return View(grammarOption);
        }

        // POST: Admin/GrammarOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grammarOption = await _context.GrammarOptions.FindAsync(id);
            if (grammarOption != null)
            {
                _context.GrammarOptions.Remove(grammarOption);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GrammarOptionExists(int id)
        {
            return _context.GrammarOptions.Any(e => e.Id == id);
        }
    }
}
