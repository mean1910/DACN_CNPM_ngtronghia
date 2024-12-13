using Google.Apis.Drive.v3.Data;

namespace elearning_b1.Models
{
    public class UserWritingSubmission
    {
        public int Id { get; set; }
        public int WritingId { get; set; }
        public Writing? Writing { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string? UserAnswer { get; set; }
        public DateTime SubmittedAt { get; set; }
    }


}
