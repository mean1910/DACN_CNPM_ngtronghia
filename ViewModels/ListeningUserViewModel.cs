using System.Collections.Generic;
using elearning_b1.Models;

public class ListeningUserViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string AudioUrl { get; set; }
    public List<string> Sentences { get; set; } // Các câu transcript tách ra
    public List<ListeningQuestion> Questions { get; set; } // Danh sách câu hỏi
}
