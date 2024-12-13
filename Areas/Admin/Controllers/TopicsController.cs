using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using elearning_b1.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace elearning_b1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TopicsController> _logger;
        private readonly string _imageUploadPath;

        public TopicsController(ApplicationDbContext context, ILogger<TopicsController> logger)
        {
            _context = context;
            _logger = logger;
            _imageUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        }

        // GET: Admin/Topics
        public async Task<IActionResult> Index()
        {
            try
            {
                var topics = await _context.Topics.ToListAsync();
                return View(topics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching topics");
                return View("Error", new { message = "Error fetching topics" });
            }
        }

        // GET: Admin/Topics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Topics/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TopicID,TopicName,Description")] Topic topic, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    try
                    {
                        // Lưu ảnh cục bộ
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(_imageUploadPath, fileName);

                        // Tạo thư mục nếu chưa tồn tại
                        if (!Directory.Exists(_imageUploadPath))
                        {
                            Directory.CreateDirectory(_imageUploadPath);
                        }

                        // Lưu file vào thư mục cục bộ
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        topic.ImageUrl = "/uploads/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error saving image locally for topic creation");
                        ModelState.AddModelError("", "Error saving image: " + ex.Message);
                        return View(topic);
                    }
                }

                try
                {
                    _context.Topics.Add(topic);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving topic to database");
                    ModelState.AddModelError("", "Error saving topic: " + ex.Message);
                    return View(topic);
                }
            }

            return View(topic);
        }

        // GET: Admin/Topics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Admin/Topics/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopicID,TopicName,Description,ImageUrl")] Topic topic, IFormFile? imageFile)
        {
            if (id != topic.TopicID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Nếu có ảnh mới, xóa ảnh cũ và lưu ảnh mới
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(topic.ImageUrl))
                        {
                            var oldFilePath = Path.Combine(_imageUploadPath, Path.GetFileName(topic.ImageUrl));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Lưu ảnh mới
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(_imageUploadPath, fileName);

                        // Tạo thư mục nếu chưa tồn tại
                        if (!Directory.Exists(_imageUploadPath))
                        {
                            Directory.CreateDirectory(_imageUploadPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        topic.ImageUrl = "/uploads/" + fileName;
                    }

                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating topic");
                    ModelState.AddModelError("", "Error updating topic: " + ex.Message);
                    return View(topic);
                }
            }

            return View(topic);
        }

        // GET: Admin/Topics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics.FirstOrDefaultAsync(m => m.TopicID == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Admin/Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic != null)
            {
                try
                {
                    // Xóa ảnh cũ
                    if (!string.IsNullOrEmpty(topic.ImageUrl))
                    {
                        var filePath = Path.Combine(_imageUploadPath, Path.GetFileName(topic.ImageUrl));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    _context.Topics.Remove(topic);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting topic");
                    ModelState.AddModelError("", "Error deleting topic: " + ex.Message);
                    return View(topic);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TopicExists(int id)
        {
            return _context.Topics.Any(e => e.TopicID == id);
        }
    }
}
