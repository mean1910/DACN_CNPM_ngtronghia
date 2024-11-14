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
    public class AdVocabulariesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdVocabulariesController(ApplicationDbContext context)
        {
            _context = context;
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


        // GET: Admin/AdVocabularies/Details/5
        public async Task<IActionResult> Details(int? id, int? topicId)
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
            ViewBag.ReturnTopicId = topicId;
            return View(vocabulary);
        }

        // GET: Admin/AdVocabularies/Create
        public IActionResult Create(int? topicId)
        {
            ViewData["TopicID"] = new SelectList(_context.Topics, "TopicID", "TopicName");
            ViewBag.ReturnTopicId = topicId;
            return View();
        }

        // POST: Admin/AdVocabularies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VocabularyID,Word,Meaning,Pronunciation,ExampleSentence,AudioFile,PartOfSpeech,TopicID")] Vocabulary vocabulary, int? topicId, IFormFile audioFile)
        {
            if (ModelState.IsValid)
            {
                if (audioFile != null && audioFile.Length > 0)
                {
                    vocabulary.AudioFile = await SaveAudioFileAsync(audioFile);
                }
                _context.Add(vocabulary);
                await _context.SaveChangesAsync();
                return RedirectToAction("ByTopic", new { topicId = topicId });
            }
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            ViewData["TopicID"] = new SelectList(_context.Topics, "TopicID", "TopicName", vocabulary.TopicID);
            ViewBag.ReturnTopicId = topicId; 
            return View(vocabulary);
        }

        // GET: Admin/AdVocabularies/Edit/5
        public async Task<IActionResult> Edit(int? id, int? topicId)
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
            ViewData["TopicID"] = new SelectList(_context.Topics, "TopicID", "TopicName", vocabulary.TopicID);
            ViewBag.ReturnTopicId = topicId;
            return View(vocabulary);
        }

        // POST: Admin/AdVocabularies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VocabularyID,Word,Meaning,Pronunciation,ExampleSentence,AudioFile,PartOfSpeech,TopicID")] Vocabulary vocabulary, int? topicId, IFormFile audioFile)
        {
            // Kiểm tra xem id có khớp với VocabularyID trong dữ liệu không
            if (id != vocabulary.VocabularyID)
            {
                return NotFound(); // Nếu id không khớp, trả về lỗi không tìm thấy
            }

            // Kiểm tra dữ liệu hợp lệ
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra nếu có tệp âm thanh được tải lên và xử lý
                    if (audioFile != null && audioFile.Length > 0)
                    {
                        vocabulary.AudioFile = await SaveAudioFileAsync(audioFile); // Lưu tệp âm thanh
                    }

                    // Cập nhật từ vựng trong cơ sở dữ liệu
                    _context.Update(vocabulary);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Edit", "AdVocabularies", new { area = "Admin", id = vocabulary.VocabularyID, topicId = vocabulary.TopicID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VocabularyExists(vocabulary.VocabularyID))
                    {
                        return NotFound(); // Nếu không tìm thấy từ vựng cần sửa
                    }
                    else
                    {
                        throw; // Xử lý lỗi nếu có
                    }
                }
            }

            // Nếu dữ liệu không hợp lệ, chuẩn bị lại các dữ liệu để hiển thị trên form
            ViewData["TopicID"] = new SelectList(_context.Topics, "TopicID", "TopicName", vocabulary.TopicID);
            ViewBag.ReturnTopicId = topicId;

            return View(vocabulary); // Trả lại view Edit nếu có lỗi trong việc xác thực dữ liệu
        }



        // GET: Admin/AdVocabularies/Delete/5
        public async Task<IActionResult> Delete(int? id, int? topicId)
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
            ViewBag.ReturnTopicId = topicId;
            return View(vocabulary);
        }

        // POST: Admin/AdVocabularies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int? topicId)
        {
            var vocabulary = await _context.Vocabularies.FindAsync(id);
            if (vocabulary != null)
            {
                _context.Vocabularies.Remove(vocabulary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ByTopic", new { topicId = topicId });
        }

        private bool VocabularyExists(int id)
        {
            return _context.Vocabularies.Any(e => e.VocabularyID == id);
        }

        // GET: Admin/AdVocabularies/ByTopic/5
        public async Task<IActionResult> ByTopic(int? topicId)
        {
            if (topicId == null)
            {
                return NotFound();
            }

            var vocabularies = await _context.Vocabularies
                                     .Where(v => v.TopicID == topicId)
                                     .Include(v => v.Topic)
                                     .ToListAsync();

            if (!vocabularies.Any())
            {
                return NotFound("No vocabularies found for this topic.");
            }

            return View(vocabularies);
        }

    }
}
