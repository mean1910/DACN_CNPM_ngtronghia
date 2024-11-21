using Microsoft.CodeAnalysis.Options;

namespace elearning_b1.Models
{
    public class VocabQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }  // Nội dung câu hỏi
        public int TopicId { get; set; }  // ID của chủ đề (thay vì ExerciseId)
        public virtual Topic? Topic { get; set; }  // Liên kết với chủ đề

        // Danh sách các lựa chọn câu trả lời
        public virtual ICollection<VocabOption>? Options { get; set; }
    }
}
