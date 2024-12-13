namespace elearning_b1.Models
{
    public class Speaking
    {
        public int Id { get; set; } // Khóa chính
        public int TopicId { get; set; } // Khóa ngoại liên kết với bảng Topic
        public string Question { get; set; }
        public Topic? Topic { get; set; } // Liên kết đến bảng Topic
    }
}
