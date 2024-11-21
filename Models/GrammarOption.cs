using System.ComponentModel.DataAnnotations.Schema;

namespace elearning_b1.Models
{
    public class GrammarOption
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int GrammarQuestionId { get; set; }
        public virtual GrammarQuestion? Question { get; set; }
        [NotMapped]
        public bool IsUserChoice { get; set; }
    }
}
