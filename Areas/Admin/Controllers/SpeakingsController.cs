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
    public class SpeakingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpeakingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Speakings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Speakings.Include(s => s.Topic);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Speakings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speaking = await _context.Speakings
                .Include(s => s.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speaking == null)
            {
                return NotFound();
            }

            return View(speaking);
        }

        // GET: Admin/Speakings/Create
        public IActionResult Create()
        {
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicID", "TopicID");
            return View();
        }

        // POST: Admin/Speakings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TopicId,Question")] Speaking speaking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(speaking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicID", "TopicID", speaking.TopicId);
            return View(speaking);
        }

        // GET: Admin/Speakings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speaking = await _context.Speakings.FindAsync(id);
            if (speaking == null)
            {
                return NotFound();
            }
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicID", "TopicID", speaking.TopicId);
            return View(speaking);
        }

        // POST: Admin/Speakings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TopicId,Question")] Speaking speaking)
        {
            if (id != speaking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speaking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpeakingExists(speaking.Id))
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
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicID", "TopicID", speaking.TopicId);
            return View(speaking);
        }

        // GET: Admin/Speakings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speaking = await _context.Speakings
                .Include(s => s.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speaking == null)
            {
                return NotFound();
            }

            return View(speaking);
        }

        // POST: Admin/Speakings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var speaking = await _context.Speakings.FindAsync(id);
            if (speaking != null)
            {
                _context.Speakings.Remove(speaking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpeakingExists(int id)
        {
            return _context.Speakings.Any(e => e.Id == id);
        }
    }
}
