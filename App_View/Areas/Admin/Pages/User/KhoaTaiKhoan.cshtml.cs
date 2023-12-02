using App_Data.Models;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace App_View.Areas.Admin.Pages.User
{
    public class KhoaTaiKhoanModel : PageModel
    {

        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly RoleManager<ChucVu> _roleManager;
        private readonly IEmailSender _emailSender;
        public KhoaTaiKhoanModel(
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

        //public DateTimeOffset? LockoutEnd { get; set; }
        //public Boolean LockoutEnabled { get; set; }
        public NguoiDung user { get; set; }

        public class InputModel
        {
            [DataType(DataType.DateTime)]
            [Display(Name = "Thời gian mở khóa tài khoản")]
            public DateTimeOffset? LockoutEnd { get; set; }

            //[Required]
            //[Display(Name = "Confirm new password")]
            //public Boolean LockoutEnabled { get; set; }

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


            IdentityResult ResultLockoutEnd = null;
            //IdentityResult ResultLockoutEnabled = null;
            if (Input.LockoutEnd == null /*&& Input.LockoutEnabled == true*/)
            {
                ResultLockoutEnd = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                await _userManager.SetLockoutEnabledAsync(user, true);
                //ResultLockoutEnabled = await _userManager.SetLockoutEnabledAsync(user, true);
            }
            else /*if (Input.LockoutEnd != null && Input.LockoutEnabled == true)*/
            {
                ResultLockoutEnd = await _userManager.SetLockoutEndDateAsync(user, Input.LockoutEnd);
                await _userManager.SetLockoutEnabledAsync(user, true);
                //ResultLockoutEnabled = await _userManager.SetLockoutEnabledAsync(user, true);
                
            }

            if (ResultLockoutEnd == null /*|| ResultLockoutEnabled == null*/ ||
     !ResultLockoutEnd.Succeeded /*|| !ResultLockoutEnabled.Succeeded*/)
            {
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi thực hiện khóa tài khoản.");
                return Page();
            }
            var NguoiKhoa = await _userManager.GetUserAsync(User);
            DateTimeOffset? time;
            if (Input.LockoutEnd == null)
            {
                 time = DateTimeOffset.MaxValue.DateTime;
            }
            else time = ((DateTimeOffset)Input.LockoutEnd).DateTime;
            await _emailSender.SendEmailAsync(user.Email, "Thông báo về việc khóa tài khoản của bạn",
                     $"Bạn đã vi phạm điều khoản trên Web bán giày thể thao Bazaizai. Tài khoản của bạn đã bị khóa bởi quản trị web: {NguoiKhoa.UserName} cho đến:{time} . Mọi thắc mắc xin vui lòng liên hệ đội ngũ hỗ trợ 0369426223.");
            //await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);         
            //await _signInManager.RefreshSignInAsync(user);
            await _userManager.UpdateSecurityStampAsync(user);
            StatusMessage = $"Bạn vừa khóa tài khoản người dùng: {user.UserName}";

            return RedirectToPage("./Index");
        }
    }
}
