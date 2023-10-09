using App_Data.Models;
using DocumentFormat.OpenXml.EMMA;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace App_View.Areas.Identity.Pages.Account
{
    public class ManageMyAccountModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;

        public ManageMyAccountModel(
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

        [TempData]
        public string StatusMessage { get; set; }

        private async Task LoadAsync(NguoiDung user)
        {
            var userO = await _userManager.GetUserAsync(User);
          
          
            var userName = await _userManager.GetUserNameAsync(user);
            if (!string.IsNullOrEmpty(userO.TenNguoiDung))
            {
                var name = userO.TenNguoiDung.ToString();
                Name = name;

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
