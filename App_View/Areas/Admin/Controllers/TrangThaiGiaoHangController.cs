using App_Data.DbContextt;
using App_Data.Models;
using App_Data.ViewModels.HoaDon;
using App_View.IServices;
using App_View.Models.ViewModels;
using App_View.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrangThaiGiaoHangController : Controller
    {
        private readonly IHoaDonServices _hoaDonServices;
        private readonly ISanPhamChiTietService sanPhamChiTietService;
        BazaizaiContext context;
        public TrangThaiGiaoHangController(ISanPhamChiTietService sanPhamChiTietService)
        {
            _hoaDonServices = new HoaDonServices();
            context = new BazaizaiContext();
            this.sanPhamChiTietService = sanPhamChiTietService;
        }
        public IActionResult TongQuanHoaDon()
        {
            return View();
        }
        public async Task<IActionResult> QuanLyHoaDonAsync(int trangThaiHD, string search)
        {
            var lstHoaDon = (await _hoaDonServices.GetHoaDon()).ToList();
            if(!string.IsNullOrEmpty(search))
            {
                lstHoaDon = lstHoaDon.Where(x=>x.MaHoaDon.ToUpper().Contains(search.ToUpper())).ToList();
            }
            if (trangThaiHD==1)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang != 0);
                return PartialView("QuanLyHoaDon", lstHoaDonOnline);
            }
            if (trangThaiHD == 2)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang != 0&& x.TrangThaiThanhToan==1);
                return PartialView("QuanLyHoaDon", lstHoaDonOnline);
            }
            if (trangThaiHD == 3)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang != 0 && x.TrangThaiThanhToan == 0);
                return PartialView("QuanLyHoaDon", lstHoaDonOnline);
            }
            if (trangThaiHD == 5)
            {
                var lstHoaDonDaHuy = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 5);
                return PartialView("QuanLyHoaDon", lstHoaDonDaHuy);
            }
            if (trangThaiHD == 0)
            {
                var lstHoaDonTQ = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 0);
                return PartialView("QuanLyHoaDon", lstHoaDonTQ);
            }
            return PartialView("QuanLyHoaDon", lstHoaDon);
        }

        public async Task<IActionResult> ChiTietHoaDonAsync(string id)
        {
           
            var hoaDon = (await _hoaDonServices.GetHoaDon()).FirstOrDefault(x => x.IdHoaDon == id);
            
            var hoaDonChiTiet = context.HoaDons.FirstOrDefault(x => x.IdHoaDon == hoaDon.IdHoaDon);
            ViewBag.TTGH = context.thongTinGiaoHangs.FirstOrDefault(x => x.IdThongTinGH == hoaDon.IdThongTinGH);
            ViewData["MAHD"] = hoaDon.MaHoaDon;
            ViewData["NGAYTAO"] = hoaDon.NgayTao;
			ViewData["TIENSHIP"] = hoaDon.TienShip;
            ViewData["TONGTIEN"] = hoaDon.TongTien;
            ViewData["TIENGIAM"] = hoaDon.TienGiam;
			var HDCT = context.hoaDonChiTiets.Where(x => x.IdHoaDon == hoaDon.IdHoaDon);
            ViewBag.HDCT= HDCT;
            return PartialView("_ChiTietHoaDon", hoaDonChiTiet);
        }
    }
}
