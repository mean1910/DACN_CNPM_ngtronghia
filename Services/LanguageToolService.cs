using Newtonsoft.Json;
using System.Text;

public class LanguageToolService
{
    private readonly HttpClient _httpClient;

    public LanguageToolService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> CheckGrammarAsync(string text)
    {
        var apiUrl = "https://api.languagetool.org/v2/check";
        var content = new StringContent($"text={text}&language=en", Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await _httpClient.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);
            return formattedJson;
        }

        return "Không có kết quả từ API.";
    }

}
