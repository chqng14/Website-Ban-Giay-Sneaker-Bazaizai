using App_Data.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static App_Data.Repositories.TrangThai;
using Microsoft.AspNetCore.Authorization;

namespace App_View.Areas.Admin.Pages.User
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MoKhoaTaiKhoanModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly RoleManager<ChucVu> _roleManager;
        private readonly IEmailSender _emailSender;
        public MoKhoaTaiKhoanModel(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager,
            RoleManager<ChucVu> roleManager,
              IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public NguoiDung user { get; set; }

        public class InputModel
        {
            [DataType(DataType.DateTime)]
            [Display(Name = "Thời gian mở khóa tài khoản")]
            public DateTimeOffset? LockoutEnd { get; set; }

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
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Không có người dùng");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"không tìm thấy người dùng có id: {id}.");
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
            IdentityResult ResultLockoutEnd = null;
            if (Input.LockoutEnd == null)
            {
                ResultLockoutEnd = await _userManager.SetLockoutEndDateAsync(user, null);
                await _userManager.SetLockoutEnabledAsync(user, true);
            }
            else
            {
                var ThoiGianKhoa = ((DateTimeOffset)user.LockoutEnd).DateTime;
                var ThoiGianMo = ((DateTimeOffset)Input.LockoutEnd).DateTime;
                TimeSpan offset = ((DateTimeOffset)user.LockoutEnd).Offset;
                TimeSpan offsetPlus7 = TimeSpan.FromHours(7);
                if (offset == offsetPlus7)
                {
                   if (ThoiGianMo < DateTime.Now)
                    {
                        ModelState.AddModelError(string.Empty, "Thời gian mở khóa Không phải thời gian trong quá khứ.");
                        return Page();
                    }
                    else if (ThoiGianMo > ThoiGianKhoa)
                    {
                        ModelState.AddModelError(string.Empty, "Thời gian mở khóa phải sớm hơn thời gian khóa.");
                        return Page();
                    }
                }
                else
                {
                    ThoiGianKhoa = ((DateTimeOffset)user.LockoutEnd).ToOffset(offsetPlus7).DateTime;
                    if (ThoiGianMo < DateTime.Now)
                    {
                        ModelState.AddModelError(string.Empty, "Thời gian mở khóa Không phải thời gian trong quá khứ.");
                        return Page();
                    }
                    else if (ThoiGianMo > ThoiGianKhoa)
                    {
                        ModelState.AddModelError(string.Empty, "Thời gian mở khóa phải sớm hơn thời gian khóa.");
                        return Page();
                    }
                }
                ResultLockoutEnd = await _userManager.SetLockoutEndDateAsync(user, Input.LockoutEnd);
                await _userManager.SetLockoutEnabledAsync(user, true);


            }

            if (ResultLockoutEnd == null ||
     !ResultLockoutEnd.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi thực hiện mở khóa tài khoản.");
                return Page();
            }

            await _emailSender.SendEmailAsync(user.Email, "Thông báo về việc mở khóa tài khoản của bạn",
                     $"Tài khoản của bạn đã được mở khóa. Mong bạn sẽ tuân thủ điều khoản trên web chúng tôi. Chân thành xin lỗi bạn vì sự bất tiện này. Mọi thắc mắc xin vui lòng liên hệ đội ngũ hỗ trợ 0369426223.");
            await _userManager.UpdateSecurityStampAsync(user);
            //await _signInManager.RefreshSignInAsync(user);
            StatusMessage = $"Bạn vừa mở khóa tài khoản người dùng: {user.UserName}";
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
