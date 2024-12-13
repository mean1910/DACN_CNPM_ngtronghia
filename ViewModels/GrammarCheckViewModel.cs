namespace elearning_b1.ViewModels
{
    public class GrammarCheckViewModel
    {
        public string OriginalText { get; set; }
        public string CorrectedText { get; set; }
        public string Message { get; set; }

        public List<GrammarError> GrammarErrors { get; set; }
    }
    public class GrammarError
    {
        public string Message { get; set; }
        public string ShortMessage { get; set; }
        public string SuggestedReplacements { get; set; }
        public string ErrorContext { get; set; }
        public string ErrorSentence { get; set; }
        public string RuleDescription { get; set; }
        public string Url { get; set; }
    }
}
