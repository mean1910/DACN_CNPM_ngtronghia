using elearning_b1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class Speaking02Controller : Controller
{
    private readonly ApplicationDbContext _context;

    public Speaking02Controller(ApplicationDbContext context)
    {
        _context = context;
    }

    // Hiển thị danh sách các chủ đề
    public async Task<IActionResult> Index(int page = 1)
    {
        ViewData["CurrentController"] = "Skills";
        ViewData["CurrentAction"] = "Speaking";
        // Đặt số lượng item trên mỗi trang
        int pageSize = 3;

        // Lấy tất cả các chủ đề và đếm số lượng từ vựng cho mỗi chủ đề
        var topics = await _context.Topics
            .Include(t => t.Vocabularies) // Đảm bảo lấy luôn danh sách từ vựng của mỗi chủ đề
            .ToListAsync();

        // Truyền vào view với số lượng từ vựng cho mỗi chủ đề
        var topicWithVocabularyCount = topics.Select(t => new
        {
            Topic = t,
            VocabularyCount = t.Vocabularies.Count // Đếm số lượng từ vựng của chủ đề
        }).ToList();

        // Lấy số lượng trang tổng cộng
        var totalPages = (int)Math.Ceiling((double)topicWithVocabularyCount.Count / pageSize);

        // Lấy các phần tử của trang hiện tại
        var pagedItems = topicWithVocabularyCount
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Truyền vào View
        ViewBag.Topics = pagedItems;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View();
    }

    // Hiển thị chi tiết chủ đề
    public async Task<IActionResult> TopicDetails(int topicId)
    {
        // Lấy thông tin chủ đề, câu hỏi, và từ vựng liên quan
        var topic = await _context.Topics
            .Include(t => t.Vocabularies) // Bao gồm các từ vựng liên quan
            .Include(t => t.Questions) // Bao gồm câu hỏi ngữ pháp trong chủ đề
            .FirstOrDefaultAsync(t => t.TopicID == topicId);

        if (topic == null)
        {
            return NotFound();
        }

        // Lấy các câu hỏi từ bảng Speaking liên quan đến chủ đề
        var speakingQuestions = await _context.Speakings
            .Where(s => s.TopicId == topicId)
            .ToListAsync();

        // Truyền dữ liệu vào View
        ViewBag.SpeakingQuestions = speakingQuestions;
        return View(topic); // Truyền chủ đề, câu hỏi và từ vựng vào View
    }
}
