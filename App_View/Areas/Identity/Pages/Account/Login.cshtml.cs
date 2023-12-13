// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using App_Data.Models;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;

namespace App_View.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<NguoiDung> _userManager;

        public LoginModel(SignInManager<NguoiDung> signInManager, ILogger<LoginModel> logger, UserManager<NguoiDung> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public class InputModel
        {
            //[Required]
            //[EmailAddress]
            //public string Email { get; set; }

            [Required(ErrorMessage = "Ô này không được để trống.")]
            [Display(Name = "Địa chỉ Email hoặc Tên tài khoản")]
            public string UserNameOrEmail { get; set; }

            [Required(ErrorMessage = "Mật khẩu không được để trống.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.UserNameOrEmail, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (!result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(Input.UserNameOrEmail);
                    if (user != null)
                    {
                        result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);                    
                    }
                }
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    var user = await _userManager.FindByEmailAsync(Input.UserNameOrEmail);
                    if (user == null)
                    {
                        user = await _userManager.FindByNameAsync(Input.UserNameOrEmail);

                    }
                    bool isCustomer = await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "NhanVien");
                    if (isCustomer)
                    {
                        return RedirectToAction("DanhSachHoaDonCho", "BanHangTaiQuay", new { area = "Admin" });
                    }
                    else return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    var user = await _userManager.FindByEmailAsync(Input.UserNameOrEmail);
                    if(user == null)
                    user = await _userManager.FindByNameAsync(Input.UserNameOrEmail);
                    _logger.LogWarning("User account locked out.");
                    //return RedirectToPage("./Lockout");
                    return RedirectToPage("./Lockout", new { id = user.Id });

                }
                else
                {
                    //result.
                    //ErrorMessage = "Error: Nỗ lực đăng nhập không hợp lệ.";
                    //return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                    ErrorMessage = "Error: Nỗ lực đăng nhập không hợp lệ.";
                    //ModelState.AddModelError(string.Empty, "Nỗ lực đăng nhập không hợp lệ.");
                    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
