using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.DanhGia;
using App_Data.ViewModels.ThongKe;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static App_Data.Repositories.TrangThai;

namespace App_Api.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly IAllRepo<HoaDon> repos;
        BazaizaiContext context = new BazaizaiContext();
        DbSet<HoaDon> DanhGias;
        private IQueryable<ThongKeDoanhThuOnline> baseQuery;

        public ThongKeController()
        {
            DanhGias = context.HoaDons;
            AllRepo<HoaDon> all = new AllRepo<HoaDon>(context, DanhGias);
            repos = all;
            baseQuery = from a in context.HoaDons
                        join e in context.ThongTinGiaoHangs on a.IdThongTinGH equals e.IdThongTinGH into thongTinGiaoHangGroup
                        from e in thongTinGiaoHangGroup.DefaultIfEmpty()
                        join d in context.KhachHangs on a.IdKhachHang equals d.IdKhachHang into khachHangGroup
                        from d in khachHangGroup.DefaultIfEmpty()
                        select new ThongKeDoanhThuOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            TongTien = (double)a.TongTien,
                            MaDonHang = a.MaHoaDon,
                            TienShip = a.TienShip,
                            TrangThaiGiaoHang = a.TrangThaiGiaoHang,
                            TenNguoiNhan = e != null ? e.TenNguoiNhan : null,
                            SoDt = e != null ? e.SDT : null,
                            DiaChi = e != null ? e.DiaChi : null,
                            LiDoHuy = a.LiDoHuy,
                            TrangThaiThanhToan = a.TrangThaiThanhToan,
                            SDTKhachHang = d != null ? d.SDT : null,
                            TenKhachHang = d != null ? d.TenKhachHang : null

                        };
        }


        [HttpGet("DoanhThuTheoThang/{month}")]
        public async Task<double> DoanhThuTheoThang(int month)
        {
            var query = repos.GetAll().Where(a => (a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao || a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.TaiQuay)
                              && a.TrangThaiThanhToan == (int)TrangThaiHoaDon.DaThanhToan
                              && a.NgayTao.Value.Month == month && a.NgayTao.Value.Year == DateTime.Now.Year);



            var result = query.ToList();

            var tongTien = result.Sum(x => (x.TongTien ?? 0) - (x.TienGiam ?? 0));

            if (tongTien >= 0)
            {
                return tongTien;
            }
            else
                return 0;
        }

        [HttpGet("DonHangTheoThang/{month}/{year}")]
        public async Task<List<HoaDon>> DonHangTheoThang(int month, int year)
        {
            var query = repos.GetAll().
                        Where(a => a.NgayTao.Value.Month == month && a.NgayTao.Value.Year == year);


            return query.ToList();
        }

        [HttpGet("DonHangTaiQuayTheoThang/{month}")]
        public async Task<int> DonHangTaiQuayTheoThang(int month)
        {
            var query = repos.GetAll().
                        Where(a => a.NgayTao.Value.Month == month && a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.TaiQuay && a.NgayTao.Value.Year == DateTime.Now.Year);


            var result = query.ToList();

            var TongDon = result.Count();
            if (TongDon >= 0)
            {
                return TongDon;
            }
            else
                return 0;
        }
        [HttpGet("ThongKeBanHang")]
        public async Task<IActionResult> ThongKeBanHang()
        {
            var query = repos.GetAll().ToList();
            var currentDate = DateTime.Now;
            var startDate = currentDate.AddMonths(-6);
            query = query.Where(x => x.NgayTao >= startDate && x.NgayTao <= currentDate).ToList();
            var result = query.GroupBy(x => new { x.NgayTao.Value.Year, x.NgayTao.Value.Month })
              .Select(x => new
              {
                  Nam = x.Key.Year,
                  Thang = x.Key.Month,
                  Online = x.Count(y => y.TrangThaiGiaoHang != (int)TrangThaiGiaoHang.TaiQuay),
                  Ofline = x.Count(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.TaiQuay),
              }).OrderBy(x => x.Nam)
    .ThenBy(x => x.Thang)
            .Select(x => new
            {
                ThoiGian = $"{x.Thang.ToString()}/{x.Nam.ToString()}",
                Online = x.Online,
                Ofline = x.Ofline
            })



    .ToList();


            return Json(new { Data = result });

        }
        [HttpGet("DonDatHnagGanDay")]
        public async Task<List<HoaDon>> DonDatHnagGanDay()
        {
            var query = repos.GetAll().Where(x => x.TrangThaiGiaoHang != (int)TrangThaiGiaoHang.TaiQuay).OrderByDescending(x => x.NgayTao).Take(6).ToList();
            return query;
        }


        [HttpGet("BieuDoThongKeHoaDonOnlineTheoNgay")]
        public IActionResult BieuDoThongKeHoaDonOnlineTheoNgay(string? fromDate, string? toDate)
        {
            var query = from a in context.HoaDons

                        where a.NgayTao != null
                        select new ThongKeDoanhThuOnline
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            TongTien = (double)a.TongTien,
                            MaDonHang = a.MaHoaDon,
                            NgayNhan = a.NgayNhan,
                            TrangThaiGiaoHang = a.TrangThaiGiaoHang,
                            TrangThaiThanhToan = a.TrangThaiThanhToan,

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
              }).Select(x => new
              {
                  NgayTao = x.NgayTao,
                  TongSoDonHang = x.TongSoDonHang,
                  SoLuongDonHangThanhCong = x.SoLuongDonHangThanhCong,
                  SoLuongDonHangHuy = x.SoLuongDonHangHuy,
                  DoanhThu = x.DoanhThuOnline + x.DoanhThuOfline,
              }).ToList();


            return Json(new { Data = result });
        }
        [HttpGet]
        public ActionResult ThongTinThongKeHoaDonOnlineTheoNgay(string fromDate, string toDate)
        {
            if (!string.IsNullOrEmpty(fromDate))
            {

                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                baseQuery = baseQuery.Where(x => x.NgayTao.Date >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                baseQuery = baseQuery.Where(x => x.NgayTao.Date <= endDate);
            }

            if (string.IsNullOrEmpty(toDate) && string.IsNullOrEmpty(fromDate))
            {
                DateTime currentDate = DateTime.Now;
                DateTime startDate = currentDate.AddDays(-7);
                baseQuery = baseQuery.Where(x => x.NgayTao.Date >= startDate);
                baseQuery = baseQuery.Where(x => x.NgayTao.Date <= currentDate);
            }
            var result = baseQuery.ToList()
              .Select(x => new
              {
                  //MaDonHang = x.MaDonHang,
                  //GiaTriDon = ((x.TongTien ?? 0 ) - (x.TienGiam ?? 0)) ,
                  //TinhTrang = GetTinhTrang(x.TrangThaiGiaoHang ?? 10),
                  //TienShip = x.TienShip ?? 0,
                  //TenKhach = x.TenKhachHang ?? x.TenNguoiNhan ?? "",
                  //SDT = x.SoDt ?? x.SDTKhachHang ?? "",
                  //TrangThaiThanhToan = GetTrangThaiThanhToan(x.TrangThaiThanhToan ?? 10),
                  //Lidohuy = x.LiDoHuy ?? "",
                  //TinhThanh = GetTinhThanhFromDiaChi(x.DiaChi ?? ""),
                  //ThoiGian = DateTime.ParseExact($"{x.NgayTao.Hour}:{x.NgayTao.Minute}", "H:m", null).ToString("HH:mm"),
                  //Ngay = x.NgayTao.ToString(),
                  MaDonHang = x.MaDonHang,
                  GiaTriDon = ((double)(x.TongTien.GetValueOrDefault()) - (double)(x.TienGiam.GetValueOrDefault())),
                  TinhTrang = GetTinhTrang(x.TrangThaiGiaoHang ?? 10),
                  TienShip = x.TienShip ?? 0,
                  TenKhach = x.TenKhachHang ?? x.TenNguoiNhan ?? "Khách vãng lai",
                  SDT = x.SoDt ?? x.SDTKhachHang ?? "",
                  TrangThaiThanhToan = GetTrangThaiThanhToan(x.TrangThaiThanhToan ?? 10),
                  Lidohuy = x.LiDoHuy ?? "",
                  TinhThanh = GetTinhThanhFromDiaChi(x.DiaChi ?? "") ?? "",
                  ThoiGian = DateTime.ParseExact($"{x.NgayTao.Hour}:{x.NgayTao.Minute}", "H:m", null),
                  Ngay = x.NgayTao,


              }).OrderBy(x => x.Ngay)
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

        [HttpGet("DoanhThuTrong7ngay")]
        public IActionResult DoanhThuTrong7ngay()
        {
            var query = repos.GetAll().Where(a => (a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.TaiQuay || a.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao) && a.TrangThaiThanhToan == (int)TrangThaiHoaDon.DaThanhToan).ToList();
            var currentDate = DateTime.Now.Date;
            var startDate = currentDate.AddDays(-7).Date;
            query = (List<HoaDon>)query.Where(x => x.NgayTao.Value.Date >= startDate && x.NgayTao.Value.Date <= currentDate).ToList();

            var result = query.GroupBy(x => x.NgayTao.Value.Date)
                .Select(x => new
                {
                    Date = x.Key,
                    TongBan = x.Sum(y => y.TongTien),
                }).Select(x => new
                {
                    Date = x.Date,
                    DoanhThu = x.TongBan
                });

            return Json(new { Data = result });

        }


        [HttpGet("TrangThaiHoaDon1thang")]
        public IActionResult TrangThaiDonHang1thang()
        {
            var query = repos.GetAll().ToList();
            var ngayHienTai = DateTime.Now;
            //var thangtruocdo= DateTime.Now.AddMonths(-1);
            query = query.Where(x => x.NgayTao.Value.Month >= ngayHienTai.Month && x.NgayTao.Value.Year == ngayHienTai.Year).ToList();

            var result = query.GroupBy(x => x.NgayTao.Value.Month)
                .Select(x => new
                {

                    SoLuongHoaDonThanhCong = x.Count(y => (y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao || y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.TaiQuay) && y.TrangThaiThanhToan == (int)TrangThaiHoaDon.DaThanhToan),
                    SoluongHoaDonThatBai = x.Count(y => y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.DaHuy || y.TrangThaiThanhToan == (int)TrangThaiHoaDon.Huy || y.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.ChoHuy),
                    SoLuongHoaDon = x.Count()

                }).
                Select(x => new
                {
                    SoLuongHoaDonThanhCong = x.SoLuongHoaDonThanhCong,
                    SoluongHoaDonThatBai = x.SoluongHoaDonThatBai,
                    SoluongHoaDonCho = x.SoLuongHoaDon - x.SoLuongHoaDonThanhCong - x.SoluongHoaDonThatBai

                }).
                ToList();

            return Json(new { Data = result });

        }
        [HttpGet("DonHangTaiQuayGanDay")]
        public async Task<List<HoaDon>> DonHangTaiQuayGanDay()
        {
            var query = repos.GetAll().Where(x => x.TrangThaiGiaoHang==(int)TrangThaiGiaoHang.TaiQuay).OrderByDescending(x => x.NgayTao).Take(6).ToList();
            return query;
        }
    }
}
