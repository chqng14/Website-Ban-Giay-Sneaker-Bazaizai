using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.DanhGia;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App_Data.Repositories.TrangThai;

namespace App_Data.Repositories
{
    public class DanhGiaRepo : IDanhGiaRepo
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
            return await _context.danhGias.Where(x => x.IdDanhGia == id).FirstOrDefaultAsync();
        }

        //public async Task<List<DanhGia>> GetListAsync(string productId, string parentId)
        //{
        //    return await _context.danhGias.Where(x => x.ParentId == parentId && x.IdSanPhamChiTiet == productId).ToListAsync();
        //}

        public async Task<List<DanhGia>> GetAllAsync()
        {
            return await _context.danhGias.ToListAsync();
        }
        public async Task<float> SoSaoTB(string IdProductChiTiet)
        {
            var danhGias = await GetListAsyncViewModel(IdProductChiTiet);
            var lstdanhGia = danhGias.Where(x => x.SaoSp != 0);
            var SoSao = lstdanhGia.Count();
            int? Tong = 0;
            if (SoSao == 0)
            {
                return 0;

            }
            else
            {
                foreach (var item in lstdanhGia)
                {
                    Tong += item.SaoSp;
                }
                return (float)(Tong / SoSao);
            }

        }
        public async Task<int> GetTongSoDanhGia(string IdProductChiTiet)
        {
            var lstdanhGia = await GetListAsyncViewModel(IdProductChiTiet);
            return lstdanhGia.Where(x => x.SaoSp != 0).Count();
        }
        public async Task<List<DanhGiaViewModel>> GetListAsyncViewModel(string IdProductChiTiet)
        {
            var IDsanPham = _context.sanPhamChiTiets
                .Where(x => x.IdChiTietSp == IdProductChiTiet)
                .Select(x => x.IdSanPham)
                .FirstOrDefault();

            var ViewMode = await (from a in _context.danhGias
                                  join b in _context.NguoiDungs on a.IdNguoiDung equals b.Id
                                  join c in _context.sanPhamChiTiets on a.IdSanPhamChiTiet equals c.IdChiTietSp
                                  join d in _context.mauSacs on c.IdMauSac equals d.IdMauSac
                                  join e in _context.kichCos on c.IdKichCo equals e.IdKichCo
                                  where c.IdSanPham == IDsanPham && a.TrangThai == (int)TrangThaiDanhGia.DaDuyet // Lọc đánh giá theo IdSanPham
                                  select new DanhGiaViewModel
                                  {
                                      IdDanhGia = a.IdDanhGia,
                                      ParentId = a.ParentId,
                                      BinhLuan = a.BinhLuan,
                                      NgayDanhGia = a.NgayDanhGia,
                                      SaoSp = a.SaoSp,
                                      IdSanPhamChiTiet = a.IdSanPhamChiTiet,
                                      IdNguoiDung = a.IdNguoiDung,
                                      TrangThai = a.TrangThai,
                                      TenNguoiDung = b.TenNguoiDung,
                                      AnhDaiDien = b.AnhDaiDien,
                                      SaoVanChuyen = a.SaoVanChuyen,
                                      SanPhamTongQuat = "Phân loại hàng: " + d.TenMauSac + "," + e.SoKichCo,


                                  })
                .OrderByDescending(x => x.NgayDanhGia)
                .ToListAsync();

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
