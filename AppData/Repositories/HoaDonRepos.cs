using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.HoaDon;
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
        private readonly BazaizaiContext context;

        public HoaDonRepos()
        {
            context = new BazaizaiContext();
        }

        public HoaDon TaoHoaDonTaiQuay(HoaDon hoaDon)
        {
            hoaDon.MaHoaDon = MaHoaDonTuSinh();
            hoaDon.TrangThai = (int)TrangThaiGiaoHang.TaiQuay;
            hoaDon.TrangThaiThanhToan = (int)TrangThaiHoaDon.ChuaThanhToan;
            if (context.HoaDons.FirstOrDefault(c => c.MaHoaDon == hoaDon.MaHoaDon) == null)
            {
                context.HoaDons.Add(hoaDon);
                context.SaveChanges();
                return hoaDon;
            }
            return null;
        }
        public string MaHoaDonTuSinh()
        {
            if (context.HoaDons.Any())
            {
                return "HD" + (context.HoaDons.Count() + 1);
            }
            return "HD1";
        }
        public bool AddBill(HoaDon item)
        {
            try
            {
                context.HoaDons.Add(item);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<HoaDonChoDTO> GetAllHoaDonCho()
        {
            var listHoaDonCho = new List<HoaDonChoDTO>();
            var listHoaDon = context.HoaDons.Where(c => c.TrangThai == (int)TrangThaiGiaoHang.TaiQuay && c.TrangThaiThanhToan == (int)TrangThaiHoaDon.ChuaThanhToan).ToList();

            foreach (var item in listHoaDon)
            {
                var hoaDonCho = new HoaDonChoDTO()
                {
                    Id = item.IdHoaDon,
                    IdNguoiDung = item.IdNguoiDung,
                    MaHoaDon = item.MaHoaDon,
                    TrangThai = item.TrangThai,
                    TrangThaiThanhToan = item.TrangThaiThanhToan,
                    hoaDonChiTietDTOs = context.hoaDonChiTiets.Where(c => c.IdHoaDon == item.IdHoaDon).ToList(),
                };
                listHoaDonCho.Add(hoaDonCho);
            }
            return listHoaDonCho;
        }
    }
}
