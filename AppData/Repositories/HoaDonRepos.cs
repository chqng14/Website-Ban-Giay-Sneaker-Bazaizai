using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static App_Data.Repositories.TrangThai;

namespace App_Data.Repositories
{
    public class HoaDonRepos : IHoaDonRepos
    {
        BazaizaiContext context;

        public HoaDonRepos()
        {
            context = new BazaizaiContext();
        }

        public bool TaoHoaDonTaiQuay(string id)
        {
            var hoaDon = new HoaDon();  
            hoaDon.IdHoaDon  = Guid.NewGuid().ToString();
            hoaDon.IdNguoiDung = id;
            hoaDon.MaHoaDon = MaHoaDonTuSinh();
            hoaDon.TrangThaiThanhToan = (int)TrangThaiHoaDon.ChuaThanhToan;
            hoaDon.TrangThai = (int)TrangThaiGiaHang.TaiQuay;
            hoaDon.NgayTao = DateTime.Now;
            context.HoaDons.Add(hoaDon);
            context.SaveChanges();
            return true;
        }
        public string MaHoaDonTuSinh()
        {
            if (context.HoaDons.Any())
            {
                return "HD" + (context.HoaDons.Count()+1);
            }
            return "HD1";
        }
    }
}
