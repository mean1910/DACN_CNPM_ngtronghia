namespace elearning_b1.Models
{
    public class ReadingQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ReadingSkillId { get; set; }
        public virtual ReadingSkill? ReadingSkill { get; set; } 
        public virtual ICollection<ReadingOption>? Options { get; set; } 
    }
}
