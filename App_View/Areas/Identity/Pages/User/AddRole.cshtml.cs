// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using App_Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace App_View.Areas.Identity.Pages.User
{
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly RoleManager<ChucVu> _roleManager;

        public AddRoleModel(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager,
            RoleManager<ChucVu> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }



        [TempData]
        public string StatusMessage { get; set; }

        public NguoiDung user { get; set; }

        [BindProperty]
        [DisplayName("Các chức vụ gán cho người dùng")]
        public string[] RoleNames { get; set; }
        public SelectList allRoles { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Không có người dùng");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"không tìm thấy người dùng có id: {id}.");
            }

            RoleNames = (await _userManager.GetRolesAsync(user)).ToArray<string>();
            List<string> roleNames = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            allRoles = new SelectList(roleNames);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Không có người dùng");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"không tìm thấy người dùng có id: {id}.");
            }
            var Rolecu = (await _userManager.GetRolesAsync(user)).ToArray();
            var DeleteRoles = Rolecu.Where(x => !RoleNames.Contains(x));
            var AddRoles = RoleNames.Where(x => !Rolecu.Contains(x));
            List<string> roleNames = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            allRoles = new SelectList(roleNames);

            var resultdelete = await _userManager.RemoveFromRolesAsync(user, DeleteRoles);
            if (!resultdelete.Succeeded)
            {
                resultdelete.Errors.ToList().ForEach(

                    error =>
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                );
                return Page();
            }

            var resultAdd = await _userManager.AddToRolesAsync(user, AddRoles);
            if (!resultAdd.Succeeded)
            {
                resultAdd.Errors.ToList().ForEach(

                    error =>
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                );
                return Page();
            }
            
            StatusMessage = $"Bạn vừa cập nhật chức vụ cho người dùng: {user.UserName}";

            return RedirectToPage("./Index");
        }
    }
}
