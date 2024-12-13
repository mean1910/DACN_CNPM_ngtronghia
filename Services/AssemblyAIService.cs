using AssemblyAI.Transcripts;
using AssemblyAI;

public class AssemblyAIService
{
    private readonly AssemblyAIClient _client;

    public AssemblyAIService(string apiKey)
    {
        _client = new AssemblyAIClient(apiKey);
    }

    public async Task<string> GetTranscriptWithSpeakerLabelsAsync(string audioUrl)
    {
        if (string.IsNullOrEmpty(audioUrl))
        {
            throw new ArgumentException("Audio URL is required.");
        }

        try
        {
            // Tạo transcript với nhận diện người nói
            var transcriptParams = new TranscriptParams
            {
                AudioUrl = audioUrl,
                FormatText = true, // Format văn bản để dễ đọc hơn
                SpeakerLabels = true // Bật tính năng nhận diện người nói
            };

            var transcript = await _client.Transcripts.TranscribeAsync(transcriptParams);
            transcript.EnsureStatusCompleted();

            // Nếu Words chứa thông tin người nói
            if (transcript.Words != null)
            {
                var transcriptWithSpeakers = string.Join("\n", transcript.Words.Select(word =>
                    $"Speaker {word.Speaker}: {word.Text}"
                ));
                return transcriptWithSpeakers;
            }

            return "Transcript không chứa thông tin về người nói.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while fetching transcript with speaker labels: {ex.Message}");
            throw;
        }
    }

    public string FormatTranscript(string rawTranscript)
    {
        var lines = rawTranscript.Split(new[] { "Speaker " }, StringSplitOptions.RemoveEmptyEntries);
        var formattedTranscript = new List<string>();
        string currentSpeaker = null;

        foreach (var line in lines)
        {
            var splitIndex = line.IndexOf(": ");
            if (splitIndex > 0)
            {
                var speaker = "Speaker " + line.Substring(0, splitIndex); // Lấy tên người nói
                var content = line.Substring(splitIndex + 2); // Lấy nội dung

                if (speaker != currentSpeaker)
                {
                    currentSpeaker = speaker;
                    formattedTranscript.Add($"\n<strong>{currentSpeaker}:</strong> {content}");
                }
                else
                {
                    formattedTranscript[formattedTranscript.Count - 1] += " " + content;
                }
            }
        }

        return string.Join("\n", formattedTranscript);
    }

    public async Task<string> GetFormattedTranscriptAsync(string audioUrl)
    {
        // Gọi AssemblyAI để lấy transcript thô
        var rawTranscript = await GetTranscriptWithSpeakerLabelsAsync(audioUrl);

        // Định dạng lại transcript để hiển thị
        return FormatTranscript(rawTranscript);
    }

}
