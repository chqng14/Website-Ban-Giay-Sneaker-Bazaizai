﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using App_Data.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using static App_Data.Repositories.TrangThai;
using Microsoft.EntityFrameworkCore;

namespace App_View.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IUserStore<NguoiDung> _userStore;
        private readonly IUserEmailStore<NguoiDung> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<NguoiDung> signInManager,
            UserManager<NguoiDung> userManager,
            IUserStore<NguoiDung> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }
        public string FirstName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGet() => RedirectToPage("./Login");

        //mặc định 
        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            string picture = info.Principal.FindFirstValue("picture");/// thêm ở đây
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }


                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var externalLoginInfo = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                var registerUser = await _userManager.FindByEmailAsync(Input.Email);
                string externalEmail = null;
                NguoiDung externalEmailUser = null;

                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    externalEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
                }
                if (externalEmail != null)
                {
                    externalEmailUser = await _userManager.FindByEmailAsync(externalEmail);
                }
                if (externalEmailUser != null && registerUser != null)
                {
                    //Input.Email==externalEmailregisterUser.Id == externalEmailUser.Id
                    if (Input.Email == externalEmail)
                    {
                        // liên kết tài khoản, đăng nhập
                        if (registerUser.AnhDaiDien == null)
                        {
                            if (info.Principal.HasClaim(c => c.Type == "urn:google:picture"))
                            {
                                registerUser.AnhDaiDien = info.Principal.FindFirst("urn:google:picture")?.Value;
                            }
                        }
                        if (registerUser.DiaChi == null)
                        {
                            if (info.Principal.HasClaim(c => c.Type == "urn:google:locale"))
                            {
                                registerUser.DiaChi = info.Principal.FindFirst("urn:google:locale")?.Value;
                            }
                        }

                        var resultLink = await _userManager.AddLoginAsync(registerUser, info);
                        if (resultLink.Succeeded)
                        {
                            await _signInManager.SignInAsync(registerUser, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    else
                    {
                        //externalEmailUser==registerUser (Input.Email!=externalEmail)
                        ModelState.AddModelError(string.Empty, "Không liên kết được tài khoản, hãy sử dụng email khác");
                        return Page();
                    }
                }
                if (externalEmailUser != null && registerUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Không hỗ trợ tạo tài khoản mới có email khác với email từ dịch vụ ngoài");
                    return Page();
                }
                if (externalEmailUser == null && externalEmail == Input.Email)
                {
                    var user = CreateUser();
                    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                    var email = Input.Email;
                    var atIndex = email.IndexOf('@');
                    if (atIndex != -1)
                    {
                        var userName = email.Substring(0, atIndex);
                        user.UserName = userName;
                    }
                    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name))
                    {
                        user.TenNguoiDung = info.Principal.FindFirst(ClaimTypes.Name)?.Value;
                    }
                    if (info.Principal.HasClaim(c => c.Type == "urn:google:picture"))
                    {
                        user.AnhDaiDien = info.Principal.FindFirst("urn:google:picture")?.Value;
                    }
                    if (info.Principal.HasClaim(c => c.Type == "urn:google:locale"))
                    {
                        user.DiaChi = info.Principal.FindFirst("urn:google:locale")?.Value;
                    }
                    string MaTS = "ND" + (await _userManager.Users.CountAsync() + 1);
                    user.MaNguoiDung = MaTS;
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, ChucVuMacDinh.KhachHang.ToString());

                        result = await _userManager.AddLoginAsync(user, info);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
                            {
                                await _userManager.AddClaimAsync(user,
                                    info.Principal.FindFirst(ClaimTypes.GivenName));
                            }
                            //if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Gender))
                            //{
                            //    await _userManager.AddClaimAsync(user,
                            //        info.Principal.FindFirst(ClaimTypes.Gender));
                            //}  
                            //if (info.Principal.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
                            //{
                            //    await _userManager.AddClaimAsync(user,
                            //        info.Principal.FindFirst(ClaimTypes.DateOfBirth));
                            //} 
                            //if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Name))
                            //{
                            //    await _userManager.AddClaimAsync(user,
                            //        info.Principal.FindFirst(ClaimTypes.Name));
                            //}
                            //if (info.Principal.HasClaim(c => c.Type == "urn:google:locale"))
                            //{
                            //    await _userManager.AddClaimAsync(user,
                            //        info.Principal.FindFirst("urn:google:locale"));
                            //}

                            //if (info.Principal.HasClaim(c => c.Type == "urn:google:picture"))
                            //{
                            //    await _userManager.AddClaimAsync(user,
                            //        info.Principal.FindFirst("urn:google:picture"));
                            //}
                            var props = new AuthenticationProperties();
                            props.StoreTokens(info.AuthenticationTokens);
                            props.IsPersistent = false;

                            var userId = await _userManager.GetUserIdAsync(user);
                            await AddCart(userId, 0);
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            await _userManager.ConfirmEmailAsync(user, code);
                            await _signInManager.SignInAsync(user, props /*isPersistent: false*/, info.LoginProvider);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }

        private NguoiDung CreateUser()
        {
            try
            {
                return Activator.CreateInstance<NguoiDung>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(NguoiDung)}'. " +
                    $"Ensure that '{nameof(NguoiDung)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<NguoiDung> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<NguoiDung>)_userStore;
        }

        public async Task<bool> AddCart(string idUser, int trangThai)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"https://localhost:7038/api/GioHang?id={idUser}&trangthai={trangThai}", null);
            return true;
        }
    }
}
