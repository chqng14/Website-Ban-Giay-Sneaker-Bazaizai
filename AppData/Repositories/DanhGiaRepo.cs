using App_Data.DbContext;
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
                await _context.DanhGias.AddAsync(danhGia);
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
                _context.DanhGias.Remove(entity);
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
            return await _context.DanhGias.Where(x => x.IdDanhGia == id).FirstOrDefaultAsync();
        }

        //public async Task<List<DanhGia>> GetListAsync(string productId, string parentId)
        //{
        //    return await _context.DanhGias.Where(x => x.ParentId == parentId && x.IdSanPhamChiTiet == productId).ToListAsync();
        //}

        public async Task<List<DanhGia>> GetAllAsync()
        {
            return await _context.DanhGias.ToListAsync();
        }
        public async Task<float> SoSaoTB(string IdProductChiTiet)
        {
            var DanhGias = await GetListAsyncViewModel(IdProductChiTiet);
            var lstdanhGia = DanhGias.Where(x => x.SaoSp != 0 && x.TrangThai == (int)TrangThaiDanhGia.DaDuyet);
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
                float a = 0;
                a = ((float)Tong / SoSao);
                a = (float)Math.Round(a, 1);
                return a;
            }

        }
        public async Task<int> GetTongSoDanhGia(string IdProductChiTiet)
        {
            var lstdanhGia = await GetListAsyncViewModel(IdProductChiTiet);
            return lstdanhGia.Where(x => x.SaoSp != 0&&x.TrangThai==(int)TrangThaiDanhGia.DaDuyet).Count();
        }
        public async Task<int> GetTongSoDanhGiaChuaDuyetCuaMotSp(string IdProductChiTiet)
        {
            var lstdanhGia = await GetListAsyncViewModel(IdProductChiTiet);
            return lstdanhGia.Where(x => x.SaoSp != 0 && x.TrangThai == (int)TrangThaiDanhGia.ChuaDuyet).Count();
        }
        //var lstdanhGia =await _context.DanhGias.ToListAsync();
        //return lstdanhGia.Where(x => x.SaoSp != 0 && x.TrangThai == (int)TrangThaiDanhGia.ChuaDuyet).Count();
        //var IDsanPham = _context.SanPhamChiTiets
        //   .Where(x => x.IdChiTietSp == IdProductChiTiet)
        //   .Select(x => x.IdSanPham)
        //   .FirstOrDefault();
        //public async Task<int> GetTongSoDanhGiaChuaDuyet()
        //{
        //    var ViewMode = await (from a in _context.DanhGias                                
        //                          join c in _context.SanPhamChiTiets on a.IdSanPhamChiTiet equals c.IdChiTietSp                       
        //                          join j in _context.SanPhams on c.IdSanPham equals j.IdSanPham
        //                          where  a.TrangThai == (int)TrangThaiDanhGia.ChuaDuyet // Lọc đánh giá theo IdSanPham
        //                          select new DanhGiaViewModel
        //                          {                                                                        
        //                              TrangThai = a.TrangThai,
        //                              TenSanPham = j.TenSanPham,
        //                          })
        //       .GroupBy(x => x.TenSanPham)  // Nhóm theo tên sản phẩm
        //            .Select(g => new DanhGiaViewModel
        //            {
        //                TrangThai = g.First().TrangThai, // Chọn một trạng thái bất kỳ từ nhóm
        //                TenSanPham = g.Key,  // Key chính là tên sản phẩm
        //            })
        //            .ToListAsync();
        //    return ViewMode;
        //}
        public async Task<List<Tuple<string, int, string, string>>> TongSoDanhGiaCuaMoiSpChuaDuyet()
        {
            var result = await (from a in _context.DanhGias
                                join c in _context.SanPhamChiTiets on a.IdSanPhamChiTiet equals c.IdChiTietSp
                                join j in _context.SanPhams on c.IdSanPham equals j.IdSanPham
                                where a.TrangThai == (int)TrangThaiDanhGia.ChuaDuyet // Lọc đánh giá theo IdSanPham
                                select new DanhGiaViewModel
                                {
                                    TenSanPham = j.TenSanPham,
                                    IdSanPham = c.IdSanPham,
                                    IdSanPhamChiTiet = c.IdChiTietSp
                                })
                            .GroupBy(x => x.TenSanPham)
                              .Select(g => new Tuple<string, int, string, string>(g.Key, g.Count(), g.First().IdSanPham, g.First().IdSanPhamChiTiet))
                            //.Select(g => new Tuple<string, int>(g.Key, g.Count())) // Tạo tuple chứa tên sản phẩm và số lượng đánh giá
                            .ToListAsync();
            return result;
        }
        public async Task<List<DanhGiaViewModel>> LstChiTietDanhGiaCuaMoiSpChuaDuyet(string idSanPham)
        {
            var ViewMode = await (from a in _context.DanhGias
                                  join b in _context.NguoiDungs on a.IdNguoiDung equals b.Id
                                  join c in _context.SanPhamChiTiets on a.IdSanPhamChiTiet equals c.IdChiTietSp
                                  join d in _context.MauSacs on c.IdMauSac equals d.IdMauSac
                                  join e in _context.KichCos on c.IdKichCo equals e.IdKichCo
                                  join j in _context.SanPhams on c.IdSanPham equals j.IdSanPham
                                  where c.IdSanPham == idSanPham && a.TrangThai == (int)TrangThaiDanhGia.ChuaDuyet
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
                                      SanPhamTongQuat = d.TenMauSac + "," + e.SoKichCo,
                                      TenSanPham = j.TenSanPham,
                                      IdSanPham = c.IdSanPham
                                  })
                .OrderByDescending(x => x.NgayDanhGia)
                .ToListAsync();

            return ViewMode;
        }
        public async Task<List<DanhGiaViewModel>> GetListAsyncViewModel(string IdProductChiTiet)
        {
            var IDsanPham = _context.SanPhamChiTiets
                .Where(x => x.IdChiTietSp == IdProductChiTiet)
                .Select(x => x.IdSanPham)
                .FirstOrDefault();

            var ViewMode = await (from a in _context.DanhGias
                                  join b in _context.NguoiDungs on a.IdNguoiDung equals b.Id
                                  join c in _context.SanPhamChiTiets on a.IdSanPhamChiTiet equals c.IdChiTietSp
                                  join d in _context.MauSacs on c.IdMauSac equals d.IdMauSac
                                  join e in _context.KichCos on c.IdKichCo equals e.IdKichCo
                                  join j in _context.SanPhams on c.IdSanPham equals j.IdSanPham
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
                                      TenNguoiDung = b.UserName,
                                      AnhDaiDien = b.AnhDaiDien,
                                      SaoVanChuyen = a.SaoVanChuyen,
                                      SanPhamTongQuat = d.TenMauSac + "," + e.SoKichCo,
                                      TenSanPham = j.TenSanPham,
                                      MoTa = a.MoTa,
                                      ChatLuongSanPham = a.ChatLuongSanPham
                                  })
                .OrderByDescending(x => x.NgayDanhGia)
                .ToListAsync();

            return ViewMode;
        }

        public async Task<bool> UpdateAsync(DanhGia danhGia)
        {
            try
            {
                _context.DanhGias.Update(danhGia);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public async Task<DanhGia?> FindbyId(string id)
        {
            return await _context.DanhGias.Where(x => x.IdDanhGia == id).FirstOrDefaultAsync();
        }

        public async Task<DanhGiaViewModel?> GetViewModelByKeyAsync(string id)
        {
            var ViewMode = await (from a in _context.DanhGias
                                  join b in _context.NguoiDungs on a.IdNguoiDung equals b.Id
                                  join c in _context.SanPhamChiTiets on a.IdSanPhamChiTiet equals c.IdChiTietSp
                                  join d in _context.MauSacs on c.IdMauSac equals d.IdMauSac
                                  join e in _context.KichCos on c.IdKichCo equals e.IdKichCo
                                  join j in _context.SanPhams on c.IdSanPham equals j.IdSanPham
                                  select new DanhGiaViewModel
                                  {
                                      IdDanhGia = a.IdDanhGia,
                                      BinhLuan = a.BinhLuan,
                                      NgayDanhGia = a.NgayDanhGia,
                                      SaoSp = a.SaoSp,
                                      TenNguoiDung = b.UserName,
                                      AnhDaiDien = b.AnhDaiDien,
                                      MoTa = a.MoTa,
                                      ChatLuongSanPham = a.ChatLuongSanPham,
                                      IdSanPhamChiTiet = a.IdSanPhamChiTiet,

                                  })

                .ToListAsync();
            var danhgia = ViewMode.FirstOrDefault(x => x.IdDanhGia == id);
            return danhgia;
        }
        public async Task<List<DanhGiaViewModel>> LstDanhGia()
        {
            var ViewMode = await (from a in _context.DanhGias
                                  join b in _context.NguoiDungs on a.IdNguoiDung equals b.Id
                                  join c in _context.SanPhamChiTiets on a.IdSanPhamChiTiet equals c.IdChiTietSp
                                  join d in _context.MauSacs on c.IdMauSac equals d.IdMauSac
                                  join e in _context.KichCos on c.IdKichCo equals e.IdKichCo
                                  join j in _context.SanPhams on c.IdSanPham equals j.IdSanPham
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
                                      TenNguoiDung = b.UserName,
                                      AnhDaiDien = b.AnhDaiDien,
                                      SaoVanChuyen = a.SaoVanChuyen,
                                      SanPhamTongQuat = d.TenMauSac + "," + e.SoKichCo,
                                      TenSanPham = j.TenSanPham,
                                      IdSanPham = c.IdSanPham,
                                      MoTa=a.MoTa,
                                      ChatLuongSanPham=a.ChatLuongSanPham,
                                      anhSp = _context.Anh
                                      .Where(an => an.IdSanPhamChiTiet == c.IdChiTietSp)
                                      .OrderBy(an => an.IdAnh)
                                      .Select(an => an.Url)
                                      .FirstOrDefault(),

                                  })
                .OrderByDescending(x => x.NgayDanhGia)
                .ToListAsync();

            return ViewMode;
        }
      
    }
}
