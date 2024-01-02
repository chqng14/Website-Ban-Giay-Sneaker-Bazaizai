using App_Data.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OpenXmlPowerTools;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Drawing;
using Microsoft.AspNetCore.Authorization;
using static App_Data.Repositories.TrangThai;

namespace App_View.Areas.Admin.Pages.User
{
    
    [Area("Admin")]
    [Authorize(Roles = "Admin , NhanVien")]
    public class IndexModel : PageModel
    {

        private readonly UserManager<NguoiDung> _userManager;

        public IndexModel(UserManager<NguoiDung> userManager)
        {
            _userManager = userManager;
        }
        public class User_Role : NguoiDung
        {
            public string RoleName { get; set; }
        }

        public List<User_Role> users { get; set; }

        [TempData]
        public string StatusMessage { get; set; }





        public async Task<IActionResult> OnGet()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(ChucVuMacDinh.KhachHang.ToString());
            var lst = usersInRole.OrderBy(x => x.UserName).ToList();
            var lstUserAndRole = lst.Select(x => new User_Role()
            {
                Id = x.Id,
                UserName = x.UserName,
                AnhDaiDien=x.AnhDaiDien,
                Email = x.Email,
                LockoutEnd = x.LockoutEnd,
                LockoutEnabled = x.LockoutEnabled,
                PhoneNumber = x.PhoneNumber,

            });
            users = lstUserAndRole.ToList();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleName = string.Join(",", roles);
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (Request.Form["addButton"] != StringValues.Empty)
            {
                for (int i = 2; i < 500; i++)
                {
                    await _userManager.CreateAsync(new NguoiDung { MaNguoiDung = "ND2", UserName = "user" + i, Email = "user" + i + "@gmail.com" }, "1234567");
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
