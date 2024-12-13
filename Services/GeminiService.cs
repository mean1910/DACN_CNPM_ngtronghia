public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _model = "models/gemini-1.5-flash:generateContent";  // Đường dẫn mô hình Gemini

    public GeminiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> SendMessageAsync(string userMessage)
    {
        var requestContent = new
        {
            messages = new[] {
                new { role = "user", content = userMessage }
            }
        };

        // Xây dựng URI hợp lệ từ BaseAddress
        var uri = new Uri(_httpClient.BaseAddress, _model);

        var response = await _httpClient.PostAsJsonAsync(uri, requestContent);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            return responseData;
        }

        return "Error: Unable to process request.";
    }
}
