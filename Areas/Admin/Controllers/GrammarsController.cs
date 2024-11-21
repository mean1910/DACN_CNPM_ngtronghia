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
    public class GrammarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GrammarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Grammars
        public async Task<IActionResult> Index()
        {
            return View(await _context.Grammars.ToListAsync());
        }

        // GET: Admin/Grammars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grammar = await _context.Grammars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grammar == null)
            {
                return NotFound();
            }

            return View(grammar);
        }

        // GET: Admin/Grammars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Grammars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content")] Grammar grammar)
        {
            if (ModelState.IsValid)
            {
           
                
                _context.Add(grammar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(grammar);
        }

        // GET: Admin/Grammars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grammar = await _context.Grammars.FindAsync(id);
            if (grammar == null)
            {
                return NotFound();
            }
            return View(grammar);
        }

        // POST: Admin/Grammars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] Grammar grammar)
        {
            if (id != grammar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grammar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrammarExists(grammar.Id))
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
            return View(grammar);
        }

        // GET: Admin/Grammars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grammar = await _context.Grammars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grammar == null)
            {
                return NotFound();
            }

            return View(grammar);
        }

        // POST: Admin/Grammars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grammar = await _context.Grammars.FindAsync(id);
            if (grammar != null)
            {
                _context.Grammars.Remove(grammar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GrammarExists(int id)
        {
            return _context.Grammars.Any(e => e.Id == id);
        }
    }
}
