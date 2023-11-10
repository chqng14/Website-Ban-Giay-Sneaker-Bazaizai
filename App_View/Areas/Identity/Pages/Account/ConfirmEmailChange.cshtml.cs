// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Text;
using System.Threading.Tasks;
using App_Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace App_View.Areas.Identity.Pages.Account
{
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;

        public ConfirmEmailChangeModel(UserManager<NguoiDung> userManager, SignInManager<NguoiDung> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = "Lỗi khi thay đổi email.";
                return Page();
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = "Lỗi thay đổi tên người dùng.";
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Cảm ơn bạn đã xác nhận thay đổi email của bạn.";
            return Page();
        }
    }
}
