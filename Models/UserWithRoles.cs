namespace elearning_b1.Models
{
    public class UserWithRoles
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }
        public bool IsLocked { get; set; }
    }

}