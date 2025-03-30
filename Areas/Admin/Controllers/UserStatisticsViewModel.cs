using Google.Apis.Drive.v3.Data;

namespace elearning_b1.Areas.Admin.Controllers
{
    internal class UserStatisticsViewModel
    {
        public int TotalUsers { get; set; }
        public int NewUsersToday { get; set; }
        public int NewUsersThisMonth { get; set; }
    }
}
