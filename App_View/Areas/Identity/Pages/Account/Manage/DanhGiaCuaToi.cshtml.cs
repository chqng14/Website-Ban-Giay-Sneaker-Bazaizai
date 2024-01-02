using App_Data.Models;
using App_Data.ViewModels.DanhGia;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using App_View.Services;
using App_View.IServices;
namespace App_View.Areas.Identity.Pages.Account.Manage
{
    public class DanhGiaCuaToiModel : PageModel
    {

        private readonly UserManager<NguoiDung> _userManager;
        private readonly IDanhGiaService _danhGiaService;

        public DanhGiaCuaToiModel(UserManager<NguoiDung> userManager, IDanhGiaService danhGiaService)
        {
            _userManager = userManager;
            _danhGiaService = danhGiaService;
        }
        public List<DanhGiaViewModel> DanhGias { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGet()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/PageNotFound");
            }
        
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var userid = await _userManager.GetUserIdAsync(user);
            if(userid == null)
            {
                return RedirectToPage("/Account/PageNotFound");
            }
            DanhGias = await _danhGiaService.GetAllDanhGiaDaDuyetByNd(userid);
            return Page();
        }
    }
}
