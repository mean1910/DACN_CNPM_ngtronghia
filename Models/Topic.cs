using Humanizer.Inflections;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace elearning_b1.Models
{
    public class Topic
    {
        

        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopicID { get; set; }

        public string TopicName { get; set; }
        public string Description { get; set; }

        // Danh sách từ vựng của chủ đề này
        public List<Vocabulary>? Vocabularies { get; set; } 
    }
}
