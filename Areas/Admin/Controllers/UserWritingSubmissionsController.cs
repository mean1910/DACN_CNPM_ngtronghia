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
    public class UserWritingSubmissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserWritingSubmissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/UserWritingSubmissions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Submissions.Include(u => u.ApplicationUser).Include(u => u.Writing);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/UserWritingSubmissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userWritingSubmission = await _context.Submissions
                .Include(u => u.ApplicationUser)
                .Include(u => u.Writing)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userWritingSubmission == null)
            {
                return NotFound();
            }

            return View(userWritingSubmission);
        }

        // GET: Admin/UserWritingSubmissions/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["WritingId"] = new SelectList(_context.Writings, "Id", "Id");
            return View();
        }

        // POST: Admin/UserWritingSubmissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WritingId,ApplicationUserId,UserAnswer,SubmittedAt")] UserWritingSubmission userWritingSubmission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userWritingSubmission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", userWritingSubmission.ApplicationUserId);
            ViewData["WritingId"] = new SelectList(_context.Writings, "Id", "Id", userWritingSubmission.WritingId);
            return View(userWritingSubmission);
        }

        // GET: Admin/UserWritingSubmissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userWritingSubmission = await _context.Submissions.FindAsync(id);
            if (userWritingSubmission == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", userWritingSubmission.ApplicationUserId);
            ViewData["WritingId"] = new SelectList(_context.Writings, "Id", "Id", userWritingSubmission.WritingId);
            return View(userWritingSubmission);
        }

        // POST: Admin/UserWritingSubmissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WritingId,ApplicationUserId,UserAnswer,SubmittedAt")] UserWritingSubmission userWritingSubmission)
        {
            if (id != userWritingSubmission.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userWritingSubmission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserWritingSubmissionExists(userWritingSubmission.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", userWritingSubmission.ApplicationUserId);
            ViewData["WritingId"] = new SelectList(_context.Writings, "Id", "Id", userWritingSubmission.WritingId);
            return View(userWritingSubmission);
        }

        // GET: Admin/UserWritingSubmissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userWritingSubmission = await _context.Submissions
                .Include(u => u.ApplicationUser)
                .Include(u => u.Writing)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userWritingSubmission == null)
            {
                return NotFound();
            }

            return View(userWritingSubmission);
        }

        // POST: Admin/UserWritingSubmissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userWritingSubmission = await _context.Submissions.FindAsync(id);
            if (userWritingSubmission != null)
            {
                _context.Submissions.Remove(userWritingSubmission);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserWritingSubmissionExists(int id)
        {
            return _context.Submissions.Any(e => e.Id == id);
        }
    }
}
