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
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public int? Gender { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string Picture { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

     

        private async Task LoadAsync(NguoiDung user)
        {
            var userO = await _userManager.GetUserAsync(User);
            var userName = await _userManager.GetUserNameAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(userO);
            var pictureClaim = userClaims.FirstOrDefault(c => c.Type == "picture");
            //if (pictureClaim != null)
            //{
            //    var pictureUrl = pictureClaim.Value;
            //    Picture = pictureUrl;
            //}
            if (!string.IsNullOrEmpty(userO.TenNguoiDung))
            {
                var name = userO.TenNguoiDung.ToString();
                Name = name;

            }
            if (!string.IsNullOrEmpty(userO.NgaySinh.ToString()))
            {
                var ngaySinh = userO.NgaySinh;
                NgaySinh = ngaySinh;

            }
            if (!string.IsNullOrEmpty(userO.GioiTinh.ToString()))
            {
                var gender = userO.GioiTinh;
                Gender = gender;

            }

            var email = await _userManager.GetEmailAsync(user);
            var phone = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            Email = email;
            Phone = phone;

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
