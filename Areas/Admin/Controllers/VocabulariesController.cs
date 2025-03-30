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
        public async Task<IActionResult> Index(int? topicId)
        {
            if (topicId == null)
            {
                return NotFound();
            }

            ViewData["TopicID"] = topicId;
            var vocabularies = _context.Vocabularies
                .Where(v => v.TopicID == topicId)
                .Include(v => v.Topic);

            return View(await vocabularies.ToListAsync());
        }

        // GET: Admin/Vocabularies/Details/5
        public async Task<IActionResult> Details(int? id, int? topicId)
        {
            if (id == null || topicId == null)
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

            ViewData["TopicID"] = topicId;
            return View(vocabulary);
        }

        // GET: Admin/Vocabularies/Create
        public IActionResult Create(int topicId)
        {
            ViewData["PartOfSpeech"] = new SelectList(Enum.GetValues(typeof(PartOfSpeech)).Cast<PartOfSpeech>());
            ViewData["TopicID"] = topicId;
            ViewData["TopicName"] = _context.Topics.FirstOrDefault(t => t.TopicID == topicId)?.TopicName;
            return View();
        }

        // POST: Admin/Vocabularies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VocabularyID,Word,Meaning,Pronunciation,ExampleSentence,PartOfSpeech,TopicID")] Vocabulary vocabulary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vocabulary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { topicId = vocabulary.TopicID });
            }

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

            // Tạo SelectList từ enum PartOfSpeech và đặt giá trị đã chọn
            ViewData["PartOfSpeech"] = new SelectList(Enum.GetValues(typeof(PartOfSpeech))
                                                        .Cast<PartOfSpeech>(),
                                                        vocabulary.PartOfSpeech);
            ViewData["TopicID"] = vocabulary.TopicID;
            ViewData["TopicName"] = _context.Topics.FirstOrDefault(t => t.TopicID == vocabulary.TopicID)?.TopicName;

            return View(vocabulary);
        }


        // POST: Admin/Vocabularies/Edit/5
        // POST: Admin/Vocabularies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VocabularyID,Word,Meaning,Pronunciation,ExampleSentence,PartOfSpeech,TopicID")] Vocabulary vocabulary)
        {
            if (id != vocabulary.VocabularyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                // Chuyển hướng về Index sau khi lưu thay đổi
                return RedirectToAction(nameof(Index), new { topicId = vocabulary.TopicID });
            }

            // Truyền lại SelectList nếu có lỗi
            ViewData["PartOfSpeech"] = new SelectList(Enum.GetValues(typeof(PartOfSpeech))
                                                        .Cast<PartOfSpeech>(), vocabulary.PartOfSpeech);
            ViewData["TopicID"] = vocabulary.TopicID;
            ViewData["TopicName"] = _context.Topics.FirstOrDefault(t => t.TopicID == vocabulary.TopicID)?.TopicName;

            return View(vocabulary);
        }



        // GET: Admin/Vocabularies/Delete/5
        public async Task<IActionResult> Delete(int? id, int topicId)
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

            ViewData["TopicID"] = topicId;
            return View(vocabulary);
        }

        // POST: Admin/Vocabularies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int topicId)
        {
            var vocabulary = await _context.Vocabularies.FindAsync(id);
            if (vocabulary != null)
            {
                _context.Vocabularies.Remove(vocabulary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { topicId = topicId });
        }

        private bool VocabularyExists(int id)
        {
            return _context.Vocabularies.Any(e => e.VocabularyID == id);
        }
    }
}
