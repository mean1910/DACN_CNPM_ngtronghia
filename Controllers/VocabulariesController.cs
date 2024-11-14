using elearning_b1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace elearning_b1.Controllers
{
    public class VocabulariesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VocabulariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy danh sách tất cả các topic
            var topics = _context.Topics.ToList();
            return View(topics); // Gửi danh sách topics về View
        }
        public IActionResult VocabularyList(int topicId)
        {

            // Lấy danh sách từ vựng theo topicId
            var vocabularies = _context.Vocabularies
                .Where(v => v.TopicID == topicId)
                .Include(v => v.Topic)
                .ToList();

            foreach (var vocab in vocabularies)
            {
                if (!string.IsNullOrEmpty(vocab.PartOfSpeech.ToString()))
                {
                    vocab.PartOfSpeech = (PartOfSpeech)Enum.Parse(typeof(PartOfSpeech), vocab.PartOfSpeech.ToString());
                }
            }

            // Gửi danh sách từ vựng về View
            return View(vocabularies);
        }
        public IActionResult Flashcards()
        {
            var vocabList = _context.Vocabularies.Include(v => v.Topic).ToList();
            return View(vocabList);  // Đảm bảo rằng bạn đang trả về một danh sách đầy đủ
        }

    }
}
