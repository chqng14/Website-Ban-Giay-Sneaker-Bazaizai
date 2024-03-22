using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.DanhGia;
using App_View.IServices;
using App_View.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, NhanVien")]
    public class DanhGiaController : Controller
    {
        private readonly UserManager<NguoiDung> _userManager;
        private IDanhGiaservice _DanhGiaservice;
        private readonly IEmailSender _emailSender;
        public DanhGiaController(IDanhGiaservice DanhGiaservice, UserManager<NguoiDung> userManager, IEmailSender emailSender)
        {
            _DanhGiaservice = DanhGiaservice;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        [HttpPost]
        public async Task<IActionResult> LstDangGiaChuaDuyetTab(int? tab)
        {


            var lstDanhGiaChuaDuyet = (await _DanhGiaservice.GetLstDanhGiaChuaDuyetByDK(tab)).ToList();
            return PartialView("_DanhSachDanhGiaChuaDuyetPartial", lstDanhGiaChuaDuyet);
        }
        public async Task<IActionResult> Index()
        {
            var lst = await _DanhGiaservice.GetAllDanhGia();
            return View(lst);
        }


        public async Task<IActionResult> DanhSachDanhGiaChuaDuyet()
        {
            return View();
        }
        public async Task<IActionResult> DanhSachDanhGiaDaDuyet()
        {
            return View();
        }






        //public async Task<IActionResult> TongSoDanhGiaCuaMoiSpChuaDuyet()
        //{
        //    var result = await _DanhGiaservice.TongSoDanhGiaCuaMoiSpChuaDuyet();

        //    // Chuyển đổi từ Tuple sang DanhGiaResult
        //    var danhGiaResults = result.Select(tuple => new DanhGiaResult
        //    {
        //        SanPham = tuple.Item1,
        //        SoLuongDanhGiaChuaDuyet = tuple.Item2,
        //        IdSanPham = tuple.Item3,
        //    }).ToList();
        //    return View(result);

        //} 
        public async Task<IActionResult> TongSoDanhGiaCuaMoiSpChuaDuyet()
        {

            var result = await _DanhGiaservice.TongSoDanhGiaCuaMoiSpChuaDuyet();
            var danhGiaResults = result.Select(item => new DanhGiaResult
            {
                SanPham = item.SanPham,
                SoLuongDanhGiaChuaDuyet = item.SoLuongDanhGiaChuaDuyet,
                IdSanPham = item.IdSanPham,
                IdChiTietSp = item.IdChiTietSp,
            }).ToList();

            return View(result);
        }

        public async Task<IActionResult> LstChiTietDanhGiaCuaMoiSpChuaDuyet(string idSanPham)
        {
            var lst = await _DanhGiaservice.LstChiTietDanhGiaCuaMoiSpChuaDuyet(idSanPham);
            return View(lst);
        }
        public async Task<IActionResult> DuyetDanhGia(string id)
        {

            var result = await _DanhGiaservice.DuyetDanhGia(id);

            if (result)
            {
                return RedirectToAction("DanhSachDanhGiaChuaDuyet");
            }
            else
            {
                return BadRequest();
            }
        }



        public async Task<IActionResult> DuyetTatCaDanhGiaCuaMSp(string idSanPham)
        {
            var lst = await _DanhGiaservice.LstChiTietDanhGiaCuaMoiSpChuaDuyet(idSanPham);
            foreach (var item in lst)
            {
                await _DanhGiaservice.DuyetDanhGia(item.IdDanhGia);
            }
            return RedirectToAction("TongSoDanhGiaCuaMoiSpChuaDuyet");
        }

        public async Task<IActionResult> XoaDanhGia(string id, string liDo)
        {
            var dg = await _DanhGiaservice.GetDanhGiaById(id);

            var user = await _userManager.FindByIdAsync(dg.IdNguoiDung);
            var ngaydanhgia = dg.NgayDanhGia.Value.ToString("HH:mm:ss, dd/MM/yyyy");
        
            var result = await _DanhGiaservice.DeleteDanhGia(id);

            if (result)
            {
                if (user != null)
                {
                    await _emailSender.SendEmailAsync(user.Email, "Thông báo về việc xóa đánh giá của bạn",
    $"Bạn đã vi phạm điều khoản trên Web bán giày thể thao Bazaizai. Đánh giá vào thời gian: {ngaydanhgia} đã bị xóa. Lí do: {liDo} . Mọi thắc mắc xin vui lòng liên hệ đội ngũ hỗ trợ 0369426223.");

                }
                return RedirectToAction("DanhSachDanhGiaChuaDuyet");

            }
            else
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> DuyetNhieuDanhGia(List<string> ids)
        {
            foreach (var id in ids)
            {
                await _DanhGiaservice.DuyetDanhGia(id);
            }
            return RedirectToAction("DanhSachDanhGiaChuaDuyet");
        }
        public async Task<IActionResult> XoaNhieuDanhGia(List<string> ids, string liDo)
        {
            foreach (var id in ids)
            {
                var dg = await _DanhGiaservice.GetDanhGiaById(id);

                var user = await _userManager.FindByIdAsync(dg.IdNguoiDung);
                var ngaydanhgia = dg.NgayDanhGia.Value.ToString("HH:mm:ss, dd/MM/yyyy");


                await _DanhGiaservice.DeleteDanhGia(id);
                if (user != null)
                {
                    await _emailSender.SendEmailAsync(user.Email, "Thông báo về việc xóa đánh giá của bạn",
   $"Bạn đã vi phạm điều khoản trên Web bán giày thể thao Bazaizai. Đánh giá vào thời gian: {ngaydanhgia} đã bị xóa. Lí do: {liDo} . Mọi thắc mắc xin vui lòng liên hệ đội ngũ hỗ trợ 0369426223.");

                }


            }
            return RedirectToAction("DanhSachDanhGiaChuaDuyet");
        }



    }
}
