using System.ComponentModel.DataAnnotations.Schema;

namespace elearning_b1.Models
{
    public class ReadingOption
    {
        public int Id { get; set; }
        public string Text { get; set; }  
        public bool IsCorrect { get; set; }
        public int ReadingQuestionId { get; set; }
        public virtual ReadingQuestion? Question { get; set; }
        [NotMapped]
        public bool IsUserChoice { get; set; }
    }
}
