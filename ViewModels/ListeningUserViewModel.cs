using System.Collections.Generic;
using elearning_b1.Models;
using elearning_b1.ViewModels;

public class ListeningUserViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string AudioUrl { get; set; }
    public List<string> Sentences { get; set; }
    public List<ListeningQuestion> Questions { get; set; }
    public List<ListeningResult> Results { get; set; }
    public double Score { get; set; }
}
