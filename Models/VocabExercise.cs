namespace elearning_b1.Models
{
    public class VocabExercise
    {
        public int Id { get; set; }
        public string Title { get; set; }  // Tên bài kiểm tra
        public int TopicId { get; set; }  // ID của chủ đề
        public virtual Topic? Topic { get; set; }  // Liên kết với chủ đề

        // Danh sách các câu hỏi trong bài kiểm tra
        public virtual ICollection<VocabQuestion>? Questions { get; set; }
    }
}
