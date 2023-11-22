using App_Data.Models;
using App_View.IServices;
using App_View.Services;
using DocumentFormat.OpenXml.EMMA;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace App_View.Areas.Identity.Pages.Account
{
    [Authorize]
    public class ManageMyAccountModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private IHoaDonServices hoaDonServices;
        private IThongTinGHServices thongTinGHServices;
        public ManageMyAccountModel(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            hoaDonServices = new HoaDonServices();
            thongTinGHServices = new ThongTinGHServices();
        }
        public string FirstName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        private async Task LoadAsync(NguoiDung user)
        {
            var userO = await _userManager.GetUserAsync(User);
            var userClaims = await _userManager.GetClaimsAsync(userO);
            var givenNameClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            var firstName = givenNameClaim?.Value;
            FirstName = firstName;
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
            var UserID = _userManager.GetUserId(User);
            var listHoaDon = await hoaDonServices.GetHoaDonOnline(UserID);
            var thongTinGH = (await thongTinGHServices.GetThongTinByIdUser(UserID)).FirstOrDefault(c => c.TrangThai == 0);
            if (listHoaDon != null)
            {
                listHoaDon = listHoaDon.OrderByDescending(dh => dh.NgayTao).Take(4).ToList();
                ViewData["listHoaDon"] = listHoaDon;
            }
            if (thongTinGH != null)
            {
                ViewData["ThongTinGH"] = thongTinGH;
            }
            return Page();
        }
    }
}
