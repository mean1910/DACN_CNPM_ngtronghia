namespace elearning_b1.Models
{
    public class Listening
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ImgUrl { get; set; }
        public string? AudioUrl { get; set; } 
        public string? Transcript { get; set; } 
        public virtual ICollection<ListeningQuestion>? Questions { get; set; }
        
    }

}
