using App_Data.Models;
using App_View.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace App_View.Areas.Admin.Pages.User
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EditUserModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        public EditUserModel(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager,
            IWebHostEnvironment webHostEnvironment,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }


        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email không được để trống.")]
            [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [DataType(DataType.PhoneNumber)]
            [Required(ErrorMessage = "Số điện thoại không được để trống.")]
            [RegularExpression(@"^(0)+(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-7|8|9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
            [Display(Name = "Số điện thoại")]
            public string Phone { get; set; }

            [DataType(DataType.Text)]
            //[StringLength(100, ErrorMessage = "{0} phải dài ít nhất là {2} và tối đa {1} ký tự.", MinimumLength = 10)]
            [Required(ErrorMessage = "Tên tài Khoản không được để trống.")]
            [Display(Name = "Tên tài Khoản")]
            public string Username { get; set; }

            [DataType(DataType.Text)]
            [Required(ErrorMessage = "Tên không được để trống.")]
            //[NameLimit(ErrorMessage = "Tên bạn nhập không hợp lệ.")]
            [Display(Name = "Tên của bạn")]
            public string Name { get; set; }

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
            Input = new InputModel
            {
                Username = userName,
                Name = user.TenNguoiDung,
                Email = await _userManager.GetEmailAsync(user),
                Phone = await _userManager.GetPhoneNumberAsync(user),
                NgaySinh = user.NgaySinh,
                GioiTinh = user.GioiTinh,
                AnhDaiDien = user.AnhDaiDien,
                DiaChi = user.DiaChi,
            };
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không thể tải người dùng có ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không thể tải người dùng có ID '{_userManager.GetUserId(User)}'.");
            }


            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var errorMessage in errorMessages)
                {
                    Console.WriteLine($"Validation Error: {errorMessage}");
                }
                await LoadAsync(user);
                return Page();
            }

            if (Input.Username != user.UserName)
            {
                user.UserName = Input.Username;
            }
            if (Input.Email != user.Email)
            {
                user.Email = Input.Email;
            }
            if (Input.Phone != user.PhoneNumber)
            {
                user.PhoneNumber = Input.Phone;
            }
            if (Input.Name != user.TenNguoiDung)
            {
                user.TenNguoiDung = Input.Name;
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

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                await _emailSender.SendEmailAsync(user.Email, "Thông báo về việc cập nhật thông tin tài khoản của bạn",
      $"Có phải bạn đã yêu cầu cập nhật thông tin tài khoản trên Web bán giày thể thao Bazaizai? Tài khoản của bạn được cập nhật thông tin bởi quản trị web: {currentUser.UserName} . Mọi thắc mắc xin vui lòng liên hệ đội ngũ hỗ trợ 0369426223.");

            }
            //await _signInManager.RefreshSignInAsync(user);
            StatusMessage = $"Hồ sơ của người dùng:{user.UserName} đã được cập nhật";
            return RedirectToPage("./UserDetail", new { id });

        }
    }
}
