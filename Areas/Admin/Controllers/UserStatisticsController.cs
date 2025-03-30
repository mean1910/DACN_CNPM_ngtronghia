using elearning_b1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;

namespace elearning_b1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserStatisticsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserStatisticsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var today = DateTime.UtcNow.Date;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            // Lấy tất cả người dùng và sắp xếp theo thời gian tạo (DateCreated)
            var users = _userManager.Users.OrderBy(u => u.DateCreated).ToList();

            // Tổng số người dùng
            var totalUsers = users.Count();

            // Thống kê người dùng mới trong tháng hiện tại
            var usersInCurrentMonth = users
                .Where(u => u.DateCreated >= startOfMonth)
                .Count();

            // Thống kê người dùng mới trong tuần hiện tại
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Tính ngày bắt đầu tuần
            var usersInCurrentWeek = users
                .Where(u => u.DateCreated >= startOfWeek)
                .Count();

            // Nhóm người dùng theo tuần
            var usersByWeek = users
                .GroupBy(u => new { Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(u.DateCreated, CalendarWeekRule.FirstDay, DayOfWeek.Monday), Month = u.DateCreated.Month })
                .Select(g => new { Week = $"Tuần {g.Key.Week} của tháng {g.Key.Month}", Count = g.Count() })
                .OrderBy(g => g.Week)
                .ToList();

            // Nhóm người dùng theo tháng
            var usersByMonth = users
                .GroupBy(u => new { u.DateCreated.Year, u.DateCreated.Month })
                .Select(g => new { Date = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MM/yyyy"), Count = g.Count() })
                .OrderBy(g => g.Date)
                .ToList();

            // Chuyển dữ liệu thành định dạng JSON để sử dụng trong view
            ViewBag.UsersByWeek = JsonConvert.SerializeObject(usersByWeek);
            ViewBag.UsersByMonth = JsonConvert.SerializeObject(usersByMonth);
            ViewBag.TotalUsers = totalUsers;
            ViewBag.UsersInCurrentMonth = usersInCurrentMonth;
            ViewBag.UsersInCurrentWeek = usersInCurrentWeek;

            return View();
        }

    }

}
