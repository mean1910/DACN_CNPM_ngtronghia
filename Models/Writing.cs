namespace elearning_b1.Models
{
    public class Writing
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Prompt { get; set; }
        public string Suggestions { get; set; }
        public ICollection<UserWritingSubmission>? Submissions { get; set; }
    }
}
