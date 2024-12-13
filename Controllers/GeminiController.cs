using Microsoft.AspNetCore.Mvc;

[Route("Gemini")]
public class GeminiController : Controller
{
    private readonly GeminiService _geminiService;
    
    public GeminiController(GeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    [HttpPost("Chat")]
    public async Task<IActionResult> Chat(string userMessage)
    {
        var responseMessage = await _geminiService.SendMessageAsync(userMessage);
        return Json(new { message = responseMessage });
    }
}
