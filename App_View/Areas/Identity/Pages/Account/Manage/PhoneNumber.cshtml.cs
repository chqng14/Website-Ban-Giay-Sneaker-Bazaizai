using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using App_Data.Models;
using App_View.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace App_View.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class PhoneNumberModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly ISMSSenderService _sMSSenderService;

        public PhoneNumberModel(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager,
            ISMSSenderService sMSSenderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _sMSSenderService = sMSSenderService;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public string CodeOTP { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }
        //public string CodeOTP { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^(0)+(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-7|8|9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "Số điện thoại không hợp lệ.")]

            [Display(Name = "Số điện thoại")]
            public string PhoneNumber { get; set; }

            //[Required]

            [DataType(DataType.Text)]
            [Display(Name = "Mã xác minh")]
            [RegularExpression("^[0-9]{6}$", ErrorMessage = "{0} phải gồm 6 chữ số.")]
            public string? CodeOTPConfirmed { get; set; }
        }



        //public async Task<IActionResult> OnGetAsync()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return NotFound($"Không thể tải người dùng có ID '{_userManager.GetUserId(User)}'.");
        //    }
        //    return Page();
        //}

        public async Task<IActionResult> OnPostChangePhoneAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Không thể tải người dùng có ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var phone = await _userManager.GetPhoneNumberAsync(user);

            if (Input.PhoneNumber != phone)
            {
                string storedOTP = HttpContext.Session.GetString("OTP");
                string creationTimeString = HttpContext.Session.GetString("OTPCreationTime");

                if (!string.IsNullOrEmpty(storedOTP) && !string.IsNullOrEmpty(creationTimeString))
                {
                    DateTimeOffset creationTime = DateTimeOffset.Parse(creationTimeString);
                    DateTimeOffset currentTime = DateTimeOffset.Now;

                    // Kiểm tra xem đã qua 3 phút chưa
                    TimeSpan difference = currentTime - creationTime;
                    if (difference.TotalMinutes <= 3)
                    {
                        // Mã OTP còn hợp lệ
                        CodeOTP = storedOTP;
                    }
                    else
                    {
                        // Mã OTP đã hết hạn
                        CodeOTP = null;

                        // Xoá thông tin từ Session để tránh sử dụng mã OTP đã hết hạn
                        HttpContext.Session.Remove("OTP");
                        HttpContext.Session.Remove("OTPCreationTime");
                    }
                }
                else
                {
                    // Không có mã OTP trong Session
                    CodeOTP = null;
                }




                //string JsonData = HttpContext.Session.GetString("OTP"); // Lấy data từ Session
                //if (JsonData == null)
                //{
                //    CodeOTP = null;
                //}
                //else CodeOTP = JsonData;
                if (CodeOTP == Input.CodeOTPConfirmed)
                {
                    var setPhoneNumberResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                    if (!setPhoneNumberResult.Succeeded)
                    {
                        await _userManager.IsPhoneNumberConfirmedAsync(user);
                        StatusMessage = "Lỗi không mong muốn khi cố gắng thêm số điện thoại mới";
                        return RedirectToPage();
                    }
                    await _signInManager.RefreshSignInAsync(user);
                    StatusMessage = "Số điện thoại của bạn đã được cập nhật";
                    return RedirectToPage("/Account/my-profile");
                }
                StatusMessage = "Error: Mã OTP không đúng.";
                return Page();

            }
            StatusMessage = "Số điện thoại của bạn không thay đổi.";
            return Page();
        }

        public async Task OnPostSendVerificationPhoneAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var phone = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phone)
            {
                // var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, Input.PhoneNumber);
                // HttpContext.Session.SetString("OTP", token);
                // HttpContext.Session.SetString("OTPCreationTime", DateTimeOffset.Now.ToString("O"));

                // string sdt = "+84" + Input.PhoneNumber.Substring(1);

                //await _sMSSenderService.SendSmsAsync(sdt, $"OTP: {token}");

                // StatusMessage = "Mã xác minh số điện thoại đã gửi. Vui lòng kiểm tra tin nhắn của bạn.";
                try
                {
                    var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, Input.PhoneNumber);
                    HttpContext.Session.SetString("OTP", token);
                    HttpContext.Session.SetString("OTPCreationTime", DateTimeOffset.Now.ToString("O"));

                    string sdt = "+84" + Input.PhoneNumber.Substring(1);

                    await _sMSSenderService.SendSmsAsync(sdt, $"OTP: {token}");

                    StatusMessage = "Mã xác minh số điện thoại đã gửi. Vui lòng kiểm tra tin nhắn của bạn.";

                }
                catch (Exception ex)
                {
                    StatusMessage = "Error: Đây là chức năng thử nghiệm. Vui lòng nhập số điện thoại đã xác thực trên Twilio.";
                }
            }
            else
                StatusMessage = "Error: Bạn vừa nhập số điện thoại hiện tại. Vui lòng nhập số điện thoại mới để tiếp tục.";
        }


    }
}
//var userId = await _userManager.GetUserIdAsync(user);
////var token= GenerateOtp();
//var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user,Input.PhoneNumber);
////await _sMSSenderService.SendSmsAsync(user.PhoneNumber, $"OTP: {token}");
