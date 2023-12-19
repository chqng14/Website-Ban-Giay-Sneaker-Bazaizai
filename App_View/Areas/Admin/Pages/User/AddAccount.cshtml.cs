using App_Data.Models;
using App_View.Areas.Identity.Pages.Account;
using App_View.Services;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using static App_Data.Repositories.TrangThai;
using Microsoft.AspNetCore.Authorization;

namespace App_View.Areas.Admin.Pages.User
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AddAccountModel : PageModel
    {
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IUserStore<NguoiDung> _userStore;
        private readonly IUserEmailStore<NguoiDung> _emailStore;
        private readonly IUserPhoneNumberStore<NguoiDung> _phoneStore;
        private readonly ILogger<RegisterModel> _logger;

        public AddAccountModel(
            UserManager<NguoiDung> userManager,
            IUserStore<NguoiDung> userStore,
            SignInManager<NguoiDung> signInManager,
            ILogger<RegisterModel> logger
            )

        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _phoneStore = GetPhoneStore();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [DataType(DataType.Date)]
            [Display(Name = "Ngày sinh")]
            [AgeDateTime(100, ErrorMessage = "Ngày sinh bạn nhập không hợp lệ.")]
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
            [Required(ErrorMessage = "Tên tài Khoản không được để trống.")]
            [Display(Name = "Tên tài Khoản")]
            public string UserName { get; set; }

            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^(0)+(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-7|8|9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
            [Display(Name = "Số điện thoại")]
            public string? Sdt { get; set; }

            [DataType(DataType.Text)]
            [Required(ErrorMessage = "Tên không được để trống.")]
            [NameLimit(ErrorMessage = "Tên nhập không hợp lệ.")]
            [Display(Name = "Họ và tên")]
            public string FullName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Địa chỉ")]
            public string? Diachi { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Giới tính")]
            public int? GioiTinh { get; set; }

            [Required(ErrorMessage = "Hãy chọn vai trò.")]
            [DataType(DataType.Text)]
            [Display(Name = "Chức vụ")]
           public string ChucVu { get; set; }


        }

 
        public async Task<IActionResult> OnPostAsync(/*string returnUrl = null*/)
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                string MaTS = "ND" + (await _userManager.Users.CountAsync() + 1);
                user.MaNguoiDung = MaTS;
                user.TrangThai = (int?)TrangThaiCoBan.HoatDong;
                user.GioiTinh = Input.GioiTinh;

                user.NgaySinh = Input.NgaySinh;
                user.DiaChi = Input.Diachi;
                user.TenNguoiDung = Input.FullName;
                user.AnhDaiDien = "/user_img/default_image.png";
                user.EmailConfirmed = true;
                await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                if (!string.IsNullOrEmpty(Input.Sdt))
                {
                    await _phoneStore.SetPhoneNumberAsync(user, Input.Sdt, CancellationToken.None);
                }
                if (Input.ChucVu ==null)
                {
                    ModelState.AddModelError(string.Empty, "Hãy chọn chức vụ");
                    return Page();
                }
                var result = await _userManager.CreateAsync(user, Input.Password);
                
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if (!string.IsNullOrEmpty(Input.ChucVu))
                    {
                        await _userManager.AddToRoleAsync(user, Input.ChucVu);
                    }
                 
                    var userId = await _userManager.GetUserIdAsync(user);
                    await AddCart(userId, 0);
                    StatusMessage = $"Bạn vừa tạo thành công người dùng: {user.UserName}";
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
            await httpClient.PostAsync($"https://localhost:7038/api/GioHang?id={idUser}&trangthai={trangThai}", null);
            return true;
        }
    }
}
