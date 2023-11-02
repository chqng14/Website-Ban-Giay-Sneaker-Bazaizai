using App_Data.DbContextt;
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
                context.hoaDonChiTiets.Add(item);
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
                var id = context.hoaDonChiTiets.Find(item.IdHoaDonChiTiet);
                context.hoaDonChiTiets.Update(id);
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
            return context.hoaDonChiTiets.ToList();
        }

        public HoaDonChiTietViewModel GetHoaDonDTO(string idHoaDon)
        {
            var hoadon = context.hoaDonChiTiets
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
                var id = context.hoaDonChiTiets.Find(item.IdHoaDonChiTiet);
                context.hoaDonChiTiets.Remove(id);
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
                var hoadDonChiTietTrung = context.hoaDonChiTiets.FirstOrDefault(c=>c.IdSanPhamChiTiet == hoaDonChiTiet.IdSanPhamChiTiet && c.IdHoaDon == hoaDonChiTiet.IdHoaDon && c.TrangThai == (int)TrangThaiHoaDonChiTiet.ChoTaiQuay);
                if(hoadDonChiTietTrung == null)
                {
                    context.hoaDonChiTiets.Add(hoaDonChiTiet);
                    context.SaveChanges();
                    return hoaDonChiTiet;
                }
                else
                {
                    hoadDonChiTietTrung.SoLuong = hoadDonChiTietTrung.SoLuong+1;
                    context.hoaDonChiTiets.Update(hoadDonChiTietTrung);
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

            var hoaDonChiTiet = context.hoaDonChiTiets.FirstOrDefault(c => c.TrangThai == (int)TrangThaiHoaDonChiTiet.ChoTaiQuay && c.IdHoaDon == idHD && c.IdSanPhamChiTiet == idSanPham);
            int soLuongThayDoi = (int)SoLuongMoi - (int)hoaDonChiTiet.SoLuong;
            int intValue;
            if (int.TryParse(SoluongTon, out intValue))
            {
               
                if (soLuongThayDoi <= int.Parse(SoluongTon))
                {
                    if (SoLuongMoi != 0)
                    {
                        hoaDonChiTiet.SoLuong = SoLuongMoi;
                        context.hoaDonChiTiets.Update(hoaDonChiTiet);
                        context.SaveChanges();
                        return soLuongThayDoi.ToString();
                    }
                    else
                    {
                        context.hoaDonChiTiets.Remove(hoaDonChiTiet);
                        context.SaveChanges();
                        return soLuongThayDoi.ToString();
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if(SoLuongMoi < hoaDonChiTiet.SoLuong)
                {
                    if (SoLuongMoi != 0)
                    {
                        hoaDonChiTiet.SoLuong = SoLuongMoi;
                        context.hoaDonChiTiets.Update(hoaDonChiTiet);
                        context.SaveChanges();
                        return soLuongThayDoi.ToString();
                    }
                    else
                    {
                        context.hoaDonChiTiets.Remove(hoaDonChiTiet);
                        context.SaveChanges();
                        return soLuongThayDoi.ToString();
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public string XoaSanPhamKhoiHoaDon(string idHD, string idSanPham)
        {
            var hoaDonChiTiet = context.hoaDonChiTiets.FirstOrDefault(c => c.TrangThai == (int)TrangThaiHoaDonChiTiet.ChoTaiQuay && c.IdHoaDon == idHD && c.IdSanPhamChiTiet == idSanPham);
            var soLuongSanPham = hoaDonChiTiet.SoLuong;
            context.hoaDonChiTiets.Remove(hoaDonChiTiet);
            context.SaveChanges();
            return soLuongSanPham.ToString();
        }
    }
}
