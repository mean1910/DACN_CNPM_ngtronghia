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
    public class VocabExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VocabExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/VocabExercises
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Exercises.Include(v => v.Topic);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/VocabExercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabExercise = await _context.Exercises
                .Include(v => v.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocabExercise == null)
            {
                return NotFound();
            }

            return View(vocabExercise);
        }

        // GET: Admin/VocabExercises/Create
        public IActionResult Create()
        {
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicID", "TopicID");
            return View();
        }

        // POST: Admin/VocabExercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,TopicId")] VocabExercise vocabExercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vocabExercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicID", "TopicID", vocabExercise.TopicId);
            return View(vocabExercise);
        }

        // GET: Admin/VocabExercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabExercise = await _context.Exercises.FindAsync(id);
            if (vocabExercise == null)
            {
                return NotFound();
            }
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicID", "TopicID", vocabExercise.TopicId);
            return View(vocabExercise);
        }

        // POST: Admin/VocabExercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,TopicId")] VocabExercise vocabExercise)
        {
            if (id != vocabExercise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vocabExercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VocabExerciseExists(vocabExercise.Id))
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
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicID", "TopicID", vocabExercise.TopicId);
            return View(vocabExercise);
        }

        // GET: Admin/VocabExercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabExercise = await _context.Exercises
                .Include(v => v.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocabExercise == null)
            {
                return NotFound();
            }

            return View(vocabExercise);
        }

        // POST: Admin/VocabExercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vocabExercise = await _context.Exercises.FindAsync(id);
            if (vocabExercise != null)
            {
                _context.Exercises.Remove(vocabExercise);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VocabExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}
