using Microsoft.CodeAnalysis.Options;

namespace elearning_b1.Models
{
    public class VocabQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }  // Nội dung câu hỏi
        public int ExerciseId { get; set; }  // ID của bài kiểm tra
        public virtual VocabExercise? Exercise { get; set; }  // Liên kết với bài kiểm tra

        // Danh sách các lựa chọn câu trả lời
        public virtual ICollection<VocabOption>? Options { get; set; }
    }
}
