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
        public string? Gender { get; set; }
        public DateTime? NgaySinh { get; set; }



        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Tame { get; set; }
      
        public string Picture { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

     

        private async Task LoadAsync(NguoiDung user)
        {
            var userO = await _userManager.GetUserAsync(User);
            var userName = await _userManager.GetUserNameAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(userO);
            var nameClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var givenNameClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            var genderClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Gender);
            var phoneClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone);
            var firstName = givenNameClaim?.Value;
            var gender=genderClaim?.Value;
            
            Gender = gender;
            FirstName = firstName;
            if (nameClaim != null)
            {
                var fullName = nameClaim.Value;
                FullName=fullName;
                var parts = fullName.Split(' '); 
                var lastName = parts.Length > 1 ? string.Join(' ', parts.Skip(1)) : "";
            }
            //if (pictureClaim != null)
            //{
            //    var pictureUrl = pictureClaim.Value;
            //    Picture = pictureUrl;
            //}
            if (!string.IsNullOrEmpty(userO.TenNguoiDung))
            {
                var name = userO.TenNguoiDung.ToString();
                Tame = name;

            }
            if (!string.IsNullOrEmpty(userO.NgaySinh.ToString()))
            {
                var ngaySinh = userO.NgaySinh;
                NgaySinh = ngaySinh;

            }
            //if (!string.IsNullOrEmpty(userO.GioiTinh.ToString()))
            //{
            //    var gender = userO.GioiTinh;
            //    Gender = gender;

            //}

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
