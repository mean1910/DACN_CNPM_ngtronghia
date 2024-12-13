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
    public class WritingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WritingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Writings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Writings.ToListAsync());
        }

        // GET: Admin/Writings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var writing = await _context.Writings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (writing == null)
            {
                return NotFound();
            }

            return View(writing);
        }

        // GET: Admin/Writings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Writings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Prompt,Suggestions")] Writing writing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(writing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(writing);
        }

        // GET: Admin/Writings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var writing = await _context.Writings.FindAsync(id);
            if (writing == null)
            {
                return NotFound();
            }
            return View(writing);
        }

        // POST: Admin/Writings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Prompt,Suggestions")] Writing writing)
        {
            if (id != writing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(writing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WritingExists(writing.Id))
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
            return View(writing);
        }

        // GET: Admin/Writings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var writing = await _context.Writings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (writing == null)
            {
                return NotFound();
            }

            return View(writing);
        }

        // POST: Admin/Writings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var writing = await _context.Writings.FindAsync(id);
            if (writing != null)
            {
                _context.Writings.Remove(writing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WritingExists(int id)
        {
            return _context.Writings.Any(e => e.Id == id);
        }
    }
}
