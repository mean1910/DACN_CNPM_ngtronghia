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
    public class ReadingSkillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReadingSkillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ReadingSkills
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReadingSkills.ToListAsync());
        }

        // GET: Admin/ReadingSkills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingSkill = await _context.ReadingSkills
                .FirstOrDefaultAsync(m => m.Id == id);
            if (readingSkill == null)
            {
                return NotFound();
            }

            return View(readingSkill);
        }

        // GET: Admin/ReadingSkills/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ReadingSkills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content")] ReadingSkill readingSkill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(readingSkill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(readingSkill);
        }

        // GET: Admin/ReadingSkills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingSkill = await _context.ReadingSkills.FindAsync(id);
            if (readingSkill == null)
            {
                return NotFound();
            }
            return View(readingSkill);
        }

        // POST: Admin/ReadingSkills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] ReadingSkill readingSkill)
        {
            if (id != readingSkill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(readingSkill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReadingSkillExists(readingSkill.Id))
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
            return View(readingSkill);
        }

        // GET: Admin/ReadingSkills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingSkill = await _context.ReadingSkills
                .FirstOrDefaultAsync(m => m.Id == id);
            if (readingSkill == null)
            {
                return NotFound();
            }

            return View(readingSkill);
        }

        // POST: Admin/ReadingSkills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var readingSkill = await _context.ReadingSkills.FindAsync(id);
            if (readingSkill != null)
            {
                _context.ReadingSkills.Remove(readingSkill);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReadingSkillExists(int id)
        {
            return _context.ReadingSkills.Any(e => e.Id == id);
        }
    }
}
