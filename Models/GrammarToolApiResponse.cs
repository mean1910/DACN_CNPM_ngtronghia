namespace elearning_b1.Models
{
    public class GrammarToolApiResponse
    {
        public SoftwareInfo software { get; set; }
        public Warnings warnings { get; set; }
        public Language language { get; set; }
        public List<Match> matches { get; set; }
    }

    public class SoftwareInfo
    {
        public string name { get; set; }
        public string version { get; set; }
        public string buildDate { get; set; }
        public int apiVersion { get; set; }
        public bool premium { get; set; }
        public string premiumHint { get; set; }
        public string status { get; set; }
    }

    public class Warnings
    {
        public bool incompleteResults { get; set; }
    }

    public class Language
    {
        public string name { get; set; }
        public string code { get; set; }
        public DetectedLanguage detectedLanguage { get; set; }
    }

    public class DetectedLanguage
    {
        public string name { get; set; }
        public string code { get; set; }
        public double confidence { get; set; }
        public string source { get; set; }
    }

    public class Match
    {
        public string message { get; set; }
        public string shortMessage { get; set; }
        public List<Replacement> replacements { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public Context context { get; set; }
        public string sentence { get; set; }
        public Rule rule { get; set; }
    }

    public class Replacement
    {
        public string value { get; set; }
    }

    public class Context
    {
        public string text { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
    }

    public class Rule
    {
        public string id { get; set; }
        public string description { get; set; }
        public List<RuleUrl> urls { get; set; }
    }

    public class RuleUrl
    {
        public string value { get; set; }
    }

}
