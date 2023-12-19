using App_Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App_View.Areas.Admin.Pages.User
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserDetailModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;

        public UserDetailModel(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public string id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? GioiTinh { get; set; }
        public string NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public string Picture { get; set; }

        [TempData]
        public string StatusMessage { get; set; }



        private async Task LoadAsync(NguoiDung user)
        {
            Username = await _userManager.GetUserNameAsync(user);
            FullName = user.TenNguoiDung;
            id=await _userManager.GetUserIdAsync(user);
            if (user.NgaySinh == null)
            {
                NgaySinh = null;
            }
            else NgaySinh = user.NgaySinh.Value.ToString("dd/MM/yyyy");
            Email = await _userManager.GetEmailAsync(user);
            Phone = await _userManager.GetPhoneNumberAsync(user);

            if (user.DiaChi == null)
            {
                DiaChi = null;
            }
            else DiaChi = user.DiaChi;
            Picture = user.AnhDaiDien;
            GioiTinh = (user.GioiTinh == 1) ? "nam" : (user.GioiTinh == 2) ? "nữ" : "";
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không thể tải người dùng có ID '{id}'.");
            }

            await LoadAsync(user);
            return Page();
        }


    }
}
