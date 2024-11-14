using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace elearning_b1.Models
{
    public class Vocabulary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VocabularyID { get; set; }

        public string Word { get; set; }
        public string Meaning { get; set; }
        public string Pronunciation { get; set; }
        public string ExampleSentence { get; set; }
        public string? AudioFile { get; set; }
        public PartOfSpeech? PartOfSpeech { get; set; }

        [ForeignKey("Topic")]
        public int TopicID { get; set; }
        [JsonIgnore]
        public Topic? Topic { get; set; }
    }
    public enum PartOfSpeech
    {
        Noun,
        Verb,
        Adjective,
        Adverb,
        Pronoun,
        Preposition,
        Conjunction,
        Interjection,
        Determiner
    } 
}
