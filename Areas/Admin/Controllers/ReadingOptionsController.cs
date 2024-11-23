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
    public class ReadingOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReadingOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ReadingOptions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ReadingOptions.Include(r => r.Question);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/ReadingOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingOption = await _context.ReadingOptions
                .Include(r => r.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (readingOption == null)
            {
                return NotFound();
            }

            return View(readingOption);
        }

        // GET: Admin/ReadingOptions/Create
        public IActionResult Create()
        {
            ViewData["ReadingQuestionId"] = new SelectList(_context.ReadingQuestions, "Id", "Id");
            return View();
        }

        // POST: Admin/ReadingOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,IsCorrect,ReadingQuestionId")] ReadingOption readingOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(readingOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReadingQuestionId"] = new SelectList(_context.ReadingQuestions, "Id", "Id", readingOption.ReadingQuestionId);
            return View(readingOption);
        }

        // GET: Admin/ReadingOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingOption = await _context.ReadingOptions.FindAsync(id);
            if (readingOption == null)
            {
                return NotFound();
            }
            ViewData["ReadingQuestionId"] = new SelectList(_context.ReadingQuestions, "Id", "Id", readingOption.ReadingQuestionId);
            return View(readingOption);
        }

        // POST: Admin/ReadingOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,IsCorrect,ReadingQuestionId")] ReadingOption readingOption)
        {
            if (id != readingOption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(readingOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReadingOptionExists(readingOption.Id))
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
            ViewData["ReadingQuestionId"] = new SelectList(_context.ReadingQuestions, "Id", "Id", readingOption.ReadingQuestionId);
            return View(readingOption);
        }

        // GET: Admin/ReadingOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingOption = await _context.ReadingOptions
                .Include(r => r.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (readingOption == null)
            {
                return NotFound();
            }

            return View(readingOption);
        }

        // POST: Admin/ReadingOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var readingOption = await _context.ReadingOptions.FindAsync(id);
            if (readingOption != null)
            {
                _context.ReadingOptions.Remove(readingOption);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReadingOptionExists(int id)
        {
            return _context.ReadingOptions.Any(e => e.Id == id);
        }
    }
}
