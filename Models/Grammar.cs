namespace elearning_b1.Models
{
    public class Grammar
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public virtual ICollection<GrammarQuestion>? Questions { get; set; }
    }
}
