﻿using App_Data.DbContextt;
using App_Data.Repositories;
using App_Data.ViewModels.DanhGia;
using App_Data.ViewModels.ThongKe;
using Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using static App_Data.Repositories.TrangThai;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ThongKeController : Controller
    {
        private BazaizaiContext db = new BazaizaiContext();
        public IActionResult Index()
        {
            return View();
        }
        

        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from a in db.HoaDons
                        join b in db.hoaDonChiTiets on a.IdHoaDon equals b.IdHoaDon
                        join c in db.sanPhamChiTiets on b.IdSanPhamChiTiet equals c.IdChiTietSp
                        where b.TrangThai == (int)TrangThaiGiaoHang.TaiQuay ||b.TrangThai==(int)TrangThaiGiaoHang.DangGiao&&a.NgayTao!=null
                        select new ThongKeViewModel
                        {
                            NgayTao = (DateTime)a.NgayTao,
                            SoLuong =(int) b.SoLuong,
                            GiaGoc = (double)c.GiaNhap,
                            GiaBan = (double)b.GiaBan,
                        };
           
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.NgayTao >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.NgayTao < endDate);
            }

            var result = query.GroupBy(x => x.NgayTao.Date).Select(x => new
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

    }
}
