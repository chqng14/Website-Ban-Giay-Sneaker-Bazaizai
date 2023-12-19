using App_Data.Models;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using static App_Data.Repositories.TrangThai;
using Microsoft.AspNetCore.Authorization;

namespace App_View.Areas.Admin.Pages.User
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DanhSachNhanVienModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        public DanhSachNhanVienModel(UserManager<NguoiDung> userManager)
        {
            _userManager = userManager;
        }
        public class User_Role : NguoiDung
        {
            public string RoleName { get; set; }
        }

        // public List<NguoiDung> users { get; set; }
        public List<User_Role> users { get; set; }
        public List<User_Role> usersManKhac { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public const int ITEMS_PER_PAGE = 8;//số phần tử trong 1 trang

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }
        public int totalUsers { get; set; }




        public async Task<IActionResult> OnGet()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(ChucVuMacDinh.NhanVien.ToString());
            var qr = usersInRole.OrderBy(x => x.UserName).ToList();
            var lstUserAndRole = qr.Select(x => new User_Role()
            {
                Id = x.Id,
                UserName = x.UserName,
                LockoutEnd = x.LockoutEnd,
                LockoutEnabled = x.LockoutEnabled,
                AnhDaiDien = x.AnhDaiDien,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
            });
            usersManKhac = lstUserAndRole.ToList();
            foreach (var user in usersManKhac)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleName = string.Join(",", roles);
            }


            totalUsers = qr.Count();
            countPages = (int)Math.Ceiling((double)totalUsers / ITEMS_PER_PAGE);
            if (currentPage < 1)
                currentPage = 1;
            if (currentPage > countPages)
                currentPage = countPages;
            var qr1 = qr.Skip((currentPage - 1) * ITEMS_PER_PAGE)
            .Take(ITEMS_PER_PAGE)
            .Select(x => new User_Role()
            {
                Id = x.Id,
                UserName = x.UserName,
                AnhDaiDien = x.AnhDaiDien,
                Email = x.Email,
                LockoutEnd = x.LockoutEnd,
                LockoutEnabled = x.LockoutEnabled,
                PhoneNumber = x.PhoneNumber,
            });
            users = qr1.ToList();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleName = string.Join(",", roles);
            }


            return Page();
        }

    }
}
