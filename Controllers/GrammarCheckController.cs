using elearning_b1.Models;
using elearning_b1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class GrammarCheckController : Controller
{
    private readonly LanguageToolService _languageToolService;

    public GrammarCheckController(LanguageToolService languageToolService)
    {
        _languageToolService = languageToolService;
    }

    [HttpPost]
    public async Task<IActionResult> CheckGrammar(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return View("Index", new GrammarCheckViewModel { Message = "Vui lòng nhập văn bản." });
        }

        // Gọi API để kiểm tra ngữ pháp
        var correctedText = await _languageToolService.CheckGrammarAsync(text);

        if (string.IsNullOrEmpty(correctedText))
        {
            return View("Index", new GrammarCheckViewModel
            {
                OriginalText = text,
                Message = "Không thể kiểm tra ngữ pháp, vui lòng thử lại sau."
            });
        }

        // Chuyển đổi JSON thành đối tượng
        var result = JsonConvert.DeserializeObject<GrammarToolApiResponse>(correctedText);

        // Lấy các lỗi ngữ pháp từ kết quả trả về
        var grammarErrors = result.matches.Select(match => new GrammarError
        {
            Message = match.message,
            ShortMessage = match.shortMessage,
            SuggestedReplacements = string.Join(", ", match.replacements.Select(r => r.value)),
            ErrorContext = match.context.text,
            ErrorSentence = match.sentence,
            RuleDescription = match.rule.description,
            Url = match.rule.urls.FirstOrDefault()?.value
        }).ToList();

        // Trả về view với kết quả kiểm tra ngữ pháp
        return View("Index", new GrammarCheckViewModel
        {
            OriginalText = text,
            CorrectedText = result.matches.Count > 0 ? "Văn bản đã được sửa lỗi." : "Không có lỗi ngữ pháp.",
            GrammarErrors = grammarErrors
        });
    }

    public IActionResult Index()
    {
        return View();
    }
}
