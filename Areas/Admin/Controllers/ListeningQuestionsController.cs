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
    public class ListeningQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListeningQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ListeningQuestions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ListeningsQuestions.Include(l => l.Listening);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/ListeningQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listeningQuestion = await _context.ListeningsQuestions
                .Include(l => l.Listening)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listeningQuestion == null)
            {
                return NotFound();
            }

            return View(listeningQuestion);
        }

        // GET: Admin/ListeningQuestions/Create
        public IActionResult Create()
        {
            ViewData["ListeningId"] = new SelectList(_context.Listenings, "Id", "Title");
            return View();
        }

        // POST: Admin/ListeningQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,ListeningId")] ListeningQuestion listeningQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listeningQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ListeningId"] = new SelectList(_context.Listenings, "Id", "Title", listeningQuestion.ListeningId);
            return View(listeningQuestion);
        }

        // GET: Admin/ListeningQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listeningQuestion = await _context.ListeningsQuestions.FindAsync(id);
            if (listeningQuestion == null)
            {
                return NotFound();
            }
            ViewData["ListeningId"] = new SelectList(_context.Listenings, "Id", "Title", listeningQuestion.ListeningId);
            return View(listeningQuestion);
        }

        // POST: Admin/ListeningQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,ListeningId")] ListeningQuestion listeningQuestion)
        {
            if (id != listeningQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listeningQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListeningQuestionExists(listeningQuestion.Id))
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
            ViewData["ListeningId"] = new SelectList(_context.Listenings, "Id", "Title", listeningQuestion.ListeningId);
            return View(listeningQuestion);
        }

        // GET: Admin/ListeningQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listeningQuestion = await _context.ListeningsQuestions
                .Include(l => l.Listening)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listeningQuestion == null)
            {
                return NotFound();
            }

            return View(listeningQuestion);
        }

        // POST: Admin/ListeningQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listeningQuestion = await _context.ListeningsQuestions.FindAsync(id);
            if (listeningQuestion != null)
            {
                _context.ListeningsQuestions.Remove(listeningQuestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListeningQuestionExists(int id)
        {
            return _context.ListeningsQuestions.Any(e => e.Id == id);
        }
    }
}
