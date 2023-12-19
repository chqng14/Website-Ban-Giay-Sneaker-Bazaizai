using App_Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static App_Data.Repositories.TrangThai;

namespace App_View.Areas.Admin.Pages.Role
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly RoleManager<ChucVu> _roleManager;
        public CreateModel(RoleManager<ChucVu> roleManager)
        {
            _roleManager = roleManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {


            [Required(ErrorMessage = "Phải nhập tên role")]
            [Display(Name = "Tên chức vụ")]
            [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string Name { set; get; }
            //[Required(ErrorMessage = "Phải có trạng thái")]
            //[Display(Name = "Trạng thái")]
            //public int? TrangThai { set; get; }
            
        }

        [BindProperty]
        public InputModel Input { set; get; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = null;
                return Page();
            }

            var newRole = new ChucVu();
            newRole.Name = Input.Name;
            newRole.Id = Guid.NewGuid().ToString();
            //newRole.TrangThai = Input.TrangThai;
            newRole.TrangThai = (int?)TrangThaiCoBan.HoatDong;
            string MaTS = "CV" + (await _roleManager.Roles.CountAsync() + 1);
            newRole.MaChucVu = MaTS;
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                StatusMessage = $"Tạo thành công chức vụ mới : {newRole.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
            }
            return Page();
        }
    }
}
