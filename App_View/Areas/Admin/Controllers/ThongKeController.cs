using App_Data.DbContext;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.DanhGia;
using App_Data.ViewModels.ThongKe;
using App_View.IServices;
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
    [Authorize(Roles = "Admin , NhanVien")]
    public class ThongKeController : Controller
    {
        private IQueryable<ThongKeDoanhThuOnline> baseQuery;
        private BazaizaiContext db = new BazaizaiContext();
        private IThongKeService _thongKeService;
        private readonly HttpClient _httpClient;
        public ThongKeController(IThongKeService thongKeService)
        {
            _httpClient = new HttpClient();
            _thongKeService = thongKeService;
            baseQuery = from a in db.HoaDons
                        join e in db.ThongTinGiaoHangs on a.IdThongTinGH equals e.IdThongTinGH into thongTinGiaoHangGroup
                        from e in thongTinGiaoHangGroup.DefaultIfEmpty()
                        join d in db.KhachHangs on a.IdKhachHang equals d.IdKhachHang into khachHangGroup
                        from d in khachHangGroup.DefaultIfEmpty()
                        select new ThongKeDoanhThuOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            TongTien = (double)a.TongTien,
                            MaDonHang = a.MaHoaDon,
                            TienShip = a.TienShip,
                            TrangThaiGiaoHang = a.TrangThaiGiaoHang,
                            LiDoHuy = a.LiDoHuy,
                            TrangThaiThanhToan = a.TrangThaiThanhToan,
                            //TenNguoiNhan = e != null ? e.TenNguoiNhan : null,
                            //SoDt = e != null ? e.SDT : null,
                            //DiaChi = e != null ? e.DiaChi : null,


                            //SDTKhachHang = d != null ? d.SDT : null,
                            //TenKhachHang = d != null ? d.TenKhachHang : null,

                            TenNguoiNhan = (e != null) ? e.TenNguoiNhan : null,
                            SoDt = (e != null) ? e.SDT : null,
                            DiaChi = (e != null) ? e.DiaChi : null,
                            SDTKhachHang = (d != null) ? d.SDT : null,
                            TenKhachHang = (d != null) ? d.TenKhachHang : null
                        };
        }
        public IActionResult TongQuan()
        {
            return View();
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
                        join b in db.HoaDonChiTiets on a.IdHoaDon equals b.IdHoaDon
                        join c in db.SanPhamChiTiets on b.IdSanPhamChiTiet equals c.IdChiTietSp
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
                        join b in db.HoaDonChiTiets on a.IdHoaDon equals b.IdHoaDon
                        join c in db.SanPhamChiTiets on b.IdSanPhamChiTiet equals c.IdChiTietSp
                        join d in db.SanPhams on c.IdSanPham equals d.IdSanPham
                        //join e in db.ThongTinGiaoHangs on a.IdThongTinGH equals e.IdThongTinGH
                        where (a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao || a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.TaiQuay) && a.TrangThaiThanhToan == (int)TrangThaiHoaDon.DaThanhToan
                        select new ThongKeTheoSanPhamBanOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            SoLuong = (int)b.SoLuong,
                            //GiaGoc = (double)c.GiaNhap,
                            GiaBan = (double)c.GiaBan,
                            GiaThucTe = (double)c.GiaThucTe,
                            TenSp = d.TenSanPham,
                            IdnguoiDung = a.IdNguoiDung,
                            TongTien = (double)a.TongTien,
                            //TienGiam = a.TienGiam,
                            IdDonHang = a.IdHoaDon,
                            //SoDt = e.SDT,
                            MaSp = d.MaSanPham,
                            Mactsp = c.Ma,

                        };
            if (!string.IsNullOrEmpty(fromDate))
            {

                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao.Date >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao.Date <= endDate);
            }

            if (string.IsNullOrEmpty(toDate) && string.IsNullOrEmpty(fromDate))
            {
                DateTime currentDate = DateTime.Now;
                DateTime startDate = currentDate.AddDays(-7);
                query = query.Where(x => x.NgayTao.Date >= startDate);
                query = query.Where(x => x.NgayTao.Date <= currentDate);
            }
            //var totalTienGiam = query.Sum(y => y.TienGiam);
            var result = query.GroupBy(x => x.MaSp)
            .Select(x => new
            {
                MaSanPham = x.Key,
                TenSanPham = x.Select(y => y.TenSp).Distinct(),
                SoLuongHangBan = x.Sum(y => y.SoLuong),
                //TongNhap = x.Sum(y => y.SoLuong * y.GiaGoc),
                TongBanThucTe = x.Sum(y => y.SoLuong * y.GiaThucTe),
                TongBan = x.Sum(y => y.SoLuong * y.GiaBan),
                SoLuongDonHang = x.Select(y => y.IdDonHang).Distinct().Count(),
                //SoLuongKhachHang = x.Select(y => y.SoDt).Distinct().Count(),

            }).Select(x => new
            {
                MaSanPham = x.MaSanPham,
                DoanhThu = x.TongBanThucTe,
                //SoLuongKhachHang = x.SoLuongKhachHang,
                SoLuongDonHang = x.SoLuongDonHang,
                SanPham = x.TenSanPham,
                SoLuongHangBan = x.SoLuongHangBan,
                //TienHang = x.TongNhap,
                ChietKhauSanPham = x.TongBan - x.TongBanThucTe,
                //ChietKhauHoaDon = totalTienGiam,
            }).ToList();

            return Json(new { Data = result });
        }

        [HttpGet]
        public ActionResult ThongTinThongKeTheoSanPhamOnline(string fromDate, string toDate)
        {

            var query = from a in db.HoaDons
                        join b in db.HoaDonChiTiets on a.IdHoaDon equals b.IdHoaDon
                        join c in db.SanPhamChiTiets on b.IdSanPhamChiTiet equals c.IdChiTietSp
                        join d in db.SanPhams on c.IdSanPham equals d.IdSanPham
                        select new ThongKeTheoSanPhamBanOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            SoLuong = (int)b.SoLuong,
                            GiaBan = (double)c.GiaBan,
                            GiaThucTe = (double)c.GiaThucTe,
                            TenSp = d.TenSanPham,
                            IdnguoiDung = a.IdNguoiDung,
                            TongTien = (double)a.TongTien,
                            IdDonHang = a.IdHoaDon,
                            MaSp = d.MaSanPham,
                            Mactsp = c.Ma,
                            MaHoaDon = a.MaHoaDon,



                        };
            if (!string.IsNullOrEmpty(fromDate))
            {

                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao.Date >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao.Date <= endDate);
            }

            if (string.IsNullOrEmpty(toDate) && string.IsNullOrEmpty(fromDate))
            {
                DateTime currentDate = DateTime.Now;
                DateTime startDate = currentDate.AddDays(-7);
                query = query.Where(x => x.NgayTao.Date >= startDate);
                query = query.Where(x => x.NgayTao.Date <= currentDate);
            }
            //var totalTienGiam = query.Sum(y => y.TienGiam);
            var result = query.GroupBy(x => x.MaSp)
            .Select(x => new
            {
                MaSanPham = x.Key,
                TenSanPham = x.Select(y => y.TenSp).Distinct(),
                SoLuongHangBan = x.Sum(y => y.SoLuong),
                //TongNhap = x.Sum(y => y.SoLuong * y.GiaGoc),
                TongBanThucTe = x.Sum(y => y.SoLuong * y.GiaThucTe),
                TongBan = x.Sum(y => y.SoLuong * y.GiaBan),
                SoLuongDonHang = x.Select(y => y.IdDonHang).Distinct().Count(),
                //SoLuongKhachHang = x.Select(y => y.SoDt).Distinct().Count(),

            }).Select(x => new
            {
                MaSanPham = x.MaSanPham,
                DoanhThu = x.TongBanThucTe,
                //SoLuongKhachHang = x.SoLuongKhachHang,
                SoLuongDonHang = x.SoLuongDonHang,
                SanPham = x.TenSanPham,
                SoLuongHangBan = x.SoLuongHangBan,
                //TienHang = x.TongNhap,
                ChietKhauSanPham = x.TongBan - x.TongBanThucTe,
                //ChietKhauHoaDon = totalTienGiam,
            }).ToList();

            return Json(new { Data = result });
        }

        public IActionResult DoanhThuTheoDonHangTheoNgay()
        {
            return View();
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
                    return "Đã giao";
                case 5:
                    return "Đã hủy";
                case 6:
                    return "Trả hàng";
                case 7:
                    return "Chờ hủy";
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
        /// Doanh thu theo hóa đơn qua khoảng thời gian
        [HttpGet]
        public ActionResult BieuDoThongKeHoaDonOnlineTheoNgay(string fromDate, string toDate)
        {
            var query = from a in db.HoaDons

                        where a.NgayTao != null
                        select new ThongKeDoanhThuOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            TongTien = (double)a.TongTien,
                            MaDonHang = a.MaHoaDon,
                            NgayNhan = a.NgayNhan,
                            TrangThaiGiaoHang = a.TrangThaiGiaoHang,
                            TrangThaiThanhToan = a.TrangThaiThanhToan,
                            TienGiam = a.TienGiam
                        };
            if (!string.IsNullOrEmpty(fromDate))
            {

                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao.Date >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao.Date <= endDate);
            }

            if (string.IsNullOrEmpty(toDate) && string.IsNullOrEmpty(fromDate))
            {
                DateTime currentDate = DateTime.Now;
                DateTime startDate = currentDate.AddDays(-7);
                query = query.Where(x => x.NgayTao.Date >= startDate);
                query = query.Where(x => x.NgayTao.Date <= currentDate);
            }
            var result = query.GroupBy(x => x.NgayTao.Date)
              .Select(x => new
              {
                  NgayTao = x.Key,
                  TongSoDonHang = x.Count(y => y.MaDonHang != null),
                  SoLuongDonHangHuy = x.Count(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaHuy || y.TrangThaiThanhToan == (int)TrangThaiHoaDon.Huy),
                  SoLuongDonHangThanhCong = x.Count(y => (y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao || y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.TaiQuay) && y.TrangThaiThanhToan == (int)TrangThaiHoaDon.DaThanhToan),
                  DoanhThuOnline = x.Where(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao && y.TrangThaiThanhToan == (int)TrangThaiHoaDon.DaThanhToan).Sum(y => y.TongTien),
                  DoanhThuOfline = x.Where(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.TaiQuay && y.TrangThaiThanhToan == (int)TrangThaiHoaDon.DaThanhToan).Sum(y => y.TongTien),
                  TongGiam = x.Where(y => y.TienGiam != null).Sum(y => y.TienGiam)
              }).Select(x => new
              {
                  NgayTao = x.NgayTao,
                  TongSoDonHang = x.TongSoDonHang,
                  SoLuongDonHangThanhCong = x.SoLuongDonHangThanhCong,
                  SoLuongDonHangHuy = x.SoLuongDonHangHuy,
                  DoanhThu = x.DoanhThuOnline + x.DoanhThuOfline - x.TongGiam,
              }).ToList();


            return Json(new { Data = result });
        }
        [HttpGet]
        public ActionResult ThongTinThongKeHoaDonOnlineTheoNgay(string fromDate, string toDate)
        {
            if (!string.IsNullOrEmpty(fromDate))
            {

                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                baseQuery = baseQuery.Where(x => x.NgayTao.Date >= startDate.Date);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                baseQuery = baseQuery.Where(x => x.NgayTao.Date <= endDate.Date);
            }

            if (string.IsNullOrEmpty(toDate) && string.IsNullOrEmpty(fromDate))
            {
                DateTime currentDate = DateTime.Now;
                DateTime startDate = currentDate.AddDays(-7);
                baseQuery = baseQuery.Where(x => x.NgayTao.Date >= startDate.Date && x.NgayTao.Date <= currentDate.Date);
            }
            var result = baseQuery.ToList()
              .Select(x => new
              {
                  MaDonHang = x.MaDonHang,
                  GiaTriDon = x.TongTien - ((double)(x.TienGiam ?? 0)),
                  TinhTrang = GetTinhTrang(x.TrangThaiGiaoHang ?? 10),
                  TienShip = x.TienShip ?? 0,
                  TenKhach = x.TenKhachHang ?? x.TenNguoiNhan ?? "Khách vãng lai",
                  SDT = x.SoDt ?? x.SDTKhachHang ?? "",
                  TrangThaiThanhToan = GetTrangThaiThanhToan(x.TrangThaiThanhToan ?? 10),
                  Lidohuy = x.LiDoHuy ?? "",
                  TinhThanh = GetTinhThanhFromDiaChi(x.DiaChi ?? "") ?? "",
                  ThoiGian = DateTime.ParseExact($"{x.NgayTao.Hour}:{x.NgayTao.Minute}", "H:m", null).ToString("HH:mm"),
                  Ngay = x.NgayTao,


              })
 .OrderBy(x => x.Ngay)
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















        // ở đây
        [HttpGet]
        public async Task<string> ThongKeBanHang()
        {
            return await _thongKeService.ThongKeBanHang();
        }

        [HttpGet]
        public async Task<string> TrangThaiHoaDonOnline1thang()
        {
            string apiUrl = "https://localhost:7038/TrangThaiHoaDon1thang";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return apiData;
            }
            catch (HttpRequestException)
            {
                return "Failed to call the API.";
            }
        }


        [HttpGet]
        public ActionResult ThongKePhuongThucThanhToan(string fromDate, string toDate)
        {

            var query = from a in db.HoaDons
                        join b in db.HoaDonChiTiets on a.IdHoaDon equals b.IdHoaDon
                        join c in db.SanPhamChiTiets on b.IdSanPhamChiTiet equals c.IdChiTietSp
                        join d in db.SanPhams on c.IdSanPham equals d.IdSanPham
                        //join e in db.ThongTinGiaoHangs on a.IdThongTinGH equals e.IdThongTinGH
                        where (a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao || a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.TaiQuay) && a.TrangThaiThanhToan == (int)TrangThaiHoaDon.DaThanhToan
                        select new ThongKeTheoSanPhamBanOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            SoLuong = (int)b.SoLuong,
                            //GiaGoc = (double)c.GiaNhap,
                            GiaBan = (double)c.GiaBan,
                            GiaThucTe = (double)c.GiaThucTe,
                            TenSp = d.TenSanPham,
                            IdnguoiDung = a.IdNguoiDung,
                            TongTien = (double)a.TongTien,
                            //TienGiam = a.TienGiam,
                            IdDonHang = a.IdHoaDon,
                            //SoDt = e.SDT,
                            MaSp = d.MaSanPham,
                            Mactsp = c.Ma,

                        };
            if (!string.IsNullOrEmpty(fromDate))
            {

                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao.Date >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.NgayTao.Date <= endDate);
            }

            if (string.IsNullOrEmpty(toDate) && string.IsNullOrEmpty(fromDate))
            {
                DateTime currentDate = DateTime.Now;
                DateTime startDate = currentDate.AddDays(-7);
                query = query.Where(x => x.NgayTao.Date >= startDate);
                query = query.Where(x => x.NgayTao.Date <= currentDate);
            }
            //var totalTienGiam = query.Sum(y => y.TienGiam);
            var result = query.GroupBy(x => x.MaSp)
            .Select(x => new
            {
                MaSanPham = x.Key,
                TenSanPham = x.Select(y => y.TenSp).Distinct(),
                SoLuongHangBan = x.Sum(y => y.SoLuong),
                //TongNhap = x.Sum(y => y.SoLuong * y.GiaGoc),
                TongBanThucTe = x.Sum(y => y.SoLuong * y.GiaThucTe),
                TongBan = x.Sum(y => y.SoLuong * y.GiaBan),
                SoLuongDonHang = x.Select(y => y.IdDonHang).Distinct().Count(),
                //SoLuongKhachHang = x.Select(y => y.SoDt).Distinct().Count(),

            }).Select(x => new
            {
                MaSanPham = x.MaSanPham,
                DoanhThu = x.TongBanThucTe,
                //SoLuongKhachHang = x.SoLuongKhachHang,
                SoLuongDonHang = x.SoLuongDonHang,
                SanPham = x.TenSanPham,
                SoLuongHangBan = x.SoLuongHangBan,
                //TienHang = x.TongNhap,
                ChietKhauSanPham = x.TongBan - x.TongBanThucTe,
                //ChietKhauHoaDon = totalTienGiam,
            }).ToList();

            return Json(new { Data = result });
        }

    }
}
