namespace elearning_b1.Models
{
    public class GrammarLesson
    {
        public int Id { get; set; }
        public string Title { get; set; } // Tiêu đề bài học ngữ pháp
        public string Description { get; set; } // Mô tả bài học
        public string DocFilePath { get; set; } // Đường dẫn tới file DOC
        public string PdfFilePath { get; set; } // Đường dẫn tới file PDF (sau khi chuyển đổi)
    }


}
