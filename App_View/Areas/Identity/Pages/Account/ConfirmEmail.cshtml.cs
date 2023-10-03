// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
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
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        public ConfirmEmailModel(UserManager<NguoiDung> userManager, SignInManager<NguoiDung> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {

            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Không tìm thấy người dùng có Id: '{userId}'."); //Unable to load user with ID
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Cảm ơn bạn đã xác nhận email của mình." : "Lỗi xác nhận email của bạn.";
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                //return  RedirectToPage("/Index");
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else return Content("Lỗi xác nhận email của bạn.");
            //return Page();
        }
    }
}
