// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using App_Data.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace App_View.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;

        public ResetPasswordModel(UserManager<NguoiDung> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string username { get; set; }
        public string anh { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "Mật khẩu mới không được để trống.")]
            [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} và dài tối đa {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
            public string ConfirmPassword { get; set; }
            [Required]
            public string Code { get; set; }

        }
        private async Task LoadAsync(NguoiDung user)
        {
            username = await _userManager.GetUserNameAsync(user);
            anh = user.AnhDaiDien;
        }
        public async Task<IActionResult> OnGetAsync(string code = null, string email=null)
        {
            if (code == null||email==null)
            {
                return BadRequest("Phải cung cấp mã để đặt lại mật khẩu.");
            }
            else
            {
                var emailUser = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(email)).ToString();
                var user= await _userManager.FindByEmailAsync(emailUser);
                if (user == null)
                {
                    return RedirectToPage("./PageNotFound");
                }
                await LoadAsync(user);
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
                    Email= emailUser,
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        Console.WriteLine($"Error: {errorMessage}, Exception: {exception?.Message}");
                    }
                }

                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                return RedirectToPage("./PageNotFound");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
