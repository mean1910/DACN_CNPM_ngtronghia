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
        public async Task<IActionResult> Create([Bind("Id,Title")] Listening listening, IFormFile audioFile)
        {
            if (ModelState.IsValid)
            {
                if (audioFile != null && audioFile.Length > 0)
                {
                    // 1. Upload file lên Google Drive và lấy fileId
                    var fileId = await _googleDriveService.UploadFileAsync(audioFile);

                    // 2. Lấy transcript từ Assembly AI
                    string fileDownloadUrl = $"https://drive.google.com/uc?id={fileId}&export=download";
                    var transcript = await _assemblyAIService.GetTranscriptAsync(fileDownloadUrl);

                    // 3. Gán fileId và transcript cho đối tượng Listening
                    listening.AudioUrl = fileId; // Chỉ lưu fileId
                    listening.Transcript = transcript; // Lưu transcript
                }

                // 4. Lưu đối tượng Listening vào cơ sở dữ liệu
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Transcript")] Listening listening, IFormFile? newAudioFile)
        {
            if (id != listening.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingListening = await _context.Listenings.FindAsync(id);
                    if (existingListening == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật thông tin cơ bản
                    existingListening.Title = listening.Title;
                    existingListening.Transcript = listening.Transcript;

                    // Nếu có file âm thanh mới, thực hiện upload và cập nhật
                    if (newAudioFile != null && newAudioFile.Length > 0)
                    {
                        // Xóa file cũ khỏi Google Drive
                        if (!string.IsNullOrEmpty(existingListening.AudioUrl))
                        {
                            await _googleDriveService.DeleteFileAsync(existingListening.AudioUrl);
                        }

                        // Upload file mới và lấy fileId
                        var newAudioFileId = await _googleDriveService.UploadFileAsync(newAudioFile);

                        // Gán lại thông tin fileId mới
                        existingListening.AudioUrl = newAudioFileId;
                    }

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
