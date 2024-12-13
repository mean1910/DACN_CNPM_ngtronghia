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

        public string? ImageUrl { get; set; } // URL của ảnh đã upload lên Google Drive

        public List<Vocabulary>? Vocabularies { get; set; }
        public virtual ICollection<VocabQuestion>? Questions { get; set; }
    }

}
