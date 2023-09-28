using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Repositories
{
    public class SanPhamChiTietRespo : ISanPhamChiTietRespo
    {

        private readonly BazaizaiContext _context;
        private readonly IMapper _mapper;
        public SanPhamChiTietRespo(IMapper mapper)
        {
            _context = new BazaizaiContext();
            _mapper = mapper;
        }
        public async Task<bool> AddAsync(SanPhamChiTiet entity)
        {
            try
            {
                await _context.sanPhamChiTiets.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var entity = await GetByKeyAsync(id)!;
                if (entity == null) return false;
                _context.sanPhamChiTiets.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<SanPhamChiTiet?> GetByKeyAsync(string id)
        {
            return await _context.sanPhamChiTiets.FirstOrDefaultAsync(x => x.IdChiTietSp == id);
        }

        public async Task<IEnumerable<SanPhamChiTiet>> GetListAsync()
        {

            return await _context.sanPhamChiTiets.Include(c => c.SanPham).ToListAsync();
            return await _context.sanPhamChiTiets.Include(x => x.MauSac).ToListAsync();
        }

        public async Task<IEnumerable<SanPhamChiTietViewModel>> GetListViewModelAsync()
        {
            var sanPhamChiTiets = await _context.sanPhamChiTiets
                                     .Include(x => x.MauSac)
                                     .Include(x => x.ChatLieu)
                                     .Include(x => x.KichCo)
                                     .Include(x => x.KieuDeGiay)
                                     .Include(x => x.LoaiGiay)
                                     .Include(x => x.XuatXu)
                                     .Include(x => x.ThuongHieu)
                                     .Include(x => x.SanPham)
                                     .Include(x => x.Anh)
                                     .ToListAsync();
            var sanPhamChiTietViewModels = _mapper.Map<List<SanPhamChiTietViewModel>>(sanPhamChiTiets);
            return sanPhamChiTietViewModels;
        }

        public async Task<bool> UpdateAsync(SanPhamChiTiet entity)
        {
            try
            {
                _context.sanPhamChiTiets.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

    }
}
