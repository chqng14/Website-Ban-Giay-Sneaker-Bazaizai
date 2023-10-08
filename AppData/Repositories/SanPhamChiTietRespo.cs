using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App_Data.Repositories.TrangThai;

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

        public async Task<List<SanPhamChiTietDTO>> GetListSanPhamChiTietDTOAsync(List<string> lstGuid)
        {
            var lstSanPhamChiTiet = (await _context.sanPhamChiTiets
                 .Include(x => x.Anh)
                 .Include(x => x.SanPham)
                 .Include(x => x.MauSac)
                 .Include(x => x.KichCo)
                 .Include(x => x.ThuongHieu)
                 .ToListAsync()).Where(sp => lstGuid.Contains(sp.IdChiTietSp!)).ToList();
            return _mapper.Map<List<SanPhamChiTietDTO>>(lstSanPhamChiTiet);
        }

        public async Task<DanhSachGiayViewModel> GetDanhSachGiayViewModelAsync()
        {
            var lstSanPhamChiTiet = await _context.sanPhamChiTiets
                .Include(it => it.SanPham)
                .Include(it => it.ThuongHieu)
                .Include(it => it.Anh)
                .ToListAsync();
            return _mapper.Map<DanhSachGiayViewModel>(lstSanPhamChiTiet);
        }

        public async Task<IEnumerable<SanPhamChiTiet>> GetListAsync()
        {
            return await _context.sanPhamChiTiets.ToListAsync();
        }


        public SanPhamDanhSachViewModel CreateSanPhamDanhSachViewModel(SanPhamChiTiet sanPham)
        {
            return new SanPhamDanhSachViewModel()
            {
                IdChiTietSp = sanPham.IdChiTietSp,
                ChatLieu = _context.ChatLieus.ToList().FirstOrDefault(x => x.IdChatLieu == sanPham.IdChatLieu)?.TenChatLieu,
                SanPham = _context.thuongHieus.ToList().FirstOrDefault(x => x.IdThuongHieu == sanPham.IdThuongHieu)?.TenThuongHieu + " " + _context.SanPhams.ToList().FirstOrDefault(x => x.IdSanPham == sanPham.IdSanPham)?.TenSanPham,
                GiaBan = sanPham.GiaBan,
                GiaNhap = sanPham.GiaNhap,
                KichCo = _context.kichCos.ToList().FirstOrDefault(x => x.IdKichCo == sanPham.IdKichCo)?.SoKichCo,
                Anh = _context.Anh.ToList().Where(x => x.IdSanPhamChiTiet == sanPham.IdChiTietSp && x.TrangThai == 0).FirstOrDefault()?.Url,
                KieuDeGiay = _context.kieuDeGiays.ToList().FirstOrDefault(x => x.IdKieuDeGiay == sanPham.IdKieuDeGiay)?.TenKieuDeGiay,
                LoaiGiay = _context.LoaiGiays.ToList().FirstOrDefault(x => x.IdLoaiGiay == sanPham.IdLoaiGiay)?.TenLoaiGiay,
                SoLuongTon = sanPham.SoLuongTon
            };
        }

        public async Task<IEnumerable<SanPhamDanhSachViewModel>> GetListViewModelAsync()
        {
            var sanPhamChiTietViewModels = (await _context.sanPhamChiTiets.ToListAsync()).Where(it => it.TrangThai == 0).OrderByDescending(x=>x.NgayTao).Select(item => CreateSanPhamDanhSachViewModel(item)).ToList();
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

        public async Task<List<ItemShopViewModel>> GetDanhSachItemShopViewModelAsync()
        {
            var listSanPham = (await _context.sanPhamChiTiets
                .Include(it => it.SanPham)
                .Include(it => it.ThuongHieu)
                .Include(it => it.Anh)
                .ToArrayAsync()).Where(sp => sp.TrangThai == 0).ToList().GroupBy(
              gr => new
              {
                  gr.IdChatLieu,
                  gr.IdSanPham,
                  gr.IdLoaiGiay,
                  gr.IdKieuDeGiay,
                  gr.IdThuongHieu,
                  gr.IdXuatXu,
              }).Select(gr => gr.First())
              .OrderByDescending(sp => sp.Ma)
              .ToList();
            var itemShops = _mapper.Map<List<ItemShopViewModel>>(listSanPham);
            return itemShops;
        }

        public async Task<ItemDetailViewModel?> GetItemDetailViewModelAynsc(string id)
        {
            var sanPhamChiTiet = (await _context.sanPhamChiTiets.
                Include(x => x.Anh).
                Include(x => x.SanPham).
                Include(x => x.ThuongHieu).
                Include(x => x.KichCo).
                Include(x => x.MauSac).
                ToListAsync()).FirstOrDefault(sp => sp.IdChiTietSp == id);
            if (sanPhamChiTiet == null) return null;
            var itemDetailViewModel = _mapper.Map<ItemDetailViewModel>(sanPhamChiTiet);
            var lstBienThe = (await _context.sanPhamChiTiets
                .Include(x => x.MauSac)
                .Include(x => x.KichCo)
                .ToListAsync())
                .Where(sp =>
                sp.TrangThai == 0 &&
                sp.IdKieuDeGiay == sanPhamChiTiet!.IdKieuDeGiay &&
                sp.IdLoaiGiay == sanPhamChiTiet.IdLoaiGiay &&
                sp.IdThuongHieu == sanPhamChiTiet.IdThuongHieu &&
                sp.IdXuatXu == sanPhamChiTiet.IdXuatXu &&
                sp.IdChatLieu == sanPhamChiTiet.IdChatLieu &&
                sp.IdSanPham == sanPhamChiTiet.IdSanPham
                ).ToList();
            itemDetailViewModel.LstMauSac = lstBienThe.Select(x => x.MauSac.TenMauSac).Distinct().ToList()!;
            itemDetailViewModel.LstKichThuoc = lstBienThe.Where(sp => sp.IdMauSac == sanPhamChiTiet.IdMauSac).Select(x => x.KichCo.SoKichCo!.Value).Distinct().OrderBy(item => item).ToList();
            return itemDetailViewModel;
        }

        public async Task<ItemDetailViewModel?> GetItemDetailViewModelWhenSelectColorAynsc(string id, string mauSac)
        {
            var sanPhamGet = (await _context.sanPhamChiTiets.ToListAsync())
                .FirstOrDefault(sp => sp.IdChiTietSp == id);
            var idMauSac = (await _context.mauSacs.ToListAsync()).FirstOrDefault(x => x.TenMauSac == mauSac)!.IdMauSac;
            if (sanPhamGet == null) return null;
            var lstBienThe = (await _context.sanPhamChiTiets
                .Include(x => x.MauSac)
                .Include(x => x.KichCo)
                .Include(x => x.Anh)
                .ToListAsync())
                .Where(sp =>
                sp.TrangThai == 0 &&
                sp.IdKieuDeGiay == sanPhamGet!.IdKieuDeGiay &&
                sp.IdLoaiGiay == sanPhamGet.IdLoaiGiay &&
                sp.IdThuongHieu == sanPhamGet.IdThuongHieu &&
                sp.IdXuatXu == sanPhamGet.IdXuatXu &&
                sp.IdChatLieu == sanPhamGet.IdChatLieu &&
                sp.IdSanPham == sanPhamGet.IdSanPham &&
                sp.IdMauSac == idMauSac
                ).ToList();
            var lstSize = lstBienThe.DistinctBy(sp => sp.KichCo.SoKichCo).OrderBy(item => item.KichCo.SoKichCo).ToList();
            var idSizeGet = lstSize.FirstOrDefault()!.KichCo.IdKichCo;
            var sanPhamChiTiet = lstBienThe.FirstOrDefault(sp => sp.IdKichCo == idSizeGet);
            var itemDetailViewModel = _mapper.Map<ItemDetailViewModel>(sanPhamChiTiet);
            itemDetailViewModel.LstKichThuoc = lstBienThe.Select(x => x.KichCo.SoKichCo!.Value).Distinct().OrderBy(item => item).ToList();
            return itemDetailViewModel;
        }

        public async Task<ItemDetailViewModel?> GetItemDetailViewModelWhenSelectSizeAynsc(string id, int size)
        {
            var sanPhamGet = (await _context.sanPhamChiTiets.ToListAsync())
                .FirstOrDefault(sp => sp.IdChiTietSp == id);
            var idsSize = (await _context.kichCos.ToListAsync()).FirstOrDefault(x => x.SoKichCo == size)!.IdKichCo;
            var sanPhamChiTiet = (await _context.sanPhamChiTiets.
                Include(x => x.Anh).
                Include(x => x.SanPham).
                Include(x => x.ThuongHieu).
                Include(x => x.KichCo).
                Include(x => x.MauSac).
                ToListAsync()).FirstOrDefault(sp =>
                sp.IdXuatXu == sanPhamGet!.IdXuatXu &&
                sp.IdMauSac == sanPhamGet.IdMauSac &&
                sp.IdLoaiGiay == sanPhamGet.IdLoaiGiay &&
                sp.IdKieuDeGiay == sanPhamGet.IdKieuDeGiay &&
                sp.IdChatLieu == sanPhamGet.IdChatLieu &&
                sp.IdSanPham == sanPhamGet.IdSanPham &&
                sp.IdThuongHieu == sanPhamGet.IdThuongHieu &&
                sp.IdKichCo == idsSize
                );
            if (sanPhamChiTiet == null) return null;
            var itemDetailViewModel = _mapper.Map<ItemDetailViewModel>(sanPhamChiTiet);
            return itemDetailViewModel;
        }

        public async Task<SanPhamChiTietViewModel?> GetSanPhamChiTietViewModelAynsc(string id)
        {
            var sanPhamChiTiet = await _context.sanPhamChiTiets
                .Include(x => x.Anh)
                .Include(x => x.ChatLieu)
                .Include(x => x.XuatXu)
                .Include(x => x.KieuDeGiay)
                .Include(x => x.MauSac)
                .Include(x => x.KichCo)
                .Include(x => x.LoaiGiay)
                .Include(x => x.SanPham)
                .Include(x => x.ThuongHieu)
                .FirstOrDefaultAsync(x => x.IdChiTietSp == id);
            return _mapper.Map<SanPhamChiTietViewModel>(sanPhamChiTiet);
        }

        public async Task<IEnumerable<SanPhamDanhSachViewModel>> GetListSanPhamNgungKinhDoanhViewModelAsync()
        {
            var sanPhamChiTietViewModels = (await _context.sanPhamChiTiets.ToListAsync()).Where(it => it.TrangThai == (int)TrangThaiCoBan.KhongHoatDong).Select(item => CreateSanPhamDanhSachViewModel(item)).ToList();
            return sanPhamChiTietViewModels;
        }

        public async Task<bool> NgungKinhDoanhSanPhamAynsc(List<string> lstguid)
        {
            try
            {
                var sanPhams = await _context.sanPhamChiTiets
                    .Where(sp => lstguid.Contains(sp.IdChiTietSp!))
                    .ToListAsync();

                if (sanPhams.Count != lstguid.Count)
                {
                    return false;
                }

                foreach (var sanPham in sanPhams)
                {
                    sanPham.TrangThai = (int)TrangThaiCoBan.KhongHoatDong;
                    _context.sanPhamChiTiets.Update(sanPham);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> KinhDoanhLaiSanPhamAynsc(List<string> lstguid)
        {
            try
            {
                var sanPhams = await _context.sanPhamChiTiets
                    .Where(sp => lstguid.Contains(sp.IdChiTietSp!))
                    .ToListAsync();

                if (sanPhams.Count != lstguid.Count)
                {
                    return false;
                }

                foreach (var sanPham in sanPhams)
                {
                    sanPham.TrangThai = (int)TrangThaiCoBan.HoatDong;
                    _context.sanPhamChiTiets.Update(sanPham);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> KhoiPhucKinhDoanhAynsc(string id)
        {
            try
            {
                var entity = await GetByKeyAsync(id)!;
                if (entity == null) return false;
                entity.TrangThai = (int)TrangThaiCoBan.HoatDong;
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
