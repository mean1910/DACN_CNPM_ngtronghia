namespace elearning_b1.Models
{
    public class ListeningQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ListeningId { get; set; }
        public virtual Listening? Listening { get; set; }
        public virtual ICollection<ListeningOption>? Options { get; set; }
    }
}
