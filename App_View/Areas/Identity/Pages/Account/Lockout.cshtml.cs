// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using App_Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App_View.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LockoutModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;

        public LockoutModel(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager)
        {
            _userManager = userManager;
        }
        //public string ThoiGianMoKhoaconlai { get; set; }
        public string? ThoiGianMoKhoa { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        private async Task LoadAsync(NguoiDung user)
        {
            //12/31/9999 11:59:59 PM +00:00

            DateTimeOffset? lockoutEndDateN = await _userManager.GetLockoutEndDateAsync(user);
            if (lockoutEndDateN != null)
            {
                TimeSpan offset =((DateTimeOffset)lockoutEndDateN).Offset;
                TimeSpan offsetPlus7 = TimeSpan.FromHours(7);
                if (offset== offsetPlus7)
                {
                    DateTimeOffset lockoutEndDate = (DateTimeOffset)lockoutEndDateN;
                    ThoiGianMoKhoa = lockoutEndDate.DateTime.ToString("MM/dd/yyyy hh:mm:ss tt");
                }
                else
                {
                    DateTimeOffset lockoutEndDate = ((DateTimeOffset)lockoutEndDateN).ToOffset(offsetPlus7);
                    ThoiGianMoKhoa = lockoutEndDate.DateTime.ToString("MM/dd/yyyy hh:mm:ss tt");
                }
            }
            else RedirectToPage("./PageNotFound");
            //var LockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            //TimeSpan ThoiGianMoKhoaConlai = LockoutEnd.Value - DateTimeOffset.UtcNow;

            //ThoiGianMoKhoaconlai = $"{ThoiGianMoKhoaConlai.Days:D2} ngày: {ThoiGianMoKhoaConlai.Hours:D2} giờ: {ThoiGianMoKhoaConlai.Minutes:D2} phút: {ThoiGianMoKhoaConlai.Seconds:D2} giây";
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToPage("./AccessDenied");
            }

            await LoadAsync(user);
            return Page();
        }
    }

}
