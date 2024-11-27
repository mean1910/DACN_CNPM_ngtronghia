using AssemblyAI.Transcripts;
using AssemblyAI;

public class AssemblyAIService
{
    private readonly AssemblyAIClient _client;

    public AssemblyAIService(string apiKey)
    {
        _client = new AssemblyAIClient(apiKey);
    }

    public async Task<string> GetTranscriptAsync(string audioUrl)
    {
        if (string.IsNullOrEmpty(audioUrl))
        {
            throw new ArgumentException("Audio URL is required.");
        }

        try
        {
            // Tạo transcript từ URL file audio
            var transcriptParams = new TranscriptParams
            {
                AudioUrl = audioUrl,
                FormatText = true // Format văn bản để dễ đọc hơn
            };

            var transcript = await _client.Transcripts.TranscribeAsync(transcriptParams);
            transcript.EnsureStatusCompleted();

            // Trả về nội dung transcript
            return transcript.Text;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while fetching transcript: {ex.Message}");
            throw;
        }
    }
}
