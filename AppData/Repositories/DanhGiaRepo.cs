using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.DanhGia;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Repositories
{
    public class DanhGiaRepo:IDanhGiaRepo
    {
        private readonly BazaizaiContext _context;
        public DanhGiaRepo()
        {
            _context = new BazaizaiContext();
        }
        public async Task<bool> AddAsync(DanhGia danhGia)
        {
            try
            {
                await _context.danhGias.AddAsync(danhGia);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var entity = await GetByKeyAsync(id);
                if (entity == null) return false;
                _context.danhGias.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<DanhGia?> GetByKeyAsync(string id)
        {
            return await _context.danhGias.Where(x => x.IdDanhGia==id).FirstOrDefaultAsync();
        }

        public async Task<List<DanhGia>> GetListAsync(string productId, string parentId)
        {
            return await _context.danhGias.Where(x => x.ParentId == parentId && x.IdSanPhamChiTiet == productId).ToListAsync();
        }
    
        public async Task<List<DanhGia>> GetAllAsync()
        {
            return await _context.danhGias.ToListAsync();
        }

        public async Task<List<DanhGiaViewModel>> GetListAsyncViewModel(string productId, string parentId)
        {
            var ViewMode = await (from a in _context.danhGias
                                  join b in _context.NguoiDungs on a.IdNguoiDung equals b.Id
                                  where a.ParentId == parentId && a.IdSanPhamChiTiet == productId
                                  select new DanhGiaViewModel
                                  {
                                      IdDanhGia = a.IdDanhGia,
                                      ParentId = parentId,
                                      BinhLuan = a.BinhLuan,
                                      NgayDanhGia = a.NgayDanhGia,
                                      SaoSp = a.SaoSp,
                                      IdSanPhamChiTiet = a.IdSanPhamChiTiet,
                                      IdNguoiDung = a.IdNguoiDung,
                                      TrangThai = a.TrangThai,
                                      Name = b.TenNguoiDung
                                  }).OrderByDescending(x => x.NgayDanhGia).ToListAsync();
            return ViewMode;


        }
        public async Task<bool> UpdateAsync(DanhGia danhGia)
        {
            try
            {
                _context.danhGias.Update(danhGia);
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
