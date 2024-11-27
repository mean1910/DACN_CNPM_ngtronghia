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
    public class ListeningOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListeningOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ListeningOptions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ListeningsOptions.Include(l => l.Question);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/ListeningOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listeningOption = await _context.ListeningsOptions
                .Include(l => l.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listeningOption == null)
            {
                return NotFound();
            }

            return View(listeningOption);
        }

        // GET: Admin/ListeningOptions/Create
        public IActionResult Create()
        {
            ViewData["ListeningQuestionId"] = new SelectList(_context.ListeningsQuestions, "Id", "Id");
            return View();
        }

        // POST: Admin/ListeningOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,IsCorrect,ListeningQuestionId")] ListeningOption listeningOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listeningOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ListeningQuestionId"] = new SelectList(_context.ListeningsQuestions, "Id", "Id", listeningOption.ListeningQuestionId);
            return View(listeningOption);
        }

        // GET: Admin/ListeningOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listeningOption = await _context.ListeningsOptions.FindAsync(id);
            if (listeningOption == null)
            {
                return NotFound();
            }
            ViewData["ListeningQuestionId"] = new SelectList(_context.ListeningsQuestions, "Id", "Id", listeningOption.ListeningQuestionId);
            return View(listeningOption);
        }

        // POST: Admin/ListeningOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,IsCorrect,ListeningQuestionId")] ListeningOption listeningOption)
        {
            if (id != listeningOption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listeningOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListeningOptionExists(listeningOption.Id))
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
            ViewData["ListeningQuestionId"] = new SelectList(_context.ListeningsQuestions, "Id", "Id", listeningOption.ListeningQuestionId);
            return View(listeningOption);
        }

        // GET: Admin/ListeningOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listeningOption = await _context.ListeningsOptions
                .Include(l => l.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listeningOption == null)
            {
                return NotFound();
            }

            return View(listeningOption);
        }

        // POST: Admin/ListeningOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listeningOption = await _context.ListeningsOptions.FindAsync(id);
            if (listeningOption != null)
            {
                _context.ListeningsOptions.Remove(listeningOption);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListeningOptionExists(int id)
        {
            return _context.ListeningsOptions.Any(e => e.Id == id);
        }
    }
}
