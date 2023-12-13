// Licensed to the .NET Foundation under one or more agreements.
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
using Google.Apis.PeopleService.v1.Data;
using Newtonsoft.Json;
using App_Data.ViewModels.DanhGia;

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
        private readonly HttpClient _httpClient;

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
            _httpClient = new HttpClient();
        }


        public string ProviderDisplayName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }



        public IActionResult OnGet() => RedirectToPage("./Login");

        public async Task<IActionResult> OnPost(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error: Không lấy được thông tin từ dịch vụ ngoài.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                bool isCustomer = await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "NhanVien");
                if (isCustomer)
                {
                    return RedirectToAction("DanhSachHoaDonCho", "BanHangTaiQuay", new { area = "Admin" });
                    //return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
               
                _logger.LogWarning("User account locked out.");
                //return RedirectToPage("./Lockout");
                return RedirectToPage("./Lockout", new { id = user.Id });
            }
            else
            {
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email);
                }
                var externalEmailUser = await _userManager.FindByEmailAsync(Email);
                var externalLoginInfo = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (externalEmailUser != null)
                {
                    if (externalEmailUser.AnhDaiDien == null)
                    {
                        if (info.Principal.HasClaim(c => c.Type == "urn:google:picture"))
                        {
                            externalEmailUser.AnhDaiDien = info.Principal.FindFirst("urn:google:picture")?.Value;
                        }
                    }
                    if (externalEmailUser.DiaChi == null)
                    {
                        if (info.Principal.HasClaim(c => c.Type == "urn:google:locale"))
                        {
                            externalEmailUser.DiaChi = info.Principal.FindFirst("urn:google:locale")?.Value;
                        }
                    }
                    var resultLink = await _userManager.AddLoginAsync(externalEmailUser, info);
                    if (resultLink.Succeeded)
                    {
                        await _signInManager.SignInAsync(externalEmailUser, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        if (externalEmailUser.EmailConfirmed==false)
                        {
                            var CheckGioHang=await GetGioHangById(externalEmailUser.Id);
                            if (CheckGioHang == null)
                            {
                                await AddCart(externalEmailUser.Id, 0);
                            }
                            var props = new AuthenticationProperties();
                            props.StoreTokens(info.AuthenticationTokens);
                            props.IsPersistent = false;
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(externalEmailUser);
                            await _userManager.ConfirmEmailAsync(externalEmailUser, code);
                            await _signInManager.SignInAsync(externalEmailUser, props, info.LoginProvider);
                            return LocalRedirect(returnUrl);
                        }

                        ErrorMessage = "Error: Không liên kết được tài khoản, hãy sử dụng email khác.";
                        //ModelState.AddModelError(string.Empty, "Không liên kết được tài khoản, hãy sử dụng email khác");
                        //return Page();
                        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                    }
                }
                else
                {
                    var user = CreateUser();
                    await _userStore.SetUserNameAsync(user, Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, Email, CancellationToken.None);
                    var email = Email;
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
                    var result1 = await _userManager.CreateAsync(user);
                    if (result1.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, ChucVuMacDinh.KhachHang.ToString());

                        result1 = await _userManager.AddLoginAsync(user, info);
                        if (result1.Succeeded)
                        {
                            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
                            {
                                await _userManager.AddClaimAsync(user,
                                    info.Principal.FindFirst(ClaimTypes.GivenName));
                            }
                            var props = new AuthenticationProperties();
                            props.StoreTokens(info.AuthenticationTokens);
                            props.IsPersistent = false;
                            var userId = await _userManager.GetUserIdAsync(user);
                            await AddCart(userId, 0);
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            await _userManager.ConfirmEmailAsync(user, code);
                            await _signInManager.SignInAsync(user, props, info.LoginProvider);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in result1.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                return Page();
            }
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
            var response= await _httpClient.PostAsync($"https://localhost:7038/api/GioHang?id={idUser}&trangthai={trangThai}", null);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<GioHang> GetGioHangById(string idUser)
        {        
            var response = await _httpClient.GetAsync($"https://localhost:7038/api/GioHang/{idUser}");
            if (response.IsSuccessStatusCode)
            {
                string apiData = await response.Content.ReadAsStringAsync();
                var GioHang = JsonConvert.DeserializeObject<GioHang>(apiData);
                return GioHang;
            }
            else
            {
                return null;
            }
        }
    }
}
