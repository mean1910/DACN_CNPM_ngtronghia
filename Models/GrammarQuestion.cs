namespace elearning_b1.Models
{
    public class GrammarQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int GrammarId { get; set; }
        public virtual Grammar? Grammar { get; set; }
        public virtual ICollection<GrammarOption>? Options { get; set; }
    }
}
