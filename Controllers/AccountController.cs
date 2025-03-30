using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using elearning_b1.Models;

namespace elearning_b1.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Sau khi người dùng đăng nhập
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User); // Lấy người dùng hiện tại
            var roles = await _userManager.GetRolesAsync(user); // Lấy các vai trò của người dùng

            // Điều hướng theo vai trò
            if (roles.Contains(SD.Role_Admin))
            {
                return RedirectToAction("Index", "Admin"); // Điều hướng đến Admin Dashboard
            }
            else if (roles.Contains(SD.Role_User))
            {
                return RedirectToAction("Index", "Home"); // Điều hướng đến trang chủ User
            }
            
            return RedirectToAction("Index", "Home"); // Nếu không có vai trò rõ ràng, điều hướng về trang chủ
        }

        // Logout và điều hướng về trang chủ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
