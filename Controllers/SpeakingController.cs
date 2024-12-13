using elearning_b1.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace elearning_b1.Controllers
{
    public class SpeakingController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _assemblyAiApiKey;

        public SpeakingController(IConfiguration configuration)
        {
            _assemblyAiApiKey = configuration["AssemblyAI:ApiKey"];
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AnalyzeAudio(IFormFile audio, string userInputText)
        {
            if (audio == null || audio.Length == 0)
            {
                return Json(new { success = false, message = "No audio file uploaded." });
            }

            if (string.IsNullOrWhiteSpace(userInputText))
            {
                return Json(new { success = false, message = "Please enter the text you want to practice." });
            }

            try
            {
                var apiKey = _assemblyAiApiKey;
                var uploadUrl = "https://api.assemblyai.com/v2/upload";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("authorization", apiKey);
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(audio.OpenReadStream()), "file", audio.FileName);

                    var response = await client.PostAsync(uploadUrl, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var uploadResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        var audioUrl = uploadResponse?.upload_url;

                        if (string.IsNullOrEmpty((string)audioUrl))
                        {
                            return Json(new { success = false, message = "Failed to get audio URL." });
                        }

                        var transcriptUrl = "https://api.assemblyai.com/v2/transcript";
                        var transcriptRequest = new
                        {
                            audio_url = audioUrl
                        };

                        var transcriptContent = new StringContent(JsonConvert.SerializeObject(transcriptRequest), Encoding.UTF8, "application/json");
                        var transcriptResponse = await client.PostAsync(transcriptUrl, transcriptContent);
                        var transcriptResponseContent = await transcriptResponse.Content.ReadAsStringAsync();

                        if (!transcriptResponse.IsSuccessStatusCode)
                        {
                            return Json(new { success = false, message = "Error while processing the audio file." });
                        }

                        var transcriptResponseData = JsonConvert.DeserializeObject<dynamic>(transcriptResponseContent);
                        var transcriptId = transcriptResponseData?.id;

                        if (string.IsNullOrEmpty((string)transcriptId))
                        {
                            return Json(new { success = false, message = "Failed to get transcript ID." });
                        }

                        // Lặp lại việc kiểm tra trạng thái transcript
                        string transcriptStatusUrl = $"https://api.assemblyai.com/v2/transcript/{transcriptId}";
                        string status = "processing";
                        string transcriptText = null;
                        int retryCount = 0;
                        const int maxRetries = 6; // Tăng số lần thử lại lên 10
                        const int delayTime = 10000; // Thêm thời gian delay lên 60 giây giữa các lần kiểm tra

                        while ((status == "processing" || status == "queued") && retryCount < maxRetries)
                        {
                            await Task.Delay(delayTime); // Delay 60 giây giữa các lần kiểm tra trạng thái
                            retryCount++;

                            var statusResponse = await client.GetAsync(transcriptStatusUrl);
                            var statusResponseContent = await statusResponse.Content.ReadAsStringAsync();
                            var statusData = JsonConvert.DeserializeObject<dynamic>(statusResponseContent);

                            if (string.IsNullOrEmpty((string)statusData?.status))
                            {
                                return Json(new { success = false, message = "Transcript status is invalid." });
                            }

                            status = statusData?.status;

                            if (status == "completed")
                            {
                                transcriptText = statusData?.text;
                                break;
                            }
                        }

                        // Nếu không hoàn thành sau các lần thử lại, trả về thông báo lỗi
                        if (string.IsNullOrEmpty(transcriptText))
                        {
                            return Json(new { success = false, message = "Transcript is not ready yet after retries." });
                        }

                        var comparisonResult = CompareTexts(userInputText, transcriptText);

                        return Json(new { success = true, transcript = transcriptText, comparisonResult });
                    }
                    else
                    {
                        return Json(new { success = false, message = $"Error uploading audio: {responseContent}" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error occurred: {ex.Message}" });
            }
        }



        private string CompareTexts(string userInput, string transcript)
        {
            userInput = userInput.Trim().ToLower();
            transcript = transcript.Trim().ToLower();

            if (userInput == transcript)
            {
                return "Great! Your pronunciation matches the text perfectly!";
            }
            else
            {
                return "There are some differences between your pronunciation and the text. Keep practicing!";
            }
        }
    }
}
