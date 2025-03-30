// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using elearning_b1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace elearning_b1.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        public string ImageUrl { get; set; }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Full Name")]
            public string FullName { get; set; }

            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            public DateTime? DateOfBirth { get; set; }

            // Thêm trường để nhận hình ảnh từ người dùng
            [Display(Name = "Profile Image")]
            public IFormFile? ProfileImage { get; set; }
        }



        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var fullName = user.FullName;
            var dateOfBirth = user.DateOfBirth;
            var imageUrl = user.ImageUrl;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FullName = fullName,
                DateOfBirth = dateOfBirth,
               
            };
            ImageUrl = imageUrl;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Xử lý cập nhật số điện thoại
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            // Cập nhật các trường khác như FullName và DateOfBirth
            if (Input.FullName != user.FullName)
            {
                user.FullName = Input.FullName;
            }

            if (Input.DateOfBirth != user.DateOfBirth)
            {
                user.DateOfBirth = Input.DateOfBirth;
            }

            // Xử lý cập nhật hình ảnh nếu có tệp hình ảnh mới
            if (Input.ProfileImage != null)
            {
                // Kiểm tra định dạng file
                var fileExtension = Path.GetExtension(Input.ProfileImage.FileName).ToLower();
                if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                {
                    ModelState.AddModelError("Input.ProfileImage", "Invalid image format. Only JPG, JPEG, and PNG are allowed.");
                    return Page();
                }

                // Lưu tệp hình ảnh vào thư mục uploads
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                var fileName = Guid.NewGuid().ToString() + fileExtension; // Đặt tên file duy nhất
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Tạo thư mục uploads nếu chưa tồn tại
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Lưu tệp vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfileImage.CopyToAsync(stream);
                }

                // Lưu đường dẫn hình ảnh vào cơ sở dữ liệu
                user.ImageUrl = "/uploads/" + fileName;
            }

            // Cập nhật thông tin người dùng trong cơ sở dữ liệu
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update user profile.";
                return RedirectToPage();
            }

            // Refresh sign-in để cập nhật thông tin mới
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }


    }
}
