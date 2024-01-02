using App_Data.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace App_View.Areas.Admin.Pages.Role
{
    //[Authorize(Roles = "Madara")]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class EditModel : PageModel
    {
        private readonly RoleManager<ChucVu> _roleManager;
        public EditModel(RoleManager<ChucVu> roleManager)
        {
            _roleManager = roleManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {

            //public string ID { set; get; }

            [Required(ErrorMessage = "Phải nhập tên role")]
            [Display(Name = "Tên chức vụ")]
            [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string Name { set; get; }
        }

        [BindProperty]
        public InputModel Input { set; get; }
        public ChucVu role { get; set; }

        //[BindProperty]
        //public bool IsUpdate { set; get; }

        public async Task<IActionResult> OnGet(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy chức vụ");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role != null)
            {
                Input = new InputModel
                {
                    Name = role.Name,
                };
                return Page();
            }
            return NotFound("Không tìm thấy chức vụ");
        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy chức vụ");
            role = await _roleManager.FindByIdAsync(roleid);
            if (role == null) return NotFound("Không tìm thấy chức vụ");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            role.Name = Input.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa sửa chức vụ : {Input.Name}";
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
