using App_Data.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace App_View.Areas.Identity.Pages.Role
{
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

            //public string ID { set; get; }

            [Required(ErrorMessage = "Phải nhập tên role")]
            [Display(Name = "Tên chức vụ")]
            [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string Name { set; get; }
        }

        [BindProperty]
        public InputModel Input { set; get; }

        [BindProperty]
        public bool IsUpdate { set; get; }
       
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = null;
                return Page();
            }

            var newRole = new ChucVu();
            newRole.Name =Input.Name;
            newRole.Id = Guid.NewGuid().ToString();
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
