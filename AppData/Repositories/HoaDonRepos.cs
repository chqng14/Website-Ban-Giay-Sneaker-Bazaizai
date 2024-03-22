using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.HoaDonChiTietDTO;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        public HoaDonRepos(IMapper mapper)
        {
            context = new BazaizaiContext();
            //_mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<HoaDon, HoaDonDTO>();
            //    cfg.CreateMap<HoaDonDTO, HoaDon>();
            //    cfg.CreateMap<HoaDonChiTiet, HoaDonChiTietTaiQuay>();
            //}).CreateMapper();
            _mapper = mapper;

        }

        public HoaDon TaoHoaDonTaiQuay(HoaDon hoaDon)
        {
            CancellationToken cancellationTokena = new CancellationToken();
            hoaDon.MaHoaDon = MaHoaDonTuSinh();
            hoaDon.TrangThaiGiaoHang = (int)TrangThaiGiaoHang.TaiQuay;
            hoaDon.TrangThaiThanhToan = (int)TrangThaiHoaDon.ChuaThanhToan;
            hoaDon.NgayTao = DateTime.Now;
            if (context.HoaDons.FirstOrDefault(c => c.MaHoaDon == hoaDon.MaHoaDon) == null)
            {
                context.HoaDons.Add(hoaDon);
                context.SaveChangesAsync(cancellationTokena);
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
                item.MaHoaDon = MaHoaDonTuSinh();
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
            var listHoaDon = context.HoaDons.Where(c => c.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.TaiQuay && c.TrangThaiThanhToan == (int)TrangThaiHoaDon.ChuaThanhToan).AsNoTracking().ToList();
            var listHoaDonChiTiet = context.HoaDonChiTiets.Where(c => c.TrangThai == (int)TrangThaiHoaDonChiTiet.ChoTaiQuay).AsNoTracking().ToList();
            foreach (var item in listHoaDon)
            {
                var hoaDonCho = new HoaDonChoDTO()
                {
                    Id = item.IdHoaDon,
                    IdNguoiDung = item.IdNguoiDung,
                    MaHoaDon = item.MaHoaDon,
                    NgayTao = item.NgayTao,
                    TrangThaiGiaoHang = item.TrangThaiGiaoHang,
                    TrangThaiThanhToan = item.TrangThaiThanhToan,
                    hoaDonChiTietDTOs = new List<HoaDonChiTietTaiQuay>()
                };
                foreach (var item2 in listHoaDonChiTiet.Where(c => c.IdHoaDon == item.IdHoaDon).ToList())
                {
                    hoaDonCho.hoaDonChiTietDTOs.Add(_mapper.Map<HoaDonChiTietTaiQuay>(item2));
                }
                listHoaDonCho.Add(hoaDonCho);
            }
            return listHoaDonCho;
        }

        public List<HoaDonViewModel> GetHoaDon()
        {
            var hoadon = context.HoaDons.Include(c => c.ThongTinGiaoHang).ToList();
            return _mapper.Map<List<HoaDonViewModel>>(hoadon);
        }

        public bool EditBill(HoaDon item)
        {
            try
            {
                var id = context.HoaDons.Find(item.IdHoaDon);
                context.HoaDons.Update(id);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<HoaDon> GetHoaDonUpdate()
        {
            var hoadon = context.HoaDons.ToList();
            return hoadon;
        }

        public string HuyHoaDon(string maHD, string lyDoHuy, string idUser)
        {
            try
            {
                var hoaDon = context.HoaDons.FirstOrDefault(c => c.MaHoaDon == maHD);
                hoaDon.NgayThanhToan = DateTime.Now;
                hoaDon.TrangThaiThanhToan = (int)TrangThaiHoaDon.Huy;
                hoaDon.LiDoHuy = lyDoHuy;
                hoaDon.IdNguoiSuaGanNhat = idUser;
                if (hoaDon != null)
                {
                    context.HoaDons.Update(hoaDon);
                    context.SaveChanges();
                    return hoaDon.IdHoaDon;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
          
        }

        public bool ThanhToanTaiQuay(HoaDon item)
        {
            try
            {
                context.Update(item);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckKhachHang(string idThongTin)
        {
            var ThongTin = context.ThongTinGiaoHangs.FirstOrDefault(c=>c.IdThongTinGH == idThongTin);
            if (ThongTin == null||ThongTin.IdNguoiDung!=null)
            {
                return false;
            }
            return true;
        }
    }
}
