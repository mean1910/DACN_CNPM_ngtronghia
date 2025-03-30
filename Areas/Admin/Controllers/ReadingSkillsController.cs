using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using elearning_b1.Models;
using Microsoft.AspNetCore.Hosting;

namespace elearning_b1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReadingSkillsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ReadingSkillsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/ReadingSkills
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReadingSkills.ToListAsync());
        }

        // GET: Admin/ReadingSkills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingSkill = await _context.ReadingSkills
                .FirstOrDefaultAsync(m => m.Id == id);
            if (readingSkill == null)
            {
                return NotFound();
            }

            return View(readingSkill);
        }

        // GET: Admin/ReadingSkills/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ReadingSkills/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content")] ReadingSkill readingSkill, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                // Xử lý file ảnh tải lên
                if (Image != null && Image.Length > 0)
                {
                    var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    var fileName = Path.GetFileName(Image.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    // Lưu file ảnh vào thư mục uploads
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    // Cập nhật đường dẫn ảnh
                    readingSkill.ImageUrl = "/uploads/" + fileName;
                }

                _context.Add(readingSkill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(readingSkill);
        }

        // GET: Admin/ReadingSkills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingSkill = await _context.ReadingSkills.FindAsync(id);
            if (readingSkill == null)
            {
                return NotFound();
            }
            return View(readingSkill);
        }

        // POST: Admin/ReadingSkills/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,ImageUrl")] ReadingSkill readingSkill, IFormFile Image)
        {
            if (id != readingSkill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý file ảnh mới nếu có
                    if (Image != null && Image.Length > 0)
                    {
                        var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                        var fileName = Path.GetFileName(Image.FileName);
                        var filePath = Path.Combine(uploadPath, fileName);

                        // Nếu đã có ảnh cũ, xóa file ảnh cũ
                        if (!string.IsNullOrEmpty(readingSkill.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, readingSkill.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Lưu file ảnh mới vào thư mục uploads
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(fileStream);
                        }

                        // Cập nhật đường dẫn ảnh mới
                        readingSkill.ImageUrl = "/uploads/" + fileName;
                    }

                    _context.Update(readingSkill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReadingSkillExists(readingSkill.Id))
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
            return View(readingSkill);
        }

        // GET: Admin/ReadingSkills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingSkill = await _context.ReadingSkills
                .FirstOrDefaultAsync(m => m.Id == id);
            if (readingSkill == null)
            {
                return NotFound();
            }

            return View(readingSkill);
        }

        // POST: Admin/ReadingSkills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var readingSkill = await _context.ReadingSkills.FindAsync(id);
            if (readingSkill != null)
            {
                _context.ReadingSkills.Remove(readingSkill);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReadingSkillExists(int id)
        {
            return _context.ReadingSkills.Any(e => e.Id == id);
        }
    }
}
