using App_Data.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace App_View.Areas.Identity.Pages.Role
{
    public class IndexModel : PageModel
    {

        private readonly RoleManager<ChucVu> _roleManager;
      
        public IndexModel(RoleManager<ChucVu> roleManager)
        {
            _roleManager = roleManager;
        }
        public List<ChucVu> roles { get; set; }

        [TempData] // Sử dụng Session lưu thông báo
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGet()
        {
            roles = await _roleManager.Roles.ToListAsync();
            return Page();
        }
    }
}
