using App_Data.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace App_View.Areas.Identity.Pages.Account
{
    [Authorize]
    public class my_profileModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;

        public my_profileModel(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }  
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? GioiTinh { get; set; }
        public string NgaySinh { get; set; }  
      
        public string Picture { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

     

        private async Task LoadAsync(NguoiDung user)
        {
            Username= await _userManager.GetUserNameAsync(user);
            FullName =user.TenNguoiDung;
          
            if (user.NgaySinh == null)
            {
                NgaySinh = null;
            }
            else NgaySinh = user.NgaySinh.Value.ToString("dd/MM/yyyy");
            var email = await _userManager.GetEmailAsync(user);
            string maskedEmail = email;  // Gán giá trị mặc định

            int atIndex = email.IndexOf('@');

            if (atIndex >= 0)
            {
                maskedEmail = email.Substring(0, 2) + new string('*', atIndex - 1) + email.Substring(atIndex);
            }

            Email = maskedEmail;
            var phone= await _userManager.GetPhoneNumberAsync(user);
            // Kiểm tra xem số điện thoại có tồn tại và có đủ ký tự không
            if (!string.IsNullOrEmpty(phone))
            {
                string lastTwoDigits = phone.Substring(phone.Length - 2);

                // Mã hóa phần còn lại của số điện thoại
                string maskedPhoneNumber = new string('*', phone.Length - 2);

                // Gán giá trị mới cho Phone
                Phone = maskedPhoneNumber + lastTwoDigits;
            }
            
            Picture = user.AnhDaiDien;
            GioiTinh = (user.GioiTinh == 1) ? "nam" : (user.GioiTinh == 2) ? "nữ" : "";
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

      
    }
}
