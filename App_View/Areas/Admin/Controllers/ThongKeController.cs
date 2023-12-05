using App_Data.DbContextt;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.DanhGia;
using App_Data.ViewModels.ThongKe;
using DocumentFormat.OpenXml.Spreadsheet;
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
        private IQueryable<ThongKeDoanhThuOnline> baseQuery;
        private IQueryable<ThongKeDoanhThuOnline> baseQueryNgay;
        private BazaizaiContext db = new BazaizaiContext();
        public ThongKeController()
        {
            baseQuery = from a in db.HoaDons
                        join b in db.hoaDonChiTiets on a.IdHoaDon equals b.IdHoaDon
                        join c in db.sanPhamChiTiets on b.IdSanPhamChiTiet equals c.IdChiTietSp
                        join e in db.thongTinGiaoHangs on a.IdThongTinGH equals e.IdThongTinGH
                        where a.TrangThaiGiaoHang != (int)TrangThaiGiaoHang.TaiQuay && a.NgayTao != null
                        select new ThongKeDoanhThuOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            TongTien = (double)a.TongTien,
                            TienGiam = a.TienGiam,
                            MaDonHang = a.MaHoaDon,
                            TienShip = a.TienShip,
                            TrangThaiGiaoHang = a.TrangThaiGiaoHang,
                            TenNguoiNhan = e.TenNguoiNhan,
                            SoDt = e.SDT,
                            DiaChi = e.DiaChi,
                            LiDoHuy = a.LiDoHuy,
                            GiaBan = (double)b.GiaBan,
                            GiaGoc = (double)b.GiaGoc,
                            SoLuong=(int)b.SoLuong,
                            TrangThaiThanhToan=a.TrangThaiThanhToan

                        };

        }

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

        public IActionResult DoanhThuTheoDonHangTheoGio()
        {
            return View();
        }
        public IActionResult DoanhThuTheoDonHangTheoThang()
        {
            return View();
        }
        public IActionResult DoanhThuTheoDonHangTheoNam()
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


        public IActionResult DoanhThuTheoDonHangTheoNgay()
        {
            return View();
        }
        /// Doanh thu theo hóa đơn qua khoảng thời gian
        [HttpGet]
        public ActionResult BieuDoThongKeHoaDonOnlineTheoNgay(string fromDate, string toDate)
        {
            var query = from a in db.HoaDons

                        where a.TrangThaiGiaoHang != (int)TrangThaiGiaoHang.TaiQuay && a.NgayTao != null
                        select new ThongKeDoanhThuOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            TongTien = (double)a.TongTien,
                            MaDonHang = a.MaHoaDon,
                            NgayNhan = a.NgayNhan,
                            TrangThaiGiaoHang = a.TrangThaiGiaoHang,

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
            var result = query.GroupBy(x => x.NgayTao.Date)
              .Select(x => new
              {
                  NgayTao = x.Key,
                  TongSoDonHang = x.Count(y => y.MaDonHang != null),
                  SoLuongDonHangHuy = x.Count(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaHuy),
                  SoLuongDonHangThanhCong = x.Count(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao),
                  DoanhThu = x.Where(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao).Sum(y => y.TongTien),
              }).Select(x => new
              {
                  NgayTao = x.NgayTao,
                  TongSoDonHang = x.TongSoDonHang,
                  SoLuongDonHangThanhCong = x.SoLuongDonHangThanhCong,
                  SoLuongDonHangHuy = x.SoLuongDonHangHuy,
                  DoanhThu = x.DoanhThu,
              }).ToList();


            return Json(new { Data = result });
        }
        [HttpGet]
        public ActionResult ThongTinThongKeHoaDonOnlineTheoNgay(string fromDate, string toDate)
        {
            if (!string.IsNullOrEmpty(fromDate))
            {

                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                baseQuery = baseQuery.Where(x => x.NgayTao >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                baseQuery = baseQuery.Where(x => x.NgayTao <= endDate);
            }

            if (string.IsNullOrEmpty(toDate) && string.IsNullOrEmpty(fromDate))
            {
                DateTime currentDate = DateTime.Now;
                DateTime startDate = currentDate.AddDays(-7);
                baseQuery = baseQuery.Where(x => x.NgayTao >= startDate);
                baseQuery = baseQuery.Where(x => x.NgayTao <= currentDate);
            }
            var result = baseQuery.GroupBy(x => x.MaDonHang)
               .Select(x => new
               {
                   MaDonHang = x.Key,
                   TongGoc = x.Sum(y => y.SoLuong * y.GiaGoc),
                   TongBan = x.Sum(y => y.SoLuong * y.GiaBan),
                   NgayTao = x.Select(y => y.NgayTao).FirstOrDefault(),
                   TrangThaiThanhToan = GetTrangThaiThanhToan(x.First().TrangThaiThanhToan ?? 10),
                   GiaTriDon = x.Select(y => y.TongTien).Distinct().FirstOrDefault(),
                   TinhTrang = GetTinhTrang(x.First().TrangThaiGiaoHang ?? 7),
                   ChietKhauHoaDon = x.Select(y => y.TienGiam).FirstOrDefault(),//voucher
                   TienShip = x.Select(y => y.TienShip).FirstOrDefault(),
                   TenKhach = x.Select(y => y.TenNguoiNhan).FirstOrDefault(),
                   SDT = x.Select(y => y.SoDt).FirstOrDefault().ToString(),
                   Lidohuy = x.Select(y => y.LiDoHuy).FirstOrDefault() ?? "",
                   TinhThanh = x.Select(y => GetTinhThanhFromDiaChi(y.DiaChi ?? "")).FirstOrDefault()
               }).AsEnumerable()
             .Select(x => new
             {
                 TongKhuyenMai=x.TongGoc-x.TongBan,
                 MaDonHang =x.MaDonHang,
                 ChietKhauHoaDon = x.ChietKhauHoaDon,
                 GiaTriDon=x.GiaTriDon,
                 TinhTrang=x.TinhTrang,
                 TienShip=x.TienShip,
                 TenKhach=x.TenKhach,
                 SDT=x.SDT,
                 TrangThaiThanhToan=x.TrangThaiThanhToan,
                 Lidohuy =x.Lidohuy,
                 TinhThanh=x.TinhThanh,
                 ThoiGian = DateTime.ParseExact($"{x.NgayTao.Hour}:{x.NgayTao.Minute}", "H:m", null).ToString("HH:mm"),
                 Ngay = x.NgayTao.Date.ToString(),
             
                
             })
 .OrderBy(x => x.Ngay)
 .ThenBy(x => x.ThoiGian)
 .ToList();

            return Json(new { Data = result });

        }
        public static string GetTinhTrang(int trangThai)
        {
            switch (trangThai)
            {
                case 0:
                    return "Tại quầy";
                case 1:
                    return "Chờ xác nhận";
                case 2:
                    return "Chờ lấy hàng";
                case 3:
                    return "Đang giao";
                case 4:
                    return "Hoàn thành";
                case 5:
                    return "Đã hủy";
                default:
                    return "Không xác định";
            }
        }
        public static string GetTrangThaiThanhToan(int trangThai)
        {
            switch (trangThai)
            {
                case 0:
                    return "Chưa thanh toán";
                case 1:
                    return "Đã thanh toán";
                case 2:
                    return "Hủy";
                default:
                    return "Không xác định";
            }
        }
        public static string GetTinhThanhFromDiaChi(string diaChi)
        {
            if (diaChi == "") return "";
            else if (diaChi == null) return "";
            string[] diaChiArray = diaChi.Split(',');
            string tinhThanh = diaChiArray[diaChiArray.Length - 1].Trim();
            return tinhThanh;
        }
        [HttpGet]
        public ActionResult BieuDoThongKeHoaDonOnlineTheoGio(string fromDate, string toDate)
        {
            var query = from a in db.HoaDons

                        where a.TrangThaiGiaoHang != (int)TrangThaiGiaoHang.TaiQuay && a.NgayTao != null
                        select new ThongKeDoanhThuOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            TongTien = (double)a.TongTien,
                            MaDonHang = a.MaHoaDon,
                            NgayNhan = a.NgayNhan,
                            TrangThaiGiaoHang = a.TrangThaiGiaoHang
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
                DateTime startDate = currentDate.AddHours(-24);
                query = query.Where(x => x.NgayTao >= startDate);
                query = query.Where(x => x.NgayTao <= currentDate);
            }
            var result = query.GroupBy(x => new { x.NgayTao.Date, x.NgayTao.Hour })
        .Select(x => new
        {
            NgayTao = $"{x.Key.Hour:00}h {x.Key.Date:dd/MM/yyyy}", // Format ngày và giờ
            TongSoDonHang = x.Count(y => y.MaDonHang != null),
            SoLuongDonHangHuy = x.Count(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaHuy),
            SoLuongDonHangThanhCong = x.Count(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao),
            DoanhThu = x.Sum(y => y.TongTien),
        }).AsEnumerable() // Chuyển sang danh sách ở bên phía máy khách
         .OrderBy(x => DateTime.ParseExact(x.NgayTao, "HH\\h dd/MM/yyyy", CultureInfo.InvariantCulture)) // Sắp xếp theo ngày tạo
        .ToList();

            return Json(new { Data = result });
        }

        [HttpGet]
        public ActionResult BieuDoThongKeHoaDonOnlineTheoThang(string fromDate, string toDate)
        {
            var query = from a in db.HoaDons
                        where a.TrangThaiGiaoHang != (int)TrangThaiGiaoHang.TaiQuay && a.NgayTao != null
                        select new ThongKeDoanhThuOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            TongTien = (double)a.TongTien,
                            MaDonHang = a.MaHoaDon,
                            NgayNhan = a.NgayNhan,
                            TrangThaiGiaoHang = a.TrangThaiGiaoHang
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
                DateTime startDate = currentDate.AddMonths(-6);
                query = query.Where(x => x.NgayTao >= startDate);
                query = query.Where(x => x.NgayTao <= currentDate);
            }
            var result = query.GroupBy(x => new { x.NgayTao.Month, x.NgayTao.Year })
    .Select(x => new
    {
        NgayTao = $"{x.Key.Month}/{x.Key.Year}",
        TongSoDonHang = x.Count(y => y.MaDonHang != null),
        SoLuongDonHangHuy = x.Count(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaHuy),
        SoLuongDonHangThanhCong = x.Count(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao),
        DoanhThu = x.Sum(y => y.TongTien),
    }).AsEnumerable()
   .OrderBy(x => DateTime.ParseExact($"{x.NgayTao}", "MM/yyyy", CultureInfo.InvariantCulture))
    .ToList();


            return Json(new { Data = result });
        }
        [HttpGet]

        public ActionResult BieuDoThongKeHoaDonOnlineTheoNam(string fromDate, string toDate)
        {
            var query = from a in db.HoaDons
                        where a.TrangThaiGiaoHang != (int)TrangThaiGiaoHang.TaiQuay && a.NgayTao != null
                        select new ThongKeDoanhThuOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            TongTien = (double)a.TongTien,
                            MaDonHang = a.MaHoaDon,
                            NgayNhan = a.NgayNhan,
                            TrangThaiGiaoHang = a.TrangThaiGiaoHang
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
                DateTime startDate = currentDate.AddYears(-3);
                query = query.Where(x => x.NgayTao >= startDate);
                query = query.Where(x => x.NgayTao <= currentDate);
            }
            var result = query.GroupBy(x => x.NgayTao.Year)
               .Select(x => new
               {
                   NgayTao = x.Key,
                   TongSoDonHang = x.Count(y => y.MaDonHang != null),
                   SoLuongDonHangHuy = x.Count(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaHuy),
                   SoLuongDonHangThanhCong = x.Count(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao),
                   DoanhThu = x.Sum(y => y.TongTien),
               })
               .OrderBy(x => x.NgayTao)
    .ToList();


            return Json(new { Data = result });
        }



    }
}
