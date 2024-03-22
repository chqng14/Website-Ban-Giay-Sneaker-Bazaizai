// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using App_Data.Models;
using App_View.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App_View.Areas.Identity.Pages.Account.Manage
{
    [Authorize]//user phải đăng nhập
    public class IndexModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IndexModel(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public string Email { get; set; }
        public string Phone { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Username")]
            [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(\w|\.|_){5,32}$", ErrorMessage = "Tên đăng nhập phải từ 6-30 ký tự với ít nhất một chữ cái (A-z). Bao gồm các số (0-9), dấu chấm (.) và dấu gạch dưới (_).")]
            public string Username { get; set; }

            [Display(Name = "Tên")]
            //[StringLength(50)]
            public string Name { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Giới tính")]
            public int? GioiTinh { get; set; }


            [DataType(DataType.Text)]
            [Display(Name = "Ngày sinh")]
            [AgeLimit(100, ErrorMessage = "Ngày sinh bạn nhập không hợp lệ.")]
            [RegularExpression(@"^(?:(?:31(\/)(?:0[13578]|1[02]))\1|(?:(?:29|30)(\/)(?:0[1,3-9]|1[0-2])\2))(?:(?:19\d{2}|20[0-9]\d))$|^(?:29(\/)0?2\3(?:(?:(?:1[9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0[1-9]|1\d|2[0-8])(\/)(?:(?:0[1-9])|(?:1[0-2]))\4(?:(?:19\d{2}|20[0-9]\d))$", ErrorMessage = "Ngày sinh không hợp lệ. Vui lòng kiểm tra lại định dạng ( dd/MM/YYYY ) và giá trị.")]
            public string? NgaySinh { get; set; }

            [Display(Name = "Ảnh đại diện")]
            public string AnhDaiDien { get; set; }
        }

        private async Task LoadAsync(NguoiDung user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            string ngaySinh = null;
            if (user.NgaySinh != null)
            {
                ngaySinh = user.NgaySinh.Value.ToString("dd/MM/yyyy");
            }
            var email = await _userManager.GetEmailAsync(user);
            string maskedEmail = email;  // Gán giá trị mặc định

            int atIndex = email.IndexOf('@');

            if (atIndex >= 0)
            {
                maskedEmail = email.Substring(0, 2) + new string('*', atIndex - 1) + email.Substring(atIndex);
            }

            Email = maskedEmail;
            var phone = await _userManager.GetPhoneNumberAsync(user);
            // Kiểm tra xem số điện thoại có tồn tại và có đủ ký tự không
            if (!string.IsNullOrEmpty(phone))
            {
                string lastTwoDigits = phone.Substring(phone.Length - 2);

                // Mã hóa phần còn lại của số điện thoại
                string maskedPhoneNumber = new string('*', phone.Length - 2);

                // Gán giá trị mới cho Phone
                Phone = maskedPhoneNumber + lastTwoDigits;
            }
            //Email = await _userManager.GetEmailAsync(user);
            //Phone = await _userManager.GetPhoneNumberAsync(user);
            Input = new InputModel
            {
                Username = userName,
                Name = user.TenNguoiDung,
                NgaySinh = ngaySinh,
                GioiTinh = user.GioiTinh,
                AnhDaiDien = user.AnhDaiDien,
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

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var username = await _userManager.GetUserNameAsync(user);
            if (Input.Username != username)
            {
                if (user.SuaDoi > 0)
                {

                    var userNameExists = await _userManager.FindByNameAsync(Input.Username);
                    if (userNameExists != null)
                    {
                        StatusMessage = "Error: Tên tài khoản đã được sử dụng. Chọn một tên tài khoản khác.";
                        return RedirectToPage();
                    }
                    var setUserName = await _userManager.SetUserNameAsync(user, Input.Username);
                    if (!setUserName.Succeeded)
                    {
                        StatusMessage = "Error: Lỗi không mong muốn khi cố gắng sửa tên tài khoản.";
                        return RedirectToPage();
                    }
                    else
                    {
                        user.SuaDoi -= 1;
                        await _userManager.UpdateAsync(user);
                    }

                }
               
            }
            var ngaySinhInput = Input.NgaySinh;
            var ngaySinhUser = user.NgaySinh?.ToString("dd/MM/yyyy");
            if (ngaySinhInput != ngaySinhUser)
            {
                if (DateTime.TryParseExact(Input.NgaySinh, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedNgaySinh))
                {
                    user.NgaySinh = parsedNgaySinh;
                }
                else
                {
                    StatusMessage = "Error: Ngày sinh không hợp lệ.";
                    return RedirectToPage();
                }
            }

            if (Input.Name != user.TenNguoiDung)
            {

                user.TenNguoiDung = Input.Name;
            }
            if (Input.GioiTinh != user.GioiTinh)
            {
                user.GioiTinh = Input.GioiTinh;
            }
            var anhdaidien = user.AnhDaiDien;
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
                    if (anhdaidien != "/user_img/default_image.png")
                    {
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
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
