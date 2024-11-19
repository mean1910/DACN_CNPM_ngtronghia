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
    public class VocabOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VocabOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/VocabOptions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Options.Include(v => v.Question);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/VocabOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabOption = await _context.Options
                .Include(v => v.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocabOption == null)
            {
                return NotFound();
            }

            return View(vocabOption);
        }

        // GET: Admin/VocabOptions/Create
        public IActionResult Create()
        {
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id");
            return View();
        }

        // POST: Admin/VocabOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,IsCorrect,QuestionId")] VocabOption vocabOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vocabOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", vocabOption.QuestionId);
            return View(vocabOption);
        }

        // GET: Admin/VocabOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabOption = await _context.Options.FindAsync(id);
            if (vocabOption == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", vocabOption.QuestionId);
            return View(vocabOption);
        }

        // POST: Admin/VocabOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    _context.Update(vocabOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VocabOptionExists(vocabOption.Id))
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
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", vocabOption.QuestionId);
            return View(vocabOption);
        }

        // GET: Admin/VocabOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabOption = await _context.Options
                .Include(v => v.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocabOption == null)
            {
                return NotFound();
            }

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
                _context.Options.Remove(vocabOption);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VocabOptionExists(int id)
        {
            return _context.Options.Any(e => e.Id == id);
        }
    }
}
