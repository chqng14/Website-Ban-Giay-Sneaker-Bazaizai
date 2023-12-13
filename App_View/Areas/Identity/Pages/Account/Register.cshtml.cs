// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using App_Data.Models;
using App_View.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IUserPhoneNumberStore<NguoiDung> _phoneStore;
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
            //[DataType(DataType.Text)]
            //[Display(Name = "Ngày sinh")]
            //[AgeLimit(100, ErrorMessage = "Ngày sinh bạn nhập không hợp lệ.")]
            //[RegularExpression(@"^(?:(?:31(\/)(?:0[13578]|1[02]))\1|(?:(?:29|30)(\/)(?:0[1,3-9]|1[0-2])\2))(?:(?:19\d{2}|20[0-9]\d))$|^(?:29(\/)0?2\3(?:(?:(?:1[9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0[1-9]|1\d|2[0-8])(\/)(?:(?:0[1-9])|(?:1[0-2]))\4(?:(?:19\d{2}|20[0-9]\d))$", ErrorMessage = "Ngày sinh không hợp lệ. Vui lòng kiểm tra lại định dạng ( dd/MM/YYYY ) và giá trị.")]
            //public string? NgaySinh { get; set; }
            [DataType(DataType.Date)]
            [AgeDateTime(100, ErrorMessage = "Ngày sinh bạn nhập không hợp lệ.")]
            [Display(Name = "Ngày sinh")]
            public DateTime? NgaySinh { get; set; }


            [Required(ErrorMessage = "Email không được để trống.")]
            [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} phải dài ít nhất là {2} và tối đa {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu")]
            [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
            public string ConfirmPassword { get; set; }

            [DataType(DataType.Text)]
            [StringLength(100, ErrorMessage = "{0} phải dài ít nhất là {2} và tối đa {1} ký tự.", MinimumLength = 10)]
            [Required(ErrorMessage = "Tên tài Khoản không được để trống.")]
            [Display(Name = "Tên tài Khoản")]
            public string UserName { get; set; }

            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^(0)+(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-7|8|9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
            [Display(Name = "Số điện thoại")]
            public string? Sdt { get; set; }

            [DataType(DataType.Text)]
            [Required(ErrorMessage = "Tên của bạn không được để trống.")]
            [NameLimit(ErrorMessage = "Tên bạn nhập không hợp lệ.")]
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

            string format = "dd/MM/yyyy";
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                string MaTS = "ND" + (await _userManager.Users.CountAsync() + 1);
                user.MaNguoiDung = MaTS;
                user.TrangThai = (int?)TrangThaiCoBan.HoatDong;
                user.GioiTinh = Input.GioiTinh;
                //var a = DateTime.Today;
                //if (DateTime.TryParseExact(Input.NgaySinh, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                //{
                //    user.NgaySinh = date;
                //}
                user.NgaySinh = Input.NgaySinh;

                //string fullName = Input.FullName;
                //string regexPattern = @"^[\p{L}]{2,}( [\p{L}]{1,})+$";
                //if (!Regex.IsMatch(fullName, regexPattern))
                //{
                //    ModelState.AddModelError(string.Empty, "Tên của bạn không hợp lệ.");
                //}
                //else user.TenNguoiDung = Input.FullName;
                user.TenNguoiDung = Input.FullName;
                user.AnhDaiDien = "/user_img/default_image.png";

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
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Xác nhận email của bạn",
                        $"Bạn đã đăng ký tài khoản trên Web bán giày thể thao Bazaizai. Vui lòng xác nhận(kích hoạt) tài khoản của bạn bằng cách <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>nhấn vào đây</a>.");
                    await AddCart(userId, 0);
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
