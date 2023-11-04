using App_Data.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace App_View.Areas.Identity.Pages.Account
{
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

            Email = await _userManager.GetEmailAsync(user);
            Phone = await _userManager.GetPhoneNumberAsync(user);
            Picture = user.AnhDaiDien;
            GioiTinh = (user.GioiTinh == 1) ? "nam" : (user.GioiTinh == 2) ? "nữ" : "";
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

      
    }
}
