using App_Data.DbContext;
using App_Data.Models;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_View.IServices;
using App_View.Models.ViewModels;
using App_View.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,NhanVien")]
    public class HoaDonController : Controller
    {

		private readonly IVoucherNguoiDungservices _VoucherNguoiDungservices;
		private readonly IVoucherservices _Voucherservices;
		private readonly IHoaDonChiTietservices _HoaDonChiTietservices; private readonly SignInManager<NguoiDung> _signInManager;
		private readonly UserManager<NguoiDung> _userManager;
		private readonly IHoaDonServices _hoaDonServices;
        private readonly ISanPhamChiTietservice SanPhamChiTietservice;
        BazaizaiContext context;
		public HoaDonController(ISanPhamChiTietservice SanPhamChiTietservice, IVoucherNguoiDungservices VoucherNguoiDungservices, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager)
		{
			_hoaDonServices = new HoaDonServices();
			context = new BazaizaiContext();
			this.SanPhamChiTietservice = SanPhamChiTietservice;
			_VoucherNguoiDungservices = VoucherNguoiDungservices;
			_HoaDonChiTietservices = new HoaDonChiTietservices();
			_signInManager = signInManager;
			_userManager = userManager;
		}
		public IActionResult TongQuanHoaDon()
        {
            return View();
        }
        public async Task<IActionResult> QuanLyHoaDonAsync(int trangThaiHD, string search)
        {
            var lstHoaDon = (await _hoaDonServices.GetHoaDon()).ToList();
            ViewBag.NguoiDung = context.NguoiDungs.AsNoTracking().ToList();
            ViewBag.KhachHang = context.KhachHangs.AsNoTracking().ToList();
            ViewBag.NguoiTao = "";
            if (!string.IsNullOrEmpty(search))
            {
                lstHoaDon = lstHoaDon.Where(x=>x.MaHoaDon.ToUpper().Contains(search.ToUpper())).ToList();
            }
            if (trangThaiHD==1)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang != 0&& x.TrangThaiGiaoHang != 5);
                return PartialView("QuanLyHoaDon", lstHoaDonOnline);
            }
            if (trangThaiHD == 2)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang != 0&& x.TrangThaiThanhToan==1 && x.TrangThaiGiaoHang != 5);
                return PartialView("QuanLyHoaDon", lstHoaDonOnline);
            }
            if (trangThaiHD == 3)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang != 0 && x.TrangThaiThanhToan == 0 && x.TrangThaiGiaoHang != 5);
                return PartialView("QuanLyHoaDon", lstHoaDonOnline);
            }
            if (trangThaiHD == 5)
            {
                var lstHoaDonDaHuy = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 5||x.TrangThaiThanhToan == 2);
                ViewBag.NguoiTao = "Người tạo";
                return PartialView("QuanLyHoaDon", lstHoaDonDaHuy);
            }
            if (trangThaiHD == 0)
            {
                var lstHoaDonTQ = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 0 && x.TrangThaiThanhToan ==1);
                ViewBag.NguoiTao = "Người tạo";
                return PartialView("QuanLyHoaDon", lstHoaDonTQ);
            }
            return PartialView("QuanLyHoaDon", lstHoaDon);
        }

        public async Task<IActionResult> ChiTietHoaDonAsync(string id)
        {
           
            var hoaDon = (await _hoaDonServices.GetHoaDon()).FirstOrDefault(x => x.IdHoaDon == id);
			var kh = context.KhachHangs.AsNoTracking().FirstOrDefault(x => x.IdKhachHang == hoaDon.IdKhachHang);
			var pttt = context.PhuongThucThanhToanChiTiets.AsNoTracking().Where(x => x.IdHoaDon == hoaDon.IdHoaDon).ToList();
			double tongTien = pttt.Sum(x => Convert.ToDouble(x.SoTien));
			ViewBag.TienKhachTra = tongTien;
			ViewBag.TenNguoiNhan = hoaDon.TenNguoiNhan;
			if (hoaDon.IdNguoiDung != null)
			{
				var nguoiDung = context.NguoiDungs.AsNoTracking().FirstOrDefault(x => x.Id == hoaDon.IdNguoiDung);

				ViewBag.NguoiDung = nguoiDung.TenNguoiDung + " " + nguoiDung.MaNguoiDung;
				if(kh!=null)
				{
					ViewBag.Sdt = kh.SDT;
				}
			}
			else ViewBag.NguoiDung = null;

			if (hoaDon?.IdKhachHang != null)
			{
				ViewBag.KhachHang = kh.TenKhachHang;
			}
			else ViewBag.KhachHang = null;
			var hoaDonChiTiet = context.HoaDons.FirstOrDefault(x => x.IdHoaDon == hoaDon!.IdHoaDon!);
            ViewBag.TTGH = context.HoaDons.FirstOrDefault(x => x.IdThongTinGH == hoaDon!.IdThongTinGH!)!.DiaChi!;
            ViewData["MAHD"] = hoaDon.MaHoaDon;
            ViewData["NGAYTAO"] = hoaDon.NgayTao;
			ViewData["TIENSHIP"] = hoaDon.TienShip;
            ViewData["TONGTIEN"] = hoaDon.TongTien;
            ViewData["TIENGIAM"] = hoaDon.TienGiam;
			var HDCT = context.HoaDonChiTiets.Where(x => x.IdHoaDon == hoaDon.IdHoaDon);
            ViewBag.HDCT= HDCT;
            return PartialView("_ChiTietHoaDon", hoaDonChiTiet);
        }
        [HttpGet]
        public async Task<IActionResult> InHoaDonTaiQuayAsync(string MaHD)
        {
            var hoaDon = (await _hoaDonServices.GetHoaDon()).FirstOrDefault(x=>x.MaHoaDon==MaHD);
			var hoaDonChiTiet = context.HoaDons.FirstOrDefault(x => x.IdHoaDon == hoaDon.IdHoaDon);
            var kh = context.KhachHangs.AsNoTracking().FirstOrDefault(x => x.IdKhachHang == hoaDon.IdKhachHang);
            var pttt = context.PhuongThucThanhToanChiTiets.AsNoTracking().Where(x => x.IdHoaDon == hoaDon.IdHoaDon).ToList();
			double tongTien = pttt.Sum(x => Convert.ToDouble(x.SoTien));
			ViewBag.TienKhachTra = tongTien;
			ViewBag.TenNguoiNhan = hoaDon.TenNguoiNhan;
			if (hoaDon.IdNguoiDung != null)
			{
				var nguoiDung = context.NguoiDungs.AsNoTracking().FirstOrDefault(x => x.Id == hoaDon.IdNguoiDung);

				ViewBag.NguoiDung = nguoiDung.TenNguoiDung + " " + nguoiDung.MaNguoiDung;
				if(kh!=null)
				{
					ViewBag.Sdt = kh.SDT;
				}
			}
			else ViewBag.NguoiDung = null;

			if (hoaDon.IdKhachHang != null)
			{
				ViewBag.KhachHang = kh.TenKhachHang;
			}
			else ViewBag.KhachHang = null;
			ViewBag.TTGH = context.HoaDons.FirstOrDefault(x => x.IdThongTinGH == hoaDon.IdThongTinGH!)!.DiaChi!; 
			ViewData["MAHD"] = hoaDon.MaHoaDon;
			ViewData["NGAYTAO"] = hoaDon.NgayTao;
			ViewData["TIENSHIP"] = hoaDon.TienShip;
			ViewData["TONGTIEN"] = hoaDon.TongTien-hoaDon.TienGiam;
			ViewData["TIENGIAM"] = hoaDon.TienGiam;
			var HDCT = context.HoaDonChiTiets.Where(x => x.IdHoaDon == hoaDon.IdHoaDon);
            ViewBag.HDCT = HDCT;
            return PartialView("InHoaDonTaiQuay", hoaDonChiTiet);
        }
        
    }
}
