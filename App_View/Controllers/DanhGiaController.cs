using App_Data.Models;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Controllers
{
    [Authorize]
    public class DanhGiaController : Controller
    {

        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private IDanhGiaService _danhGiaService;

        public DanhGiaController(IDanhGiaService danhGiaService, UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _danhGiaService = danhGiaService;
        }


        //public ActionResult AddDanhGia()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> AddDanhGia(DanhGia danhgia)
        {
            var listdanhgia = await _danhGiaService.GetDanhGiaById(danhgia.IdDanhGia);
            if (listdanhgia != null)
            {
                return Json(new { mess = "Bạn đã đánh giá sản phẩm này rồi!" });
            }
            var user = await _userManager.GetUserAsync(User);
            danhgia.IdNguoiDung = await _userManager.GetUserIdAsync(user);
            danhgia.ParentId = null;
            await _danhGiaService.CreateDanhGia(danhgia);
            string[] parts = danhgia.IdDanhGia.Split('*');
            var idHoaDon = parts[1];
            return Json(new { iddanhgia = danhgia.IdSanPhamChiTiet });
        }





    }
}
