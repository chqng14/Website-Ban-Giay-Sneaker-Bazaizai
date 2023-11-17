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
    public class HoaDonController : Controller
    {
        private readonly IHoaDonServices _hoaDonServices;
        private readonly ISanPhamChiTietService sanPhamChiTietService;
        BazaizaiContext context;
        public HoaDonController(ISanPhamChiTietService sanPhamChiTietService)
        {
            _hoaDonServices = new HoaDonServices();
            context = new BazaizaiContext();
            this.sanPhamChiTietService = sanPhamChiTietService;
        }

        public async Task<IActionResult> QuanLyHoaDonAsync()
        {
            var lstHoaDon = await _hoaDonServices.GetHoaDon();
            return View("QuanLyHoaDon",lstHoaDon);
        }
        public async Task<IActionResult> ChiTietHoaDonAsync(string id)
        {
            var sanPhamChiTietList = await sanPhamChiTietService.GetListSanPhamChiTietViewModelAsync();
            var sanPhamSaleViewModelList = new List<SanPhamSaleViewModel>();
            var hoaDon = (await _hoaDonServices.GetHoaDon()).FirstOrDefault(x => x.IdHoaDon == id);
            foreach (var pro in sanPhamChiTietList)
            {
                var trangThaiSale = (await sanPhamChiTietService.GetByKeyAsync(pro.IdChiTietSp)).TrangThaiSale;
                var trangThai = (await sanPhamChiTietService.GetByKeyAsync(pro.IdChiTietSp)).TrangThai;
                var giaThucTe = (await sanPhamChiTietService.GetByKeyAsync(pro.IdChiTietSp)).GiaThucTe;
                var sanPhamSaleViewModel = new SanPhamSaleViewModel
                {
                    SanPhamDanhSachView = pro,
                    TrangThaiSale = Convert.ToInt32(trangThaiSale),
                    TrangThai = Convert.ToInt32(trangThai),
                    GiaThucTe = giaThucTe
                };

                sanPhamSaleViewModelList.Add(sanPhamSaleViewModel);
            }
            var hoaDonChiTiet = context.HoaDons.FirstOrDefault(x => x.IdHoaDon == hoaDon.IdHoaDon);
            ViewBag.TTGH = context.thongTinGiaoHangs.FirstOrDefault(x => x.IdThongTinGH == hoaDon.IdThongTinGH);
            ViewData["MAHD"] = hoaDon.MaHoaDon;
            ViewData["NGAYTAO"] = hoaDon.NgayTao;
			ViewData["TIENSHIP"] = hoaDon.TienShip;
            ViewData["TONGTIEN"] = hoaDon.TongTien;
            ViewData["TIENGIAM"] = hoaDon.TienGiam;
			var HDCT = context.hoaDonChiTiets.Where(x => x.IdHoaDon == hoaDon.IdHoaDon);
            ViewBag.HDCT= HDCT;
            ViewBag.SPCT = sanPhamSaleViewModelList;
            return PartialView("_ChiTietHoaDon", hoaDonChiTiet);
        }
    }
}
