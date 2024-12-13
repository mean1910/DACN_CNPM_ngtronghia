using elearning_b1.Models;

namespace elearning_b1.ViewModels
{
    public class WriteSubmissionViewModel
    {
        public int WritingId { get; set; } // Thêm trường này
        public string WritingTitle { get; set; } // Thêm các trường chi tiết của Writing
        public string WritingPrompt { get; set; }
        public string WritingSuggestions { get; set; }
        public string UserAnswer { get; set; }
        public string ExistingAnswer { get; set; }
        public int SubmissionId { get; set; }
        public List<GrammarError> GrammarErrors { get; set; } = new List<GrammarError>();
    }
}
