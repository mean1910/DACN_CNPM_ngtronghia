using System.ComponentModel.DataAnnotations.Schema;

namespace elearning_b1.Models
{
    public class VocabOption
    {
        public int Id { get; set; }
        public string Text { get; set; }  // Nội dung lựa chọn
        public bool IsCorrect { get; set; }  // Kiểm tra xem lựa chọn có đúng không
        [NotMapped] // Đánh dấu thuộc tính này không lưu vào CSDL
        public bool IsUserChoice { get; set; } // Lưu lựa chọn của người dùng tạm thời
        public int QuestionId { get; set; }  // ID của câu hỏi
        public virtual VocabQuestion? Question { get; set; }  // Liên kết với câu hỏi
    }
}
