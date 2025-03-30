using Microsoft.AspNetCore.Identity;

namespace elearning_b1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
