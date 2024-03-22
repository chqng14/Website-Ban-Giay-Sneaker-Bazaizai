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
        private IDanhGiaservice _DanhGiaservice;

        public DanhGiaController(IDanhGiaservice DanhGiaservice, UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _DanhGiaservice = DanhGiaservice;
        }


        //public ActionResult AddDanhGia()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> AddDanhGia(DanhGia danhgia)
        {
            var listdanhgia = await _DanhGiaservice.GetDanhGiaById(danhgia.IdDanhGia);
            if (listdanhgia != null)
            {
                return Json(new { mess = "Bạn đã đánh giá sản phẩm này rồi!" });
            }
            var user = await _userManager.GetUserAsync(User);
            danhgia.IdNguoiDung = await _userManager.GetUserIdAsync(user);
            danhgia.ParentId = null;
            await _DanhGiaservice.CreateDanhGia(danhgia);
            string[] parts = danhgia.IdDanhGia.Split('*');
            var idHoaDon = parts[1];
            return Json(new { iddanhgia = danhgia.IdDanhGia });
        }





    }
}
