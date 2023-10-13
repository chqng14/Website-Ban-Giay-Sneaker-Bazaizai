// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using App_Data.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static App_Data.Repositories.TrangThai;

namespace App_View.Areas.Identity.Pages.Account
{

    public class RegisterModel : PageModel
    {

        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IUserStore<NguoiDung> _userStore;
        private readonly IUserEmailStore<NguoiDung> _emailStore;
        private readonly IUserPhoneNumberStore<NguoiDung> _phoneStore;//thêm
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<NguoiDung> userManager,
            IUserStore<NguoiDung> userStore,
            SignInManager<NguoiDung> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender
            )

        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _phoneStore = GetPhoneStore();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {
            [DataType(DataType.DateTime)]
            [Display(Name = "NgaySinh")]
            public DateTime? NgaySinh { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            //[Required]
            //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            //[DataType(DataType.Password)]
            //[Display(Name = "Password")]
            //public string Password { get; set; }

            //[DataType(DataType.Password)]
            //[Display(Name = "Confirm password")]
            //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            //public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }





            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [DataType(DataType.Text)]
            [Required]
            [Display(Name = "Tên tài Khoản")]
            public string UserName { get; set; }

            [DataType(DataType.PhoneNumber)]
            //[RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại phải có từ 10 đến 11 chữ số.")]
            [Display(Name = "Số điện thoại")]
            public string Sdt { get; set; }

            [DataType(DataType.Text)]
            [Required]
            [Display(Name = "Tên của bạn")]
            public string FullName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Giới tính")]
            public int? GioiTinh { get; set; }


        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                string MaTS = "ND" + (await _userManager.Users.CountAsync() + 1);
                user.MaNguoiDung = MaTS;
                user.TrangThai = (int?)TrangThaiCoBan.HoatDong;
                user.GioiTinh = Input.GioiTinh;
                user.NgaySinh = Input.NgaySinh;
                await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                if (!string.IsNullOrEmpty(Input.Sdt))
                {
                    await _phoneStore.SetPhoneNumberAsync(user, Input.Sdt, CancellationToken.None);
                }

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, ChucVuMacDinh.KhachHang.ToString());

                    var userId = await _userManager.GetUserIdAsync(user);
                    //Phát sinh token để xác thực email
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Xác nhận email của bạn",
                        $"Bạn đã đăng ký tài khoản trên Web bán giày thể thao Bazaizai. Vui lòng xác nhận(kích hoạt) tài khoản của bạn bằng cách <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>nhấn vào đây</a>.");
                    await AddCart(userId, 0);/// them vao ở đây
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);//isPersistent: true thì sẽ thiết lập cooki để nhớ, làn sau vẫn có thể truy cập lại mà k cần đăng nhập
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
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
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
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

        private IUserPhoneNumberStore<NguoiDung> GetPhoneStore()
        {
            if (!_userManager.SupportsUserPhoneNumber)
            {
                throw new NotSupportedException("The default UI requires a user store with PhoneNumber support.");
            }
            return (IUserPhoneNumberStore<NguoiDung>)_userStore;
        }
        public async Task<bool> AddCart(string idUser, int trangThai)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"https://localhost:7038/api/GioHang?id={idUser}&trangthai={trangThai}", null);
            return true;
        }


    }
}
