using App_Data.Models;
using App_View.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace App_View.Areas.Admin.Pages.User
{
    [Area("Admin")]
    [Authorize]
    public class EditProfileModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EditProfileModel(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {


            [DataType(DataType.Text)]
            [Display(Name = "Giới tính")]
            public int? GioiTinh { get; set; }

            [DataType(DataType.Date)]
            [AgeDateTime(100, ErrorMessage = "Ngày sinh bạn nhập không hợp lệ.")]
            [Display(Name = "Ngày sinh")]
            public DateTime? NgaySinh { get; set; }

            [Display(Name = "Ảnh đại diện")]
            public string? AnhDaiDien { get; set; }

            [Display(Name = "Địa chỉ")]
            public string? DiaChi { get; set; }


        }

        private async Task LoadAsync(NguoiDung user)
        {
            var userName = await _userManager.GetUserNameAsync(user);


            Username = userName;
            Name = user.TenNguoiDung;
            Email = await _userManager.GetEmailAsync(user);
            Phone = await _userManager.GetPhoneNumberAsync(user);
            Input = new InputModel
            {

                NgaySinh =user.NgaySinh,
                GioiTinh = user.GioiTinh,
                AnhDaiDien = user.AnhDaiDien,
                DiaChi = user.DiaChi,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Không thể tải người dùng có ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Không thể tải người dùng có ID '{_userManager.GetUserId(User)}'.");
            }

            //if (!ModelState.IsValid)
            //{
            //    await LoadAsync(user);
            //    return Page();
            //}
            if (!ModelState.IsValid)
            {
                // Truy cập các thông điệp lỗi và in ra
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var errorMessage in errorMessages)
                {
                    Console.WriteLine($"Validation Error: {errorMessage}");
                }

                // Load lại dữ liệu và trả về trang
                await LoadAsync(user);
                return Page();
            }


            if (Input.NgaySinh != user.NgaySinh)
            {
                user.NgaySinh = Input.NgaySinh;
            }

            if (Input.GioiTinh != user.GioiTinh)
            {
                user.GioiTinh = Input.GioiTinh;
            }
            if (Input.DiaChi != user.DiaChi)
            {
                user.DiaChi = Input.DiaChi;
            }
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "user_img");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string imageFileName = user.AnhDaiDien.Replace("/user_img/", "");
                string oldImagePath = Path.Combine(uploadsFolder, imageFileName);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                user.AnhDaiDien = "/user_img/" + uniqueFileName;
                var setImgResult = await _userManager.UpdateAsync(user);
                if (setImgResult.Succeeded)
                {
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                StatusMessage = "Error: Lỗi không mong muốn khi cố gắng cập nhật thông tin.";
            }


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Hồ sơ của bạn đã được cập nhật";
            return RedirectToPage();
        }
    }
}
