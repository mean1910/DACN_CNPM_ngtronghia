using elearning_b1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

using elearning_b1.Models;
using elearning_b1.ViewModels;
using Newtonsoft.Json;

[Authorize]
public class WritingController : Controller
{
    private readonly LanguageToolService _languageToolService;
    private readonly ApplicationDbContext _context;

    public WritingController(ApplicationDbContext context, LanguageToolService languageToolService)
    {
        _context = context;
        _languageToolService = languageToolService;
    }   

    // Action để hiển thị danh sách các chủ đề viết
    public async Task<IActionResult> Index()
    {
        ViewData["CurrentController"] = "Skills";
        ViewData["CurrentAction"] = "Writing";
        var writings = await _context.Writings.ToListAsync();
        return View(writings);
    }

    // Action để xử lý việc tạo hoặc mở bài viết
    public async Task<IActionResult> WriteSubmission(int writingId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Tìm bài viết của chủ đề hiện tại
        var existingSubmission = await _context.Submissions
            .FirstOrDefaultAsync(u => u.WritingId == writingId &&
                                      u.ApplicationUserId == userId);

        // Lấy thông tin chủ đề
        var writing = await _context.Writings
            .FirstOrDefaultAsync(w => w.Id == writingId);

        if (writing == null)
        {
            return NotFound("Không tìm thấy chủ đề viết.");
        }

        // Tạo ViewModel
        var viewModel = new WriteSubmissionViewModel
        {
            WritingId = writing.Id,
            WritingTitle = writing.Title,
            WritingPrompt = writing.Prompt,
            WritingSuggestions = writing.Suggestions,
            // Nếu đã có bài viết trước đó, điền nội dung bài viết
            UserAnswer = existingSubmission?.UserAnswer ?? "",
            ExistingAnswer = existingSubmission?.UserAnswer ?? ""
        };

        return View("WriteSubmission", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CheckGrammar(WriteSubmissionViewModel model)
    {
        // Kiểm tra UserAnswer có giá trị hay không
        if (string.IsNullOrWhiteSpace(model.UserAnswer))
        {
            TempData["ErrorMessage"] = "Vui lòng nhập bài viết của bạn trước khi kiểm tra ngữ pháp.";
            return View("WriteSubmission", model); // Trả về lại view hiện tại với thông báo lỗi
        }

        try
        {
            // Gọi API để kiểm tra ngữ pháp
            var correctedText = await _languageToolService.CheckGrammarAsync(model.UserAnswer);

            // Kiểm tra kết quả trả về từ API
            if (string.IsNullOrEmpty(correctedText))
            {
                TempData["ErrorMessage"] = "Không thể kiểm tra ngữ pháp, vui lòng thử lại sau.";
                return View("WriteSubmission", model);
            }

            // Chuyển đổi kết quả trả về từ API thành đối tượng
            var result = JsonConvert.DeserializeObject<GrammarToolApiResponse>(correctedText);

            // Kiểm tra result và matches có hợp lệ không
            if (result == null || result.matches == null)
            {
                TempData["ErrorMessage"] = "Kết quả trả về từ hệ thống không hợp lệ.";
                return View("WriteSubmission", model);
            }

            // Lấy danh sách lỗi ngữ pháp từ kết quả trả về
            model.GrammarErrors = result.matches
                ?.Select(match => new GrammarError
                {
                    Message = match.message,
                    ShortMessage = match.shortMessage,
                    SuggestedReplacements = string.Join(", ", match.replacements?.Select(r => r.value) ?? new List<string>()),
                    ErrorContext = match.context?.text ?? string.Empty,
                    ErrorSentence = match.sentence,
                    RuleDescription = match.rule.description,
                    Url = match.rule.urls?.FirstOrDefault()?.value
                })
                .Where(error => error != null) // Lọc các lỗi null
                .ToList() ?? new List<GrammarError>();

            // Kiểm tra và đặt thông báo dựa trên lỗi ngữ pháp
            if (model.GrammarErrors.Count > 0)
            {
                TempData["ErrorMessage"] = "Đã tìm thấy một số lỗi ngữ pháp trong bài viết của bạn.";
            }
            else
            {
                TempData["SuccessMessage"] = "Bài viết của bạn không có lỗi ngữ pháp.";
            }
        }
        catch (Exception ex)
        {
            // Xử lý ngoại lệ và gửi thông báo lỗi
            TempData["ErrorMessage"] = "Đã xảy ra lỗi khi kiểm tra ngữ pháp: " + ex.Message;
            return View("WriteSubmission", model);
        }

        // Trả về lại view với thông báo và danh sách lỗi ngữ pháp (nếu có)
        return View("WriteSubmission", model);
    }




    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveSubmission(WriteSubmissionViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Kiểm tra xem người dùng đã có bài viết cho chủ đề này chưa
        var existingSubmission = await _context.Submissions
            .FirstOrDefaultAsync(u => u.WritingId == model.WritingId &&
                                      u.ApplicationUserId == userId);

        if (existingSubmission != null)
        {
            // Nếu đã tồn tại, cập nhật bài viết
            existingSubmission.UserAnswer = model.UserAnswer;
            existingSubmission.SubmittedAt = DateTime.UtcNow;
        }
        else
        {
            // Nếu chưa tồn tại, tạo bài viết mới
            var newSubmission = new UserWritingSubmission
            {
                WritingId = model.WritingId,
                ApplicationUserId = userId,
                UserAnswer = model.UserAnswer,
                SubmittedAt = DateTime.UtcNow
            };
            _context.Submissions.Add(newSubmission);
        }

        try
        {
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Quay lại giao diện viết bài với thông báo lưu thành công
            TempData["SuccessMessage"] = "Bài viết đã được lưu thành công!";
            return RedirectToAction("WriteSubmission", new { writingId = model.WritingId });
        }
        catch (Exception ex)
        {
            // Nếu có lỗi, quay lại trang viết bài với thông báo lỗi
            ModelState.AddModelError("", "Có lỗi xảy ra khi lưu bài viết. Vui lòng thử lại.");

            return View("WriteSubmission", new WriteSubmissionViewModel
            {
                WritingId = model.WritingId,
                WritingTitle = model.WritingTitle,
                WritingPrompt = model.WritingPrompt,
                WritingSuggestions = model.WritingSuggestions,
                UserAnswer = model.UserAnswer
            });
        }
    }

    public async Task<IActionResult> MySubmissions()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var submissions = await _context.Submissions
            .Where(u => u.ApplicationUserId == userId)
            .Select(u => new MySubmissionsViewModel
            {
                Id = u.Id,
                WritingTitle = u.Writing.Title,
                WritingPrompt = u.Writing.Prompt,
                UserAnswer = u.UserAnswer,
                SubmittedAt = u.SubmittedAt
            })
            .ToListAsync();

        return View(submissions);
    }

    // Thêm ViewModel mới
    public class MySubmissionsViewModel
    {
        public int Id { get; set; }
        public string WritingTitle { get; set; }
        public string WritingPrompt { get; set; }
        public string UserAnswer { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}