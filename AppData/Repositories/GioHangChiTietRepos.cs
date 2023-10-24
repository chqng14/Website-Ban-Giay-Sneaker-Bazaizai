using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Repositories
{
    public class GioHangChiTietRepos : IGioHangChiTietRepos
    {
        BazaizaiContext context;
        private readonly IMapper _mapper;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        public GioHangChiTietRepos(IMapper mapper)
        {
            _mapper = mapper;
            context = new BazaizaiContext();
        }

        public bool AddCartDetail(GioHangChiTiet item)
        {
            try
            {
                context.gioHangChiTiets.Add(item);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EditCartDetail(GioHangChiTiet item)
        {
            try
            {
                var id = context.gioHangChiTiets.Find(item.IdGioHangChiTiet);
                context.gioHangChiTiets.Update(id);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<GioHangChiTiet> GetAll()
        {
            return context.gioHangChiTiets.Include(x => x.SanPhamChiTiet).ThenInclude(spct => spct.SanPham).ToList();
        }

        public IEnumerable<GioHangChiTietDTO> GetAllGioHangDTO()
        {
            var giohang = context.gioHangChiTiets
                .Include(x => x.SanPhamChiTiet).ThenInclude(spct => spct.SanPham)
                .Include(x => x.SanPhamChiTiet).ThenInclude(spct => spct.MauSac)
                .Include(x => x.SanPhamChiTiet).ThenInclude(spct => spct.KichCo)
                .Include(x => x.SanPhamChiTiet).ThenInclude(spct => spct.ThuongHieu)
                .Include(x => x.SanPhamChiTiet).ThenInclude(spct => spct.Anh)
                .ToList();
            return _mapper.Map<List<GioHangChiTietDTO>>(giohang);
        }

        public bool RemoveCartDetail(GioHangChiTiet item)
        {
            try
            {
                var id = context.gioHangChiTiets.Find(item.IdGioHangChiTiet);
                context.gioHangChiTiets.Remove(id);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
