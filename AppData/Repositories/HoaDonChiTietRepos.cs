using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDonChiTietDTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App_Data.Repositories.TrangThai;

namespace App_Data.Repositories
{
    public class HoaDonChiTietRepos : IHoaDonChiTietRepos
    {
        BazaizaiContext context;
        private readonly IMapper _mapper;
        public HoaDonChiTietRepos(IMapper mapper)
        {
            context = new BazaizaiContext();
            _mapper = mapper;
        }
        public bool AddBillDetail(HoaDonChiTiet item)
        {
            try
            {
                context.HoaDonChiTiets.Add(item);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EditBillDetail(HoaDonChiTiet item)
        {
            try
            {
                var id = context.HoaDonChiTiets.Find(item.IdHoaDonChiTiet);
                context.HoaDonChiTiets.Update(id);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<HoaDonChiTiet> GetAll()
        {
            return context.HoaDonChiTiets.ToList();
        }

        public HoaDonChiTietViewModel GetHoaDonDTO(string idHoaDon)
        {
            var hoadon = context.HoaDonChiTiets
                .Include(x => x.SanPhamChiTiet).ThenInclude(spct => spct.SanPham)
                .Include(x => x.SanPhamChiTiet).ThenInclude(spct => spct.MauSac)
                .Include(x => x.SanPhamChiTiet).ThenInclude(spct => spct.KichCo)
                .Include(x => x.HoaDon).ThenInclude(hd => hd.ThongTinGiaoHang)
                .Include(x => x.HoaDon).ThenInclude(hd => hd.Voucher)
                .FirstOrDefault(x => x.IdHoaDon == idHoaDon);
            return _mapper.Map<HoaDonChiTietViewModel>(hoadon);
        }

        public bool RemoveBillDetail(HoaDonChiTiet item)
        {
            try
            {
                var id = context.HoaDonChiTiets.Find(item.IdHoaDonChiTiet);
                context.HoaDonChiTiets.Remove(id);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public HoaDonChiTiet ThemSanPhamVaoHoaDon(HoaDonChiTiet hoaDonChiTiet)
        {
            try
            {
                var hoadDonChiTietTrung = context.HoaDonChiTiets.FirstOrDefault(c => c.IdSanPhamChiTiet == hoaDonChiTiet.IdSanPhamChiTiet && c.IdHoaDon == hoaDonChiTiet.IdHoaDon && c.TrangThai == (int)TrangThaiHoaDonChiTiet.ChoTaiQuay);
                if (hoadDonChiTietTrung == null)
                {
                    context.HoaDonChiTiets.Add(hoaDonChiTiet);
                    context.SaveChanges();
                    return hoaDonChiTiet;
                }
                else
                {
                    hoadDonChiTietTrung.SoLuong = hoadDonChiTietTrung.SoLuong + 1;
                    context.HoaDonChiTiets.Update(hoadDonChiTietTrung);
                    context.SaveChanges();
                    return hoadDonChiTietTrung;

                }

            }
            catch (Exception)
            {

                return null;
            }
        }

        public string UpdateSoLuong(string idHD, string idSanPham, int SoLuongMoi, string SoluongTon)
        {

            var hoaDonChiTiet = context.HoaDonChiTiets.FirstOrDefault(c => c.TrangThai == (int)TrangThaiHoaDonChiTiet.ChoTaiQuay && c.IdHoaDon == idHD && c.IdSanPhamChiTiet == idSanPham);
            int soLuongThayDoi = (int)SoLuongMoi - (int)hoaDonChiTiet.SoLuong;
            if (soLuongThayDoi <= int.Parse(SoluongTon))
            {
                if (SoLuongMoi != 0)
                {
                    hoaDonChiTiet.SoLuong = SoLuongMoi;
                    context.HoaDonChiTiets.Update(hoaDonChiTiet);
                    context.SaveChanges();
                    return soLuongThayDoi.ToString();
                }
                else
                {
                    context.HoaDonChiTiets.Remove(hoaDonChiTiet);
                    context.SaveChanges();
                    return soLuongThayDoi.ToString();
                }
            }
            else
            {
                return null;
            }
        }

        public string XoaSanPhamKhoiHoaDon(string idHD, string idSanPham)
        {
            var hoaDonChiTiet = context.HoaDonChiTiets.FirstOrDefault(c => c.TrangThai == (int)TrangThaiHoaDonChiTiet.ChoTaiQuay && c.IdHoaDon == idHD && c.IdSanPhamChiTiet == idSanPham);
            var soLuongSanPham = hoaDonChiTiet.SoLuong;
            context.HoaDonChiTiets.Remove(hoaDonChiTiet);
            context.SaveChanges();
            return soLuongSanPham.ToString();
        }
        public List<HoaDonChiTiet> GetAllHoaDonOnline()
        {
            throw new NotImplementedException();
        }

        public List<HoaDonChiTiet> HuyHoaDonChiTiet(string idHD)
        {
            try
            {
                var hoaDonChiTiet = context.HoaDonChiTiets.Where(c => c.TrangThai == (int)TrangThaiHoaDonChiTiet.ChoTaiQuay && c.IdHoaDon == idHD).ToList();
                if (hoaDonChiTiet.Any())
                {
                    foreach (var item in hoaDonChiTiet)
                    {
                        item.TrangThai = (int)TrangThaiHoaDonChiTiet.Huy;
                    }
                    context.UpdateRange(hoaDonChiTiet);
                    context.SaveChanges();
                    return hoaDonChiTiet;
                }
                return null;
               
            }
            catch (Exception)
            {

                return null;
            }
            
        }

        public bool ThanhToanHoaDonChiTiet(string idHD)
        {
            try
            {
                var hoaDonChiTiet = context.HoaDonChiTiets.Where(c => c.TrangThai == (int)TrangThaiHoaDonChiTiet.ChoTaiQuay && c.IdHoaDon == idHD).ToList();
                if (hoaDonChiTiet.Any())
                {
                    foreach (var item in hoaDonChiTiet)
                    {
                        item.TrangThai = (int)TrangThaiHoaDonChiTiet.DaThanhToan;
                    }
                    context.UpdateRange(hoaDonChiTiet);
                    context.SaveChanges();
                    return true;
                }
                return false;

            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
