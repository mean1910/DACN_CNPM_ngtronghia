using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using elearning_b1.Models;
using elearning_b1.Services;

namespace elearning_b1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ListeningsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GoogleDriveService _googleDriveService;
        private  readonly AssemblyAIService _assemblyAIService;

        public ListeningsController(ApplicationDbContext context, AssemblyAIService assemblyAIService, GoogleDriveService googleDriveService)
        {
            _context = context;
            _googleDriveService = googleDriveService;
            _assemblyAIService = assemblyAIService;
        }

        // GET: Admin/Listenings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Listenings.ToListAsync());
        }

        // GET: Admin/Listenings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listening = await _context.Listenings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listening == null)
            {
                return NotFound();
            }

            return View(listening);
        }

        // GET: Admin/Listenings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Listenings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] Listening listening, IFormFile audioFile, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // 1. Xử lý upload audio file lên Google Drive và lấy fileId
                if (audioFile != null && audioFile.Length > 0)
                {
                    var fileId = await _googleDriveService.UploadFileAsync(audioFile);
                    string fileDownloadUrl = $"https://drive.google.com/uc?id={fileId}&export=download";
                    var transcript = await _assemblyAIService.GetFormattedTranscriptAsync(fileDownloadUrl);

                    listening.AudioUrl = fileId; // Lưu fileId của audio
                    listening.Transcript = transcript; // Lưu transcript
                }

                // 2. Xử lý upload ảnh vào thư mục wwwroot/uploads
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Đường dẫn để lưu ảnh
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    // Kiểm tra nếu thư mục chưa tồn tại thì tạo mới
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    // Lưu file vào thư mục uploads
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName); // Tạo tên file duy nhất
                    var filePath = Path.Combine(uploadFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream); // Copy ảnh vào thư mục
                    }

                    listening.ImgUrl = "/uploads/" + fileName; // Lưu đường dẫn ảnh vào cơ sở dữ liệu
                }

                // 3. Lưu đối tượng Listening vào cơ sở dữ liệu
                _context.Add(listening);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(listening);
        }


        // GET: Admin/Listenings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listening = await _context.Listenings.FindAsync(id);
            if (listening == null)
            {
                return NotFound();
            }
            return View(listening);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Transcript")] Listening listening, IFormFile? newAudioFile, IFormFile? newImageFile)
        {
            if (id != listening.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy đối tượng Listening hiện tại từ cơ sở dữ liệu
                    var existingListening = await _context.Listenings.FindAsync(id);
                    if (existingListening == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật thông tin cơ bản
                    existingListening.Title = listening.Title;

                    // Nếu có tệp audio mới, upload tệp, xóa tệp cũ và lấy transcript mới
                    if (newAudioFile != null && newAudioFile.Length > 0)
                    {
                        // Xóa tệp cũ trên Google Drive (nếu có)
                        if (!string.IsNullOrEmpty(existingListening.AudioUrl))
                        {
                            await _googleDriveService.DeleteFileAsync(existingListening.AudioUrl);
                        }

                        // Upload tệp mới lên Google Drive
                        var newAudioFileId = await _googleDriveService.UploadFileAsync(newAudioFile);
                        existingListening.AudioUrl = newAudioFileId; // Gán fileId mới

                        // Tạo URL tải file mới từ Google Drive
                        string fileDownloadUrl = $"https://drive.google.com/uc?id={newAudioFileId}&export=download";

                        // Lấy transcript mới từ AssemblyAI
                        var newTranscript = await _assemblyAIService.GetFormattedTranscriptAsync(fileDownloadUrl);
                        existingListening.Transcript = newTranscript; // Gán transcript mới
                    }

                    // Nếu có tệp ảnh mới, upload ảnh và lưu đường dẫn ảnh vào cơ sở dữ liệu
                    if (newImageFile != null && newImageFile.Length > 0)
                    {
                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(existingListening.ImgUrl))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingListening.ImgUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Lưu ảnh mới vào thư mục uploads
                        var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                        // Kiểm tra nếu thư mục chưa tồn tại thì tạo mới
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newImageFile.FileName); // Tạo tên file duy nhất
                        var filePath = Path.Combine(uploadFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await newImageFile.CopyToAsync(fileStream); // Copy ảnh vào thư mục
                        }

                        existingListening.ImgUrl = "/uploads/" + fileName; // Lưu đường dẫn ảnh vào cơ sở dữ liệu
                    }

                    // Cập nhật lại cơ sở dữ liệu
                    _context.Update(existingListening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListeningExists(listening.Id))
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

            return View(listening);
        }



        // GET: Admin/Listenings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listening = await _context.Listenings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listening == null)
            {
                return NotFound();
            }

            return View(listening);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listening = await _context.Listenings.FindAsync(id);
            if (listening != null)
            {
                // Xóa file trên Google Drive nếu có
                if (!string.IsNullOrEmpty(listening.AudioUrl))
                {
                    await _googleDriveService.DeleteFileAsync(listening.AudioUrl);
                }

                // Xóa đối tượng Listening khỏi cơ sở dữ liệu
                _context.Listenings.Remove(listening);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool ListeningExists(int id)
        {
            return _context.Listenings.Any(e => e.Id == id);
        }
    }
}
