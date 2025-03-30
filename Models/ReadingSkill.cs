namespace elearning_b1.Models
{
    public class ReadingSkill
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public string Content { get; set; } 
        public  string? ImageUrl { get; set; }
        public virtual ICollection<ReadingQuestion>? Questions { get; set; }
    }
}
