using System.ComponentModel.DataAnnotations.Schema;

namespace elearning_b1.Models
{
    public class ListeningOption
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int ListeningQuestionId { get; set; }
        public virtual ListeningQuestion? Question { get; set; }
        [NotMapped]
        public bool IsUserChoice { get; set; }
    }

}
