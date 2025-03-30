using elearning_b1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace elearning_b1.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)] // Chỉ Admin mới có quyền thực hiện các thao tác này
    public class AccountManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountManagementController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Xem danh sách tài khoản
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5; // Số lượng người dùng trên mỗi trang
            int skip = (page - 1) * pageSize; // Tính toán số lượng người dùng cần bỏ qua

            // Lấy danh sách người dùng và phân trang
            var users = _userManager.Users.Skip(skip).Take(pageSize).ToList();

            // Lấy danh sách quyền của từng người dùng
            var userRoles = new Dictionary<string, IList<string>>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles;
            }

            // Truyền thông tin quyền vào ViewBag
            ViewBag.UserRoles = userRoles;

            // Tính toán tổng số trang
            int totalUsers = _userManager.Users.Count(); // Tổng số người dùng
            int totalPages = (int)Math.Ceiling((double)totalUsers / pageSize); // Tổng số trang

            // Truyền thông tin phân trang vào ViewBag
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(users); // Trả về danh sách người dùng cho View
        }



        // Xem chi tiết tài khoản
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user); // Tạo View để hiển thị thông tin chi tiết
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Xóa tất cả quyền cũ trước khi cấp quyền mới
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
            {
                TempData["Error"] = "Error while removing current roles.";
                return RedirectToAction(nameof(Index));
            }

            // Thêm quyền mới
            var addResult = await _userManager.AddToRoleAsync(user, role);
            if (addResult.Succeeded)
            {
                TempData["Success"] = "Role assigned successfully!";
            }
            else
            {
                TempData["Error"] = "Error while assigning role.";
            }

            return RedirectToAction(nameof(Index));
        }


        // Khóa tài khoản
        [HttpPost]
        public async Task<IActionResult> LockAccount(string userId, DateTime lockoutEnd)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
            if (result.Succeeded)
            {
                TempData["Success"] = "Account locked successfully!";
            }
            else
            {
                TempData["Error"] = "Error while locking account.";
            }
            return RedirectToAction(nameof(Index));
        }

        // Mở khóa tài khoản
        [HttpPost]
        public async Task<IActionResult> UnlockAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            if (result.Succeeded)
            {
                TempData["Success"] = "Account unlocked successfully!";
            }
            else
            {
                TempData["Error"] = "Error while unlocking account.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
