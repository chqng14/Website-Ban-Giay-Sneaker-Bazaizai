﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using App_Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static App_Data.Repositories.TrangThai;

namespace App_View.Areas.Admin.Pages.User
{
    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;

        public SetPasswordModel(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public NguoiDung user { get; set; }


        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

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

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var LstRole = await _userManager.GetRolesAsync(user);
            int i = 0;
            foreach (var role in LstRole)
            {
                if (role == ChucVuMacDinh.NhanVien.ToString())
                {
                    i = 1;
                }
                if (role == ChucVuMacDinh.Admin.ToString())
                {
                    i = 2;
                }
                if (role == ChucVuMacDinh.KhachHang.ToString())
                {
                    i = 3;
                }
            }
           
            await  _userManager.RemovePasswordAsync(user);
            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = $"Bạn vừa cập nhật mật khẩu cho người dùng: {user.UserName}";

            if (i == 1)
            {
                return RedirectToPage("./DanhSachNhanVien");
            }
            else if (i == 2)
            {
                return RedirectToPage("./DanhSachQuanly");
            }
            else if (i == 3)
            {
                return RedirectToPage("./Index");
            }
            else return RedirectToPage("./Index");
        }
    }
}
