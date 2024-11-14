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
    public class VocabulariesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VocabulariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Vocabularies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vocabularies.Include(v => v.Topic);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Vocabularies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabulary = await _context.Vocabularies
                .Include(v => v.Topic)
                .FirstOrDefaultAsync(m => m.VocabularyID == id);
            if (vocabulary == null)
            {
                return NotFound();
            }

            return View(vocabulary);
        }

        private async Task<string> SaveAudioFileAsync(IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return null;
            }
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/audio");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }
            var filePath = Path.Combine(uploadsFolderPath, audioFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await audioFile.CopyToAsync(stream);
            }
            return "/audio/" + audioFile.FileName;
        }

        // GET: Admin/Vocabularies/Create
        public IActionResult Create()
        {
            ViewData["TopicID"] = new SelectList(_context.Topics, "TopicID", "TopicID");
            return View();
        }

        // POST: Admin/Vocabularies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VocabularyID,Word,Meaning,Pronunciation,ExampleSentence,AudioFile,PartOfSpeech,TopicID")] Vocabulary vocabulary, IFormFile audioFile)
        {
            if (ModelState.IsValid)
            {
                if (audioFile != null && audioFile.Length > 0)
                {
                    vocabulary.AudioFile = await SaveAudioFileAsync(audioFile);
                }
                _context.Add(vocabulary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TopicID"] = new SelectList(_context.Topics, "TopicID", "TopicID", vocabulary.TopicID);
            return View(vocabulary);
        }

        // GET: Admin/Vocabularies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabulary = await _context.Vocabularies.FindAsync(id);
            if (vocabulary == null)
            {
                return NotFound();
            }
            ViewData["TopicID"] = new SelectList(_context.Topics, "TopicID", "TopicID", vocabulary.TopicID);
            return View(vocabulary);
        }

        // POST: Admin/Vocabularies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VocabularyID,Word,Meaning,Pronunciation,ExampleSentence,AudioFile,PartOfSpeech,TopicID")] Vocabulary vocabulary, IFormFile audioFile)
        {
            if (id != vocabulary.VocabularyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (audioFile != null && audioFile.Length > 0)
                    {
                        vocabulary.AudioFile = await SaveAudioFileAsync(audioFile);
                    }
                    _context.Update(vocabulary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VocabularyExists(vocabulary.VocabularyID))
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
            ViewData["TopicID"] = new SelectList(_context.Topics, "TopicID", "TopicID", vocabulary.TopicID);
            return View(vocabulary);
        }

        // GET: Admin/Vocabularies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vocabulary = await _context.Vocabularies
                .Include(v => v.Topic)
                .FirstOrDefaultAsync(m => m.VocabularyID == id);
            if (vocabulary == null)
            {
                return NotFound();
            }

            return View(vocabulary);
        }

        // POST: Admin/Vocabularies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vocabulary = await _context.Vocabularies.FindAsync(id);
            if (vocabulary != null)
            {
                _context.Vocabularies.Remove(vocabulary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VocabularyExists(int id)
        {
            return _context.Vocabularies.Any(e => e.VocabularyID == id);
        }
    }
}
