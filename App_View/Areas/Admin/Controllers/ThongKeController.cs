using App_Data.DbContextt;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.DanhGia;
using App_Data.ViewModels.ThongKe;
using Google;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System.Globalization;
using static App_Data.Repositories.TrangThai;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class ThongKeController : Controller
    {
        private BazaizaiContext db = new BazaizaiContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DoanhThuTheoSanPham()
        {
            return View();
        }
        public IActionResult DoanhThuTheoDonHang()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from a in db.HoaDons
                        join b in db.hoaDonChiTiets on a.IdHoaDon equals b.IdHoaDon
                        join c in db.sanPhamChiTiets on b.IdSanPhamChiTiet equals c.IdChiTietSp
                        where a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.TaiQuay || a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao && a.NgayTao != null
                        select new ThongKeViewModel
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            SoLuong = (int)b.SoLuong,
                            GiaGoc = (double)c.GiaNhap,
                            GiaBan = (double)b.GiaBan,
                        };

            //fromDate = "2023-11-23";
            if (!string.IsNullOrEmpty(fromDate))
            {

                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao <= endDate);
            }

            if (string.IsNullOrEmpty(toDate) && string.IsNullOrEmpty(fromDate))
            {
                DateTime currentDate = DateTime.Now;
                DateTime startDate = currentDate.AddDays(-7);
                query = query.Where(x => x.NgayTao >= startDate);
                query = query.Where(x => x.NgayTao <= currentDate);
            }
            var result = query.GroupBy(x => x.NgayTao.Date)
                .Select(x => new
                {
                    Date = x.Key,
                    TongNhap = x.Sum(y => y.SoLuong * y.GiaGoc),
                    TongBan = x.Sum(y => y.SoLuong * y.GiaBan),
                }).Select(x => new
                {
                    Date = x.Date,
                    DoanhThu = x.TongBan,
                    LoiNhuan = x.TongBan - x.TongNhap
                });

            return Json(new { Data = result });

        }

        [HttpGet]
        public ActionResult ThongKeTheoSanPhamOnline(string fromDate, string toDate)
        {

            var query = from a in db.HoaDons
                        join b in db.hoaDonChiTiets on a.IdHoaDon equals b.IdHoaDon
                        join c in db.sanPhamChiTiets on b.IdSanPhamChiTiet equals c.IdChiTietSp
                        join d in db.SanPhams on c.IdSanPham equals d.IdSanPham
                        join e in db.thongTinGiaoHangs on a.IdThongTinGH equals e.IdThongTinGH
                        where a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao && a.NgayTao != null
                        select new ThongKeTheoSanPhamBanOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            SoLuong = (int)b.SoLuong,
                            GiaGoc = (double)c.GiaNhap,
                            GiaBan = (double)c.GiaBan,
                            GiaThucTe = (double)c.GiaThucTe,
                            TenSp = d.TenSanPham,
                            IdnguoiDung = a.IdNguoiDung,
                            TongTien = (double)a.TongTien,
                            TienGiam = a.TienGiam,
                            IdDonHang = a.IdHoaDon,
                            SoDt = e.SDT,
                            MaSp = d.MaSanPham

                        };
            if (!string.IsNullOrEmpty(fromDate))
            {

                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao <= endDate);
            }

            if (string.IsNullOrEmpty(toDate) && string.IsNullOrEmpty(fromDate))
            {
                DateTime currentDate = DateTime.Now;
                DateTime startDate = currentDate.AddDays(-7);
                query = query.Where(x => x.NgayTao >= startDate);
                query = query.Where(x => x.NgayTao <= currentDate);
            }
            var totalTienGiam = query.Sum(y => y.TienGiam);
            var result = query.GroupBy(x => x.MaSp)
            .Select(x => new
            {
                MaSanPham = x.Key,
                TenSanPham = x.Select(y => y.TenSp).Distinct(),
                SoLuongHangBan = x.Sum(y => y.SoLuong),
                TongNhap = x.Sum(y => y.SoLuong * y.GiaGoc),
                TongBanThucTe = x.Sum(y => y.SoLuong * y.GiaThucTe),
                TongBan = x.Sum(y => y.SoLuong * y.GiaBan),
                SoLuongDonHang = x.Select(y => y.IdDonHang).Distinct().Count(),
                SoLuongKhachHang = x.Select(y => y.SoDt).Distinct().Count(),

            }).Select(x => new
            {
                DoanhThu = x.TongBanThucTe,
                SoLuongKhachHang = x.SoLuongKhachHang,
                SoLuongDonHang = x.SoLuongDonHang,
                SanPham = x.TenSanPham,
                SoLuongHangBan = x.SoLuongHangBan,
                TienHang = x.TongNhap,
                ChietKhauSanPham = x.TongBan - x.TongBanThucTe,
                MaSanPham = x.MaSanPham,
                ChietKhauHoaDon = totalTienGiam,
            }).ToList();

            return Json(new { Data = result });
        }


        [HttpGet]
        public ActionResult ThongKeTheoHoaDonOnline(string fromDate, string toDate)
        {

            var query = from a in db.HoaDons
                        join b in db.hoaDonChiTiets on a.IdHoaDon equals b.IdHoaDon
                        join c in db.sanPhamChiTiets on b.IdSanPhamChiTiet equals c.IdChiTietSp
                        join d in db.SanPhams on c.IdSanPham equals d.IdSanPham
                        join e in db.thongTinGiaoHangs on a.IdThongTinGH equals e.IdThongTinGH
                        where a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao && a.NgayTao != null
                        select new ThongKeTheoSanPhamBanOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            SoLuong = (int)b.SoLuong,
                            GiaGoc = (double)c.GiaNhap,
                            GiaBan = (double)c.GiaBan,
                            GiaThucTe = (double)c.GiaThucTe,
                            TenSp = d.TenSanPham,
                            IdnguoiDung = a.IdNguoiDung,
                            TongTien = (double)a.TongTien,
                            TienGiam = a.TienGiam,
                            IdDonHang = a.IdHoaDon,
                            SoDt = e.SDT,
                            MaSp = d.MaSanPham

                        };
            if (!string.IsNullOrEmpty(fromDate))
            {

                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao <= endDate);
            }

            if (string.IsNullOrEmpty(toDate) && string.IsNullOrEmpty(fromDate))
            {
                DateTime currentDate = DateTime.Now;
                DateTime startDate = currentDate.AddDays(-7);
                query = query.Where(x => x.NgayTao >= startDate);
                query = query.Where(x => x.NgayTao <= currentDate);
            }
            var totalTienGiam = query.Sum(y => y.TienGiam);
            var result = query.GroupBy(x => x.MaSp)
            .Select(x => new
            {
                MaSanPham = x.Key,
                TenSanPham = x.Select(y => y.TenSp).Distinct(),
                SoLuongHangBan = x.Sum(y => y.SoLuong),
                TongNhap = x.Sum(y => y.SoLuong * y.GiaGoc),
                TongBanThucTe = x.Sum(y => y.SoLuong * y.GiaThucTe),
                TongBan = x.Sum(y => y.SoLuong * y.GiaBan),
                SoLuongDonHang = x.Select(y => y.IdDonHang).Distinct().Count(),
                SoLuongKhachHang = x.Select(y => y.SoDt).Distinct().Count(),

            }).Select(x => new
            {
                DoanhThu = x.TongBanThucTe,
                SoLuongKhachHang = x.SoLuongKhachHang,
                SoLuongDonHang = x.SoLuongDonHang,
                SanPham = x.TenSanPham,
                SoLuongHangBan = x.SoLuongHangBan,
                TienHang = x.TongNhap,
                ChietKhauSanPham = x.TongBan - x.TongBanThucTe,
                MaSanPham = x.MaSanPham,
                ChietKhauHoaDon = totalTienGiam,
            }).ToList();

            return Json(new { Data = result });
        }

    }
}
