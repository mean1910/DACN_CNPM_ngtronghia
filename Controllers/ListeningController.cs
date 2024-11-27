using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using elearning_b1.Models;

namespace elearning_b1.Controllers
{
    public class ListeningController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListeningController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var listenings = await _context.Listenings.ToListAsync();
            return View(listenings);
        }



        // GET: ListeningUser/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy bài nghe và các câu hỏi liên quan
            var listening = await _context.Listenings
                .Include(l => l.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (listening == null)
            {
                return NotFound();
            }

            // Tách transcript thành từng câu
            var sentences = string.IsNullOrEmpty(listening.Transcript)
                ? new List<string>()
                : listening.Transcript.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(s => s.Trim())
                                       .ToList();

            // Truyền dữ liệu vào ViewModel
            var model = new ListeningUserViewModel
            {
                Id = listening.Id,
                Title = listening.Title,
                AudioUrl = $"https://drive.google.com/file/d/{listening.AudioUrl}/preview",
                Sentences = sentences,
                Questions = listening.Questions?.ToList() ?? new List<ListeningQuestion>()
            };

            return View(model);
        }
    }
}
