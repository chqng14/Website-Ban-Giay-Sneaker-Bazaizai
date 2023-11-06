using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.FilterViewModel;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
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
            var lstSanPhamChiTiet = await _context.sanPhamChiTiets
                 .Where(sp => lstGuid.Contains(sp.IdChiTietSp!))
                 .Include(x => x.Anh)
                 .Include(x => x.SanPham)
                 .Include(x => x.MauSac)
                 .Include(x => x.KichCo)
                 .Include(x => x.ThuongHieu)
                 .ToListAsync();
            return _mapper.Map<List<SanPhamChiTietDTO>>(lstSanPhamChiTiet);
        }

        public async Task<DanhSachGiayViewModel> GetDanhSachGiayViewModelAsync()
        {
            var lstSanPhamChiTiet = await _context.sanPhamChiTiets
                .Include(it => it.SanPham)
                .Include(it => it.ThuongHieu)
                .Include(it => it.KichCo)
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
                MauSac = _context.mauSacs.ToList().FirstOrDefault(ms => ms.IdMauSac == sanPham.IdMauSac)?.TenMauSac,
                KichCo = _context.kichCos.ToList().FirstOrDefault(x => x.IdKichCo == sanPham.IdKichCo)?.SoKichCo,
                Anh = _context.Anh.ToList().Where(x => x.IdSanPhamChiTiet == sanPham.IdChiTietSp && x.TrangThai == 0).OrderBy(x => x.NgayTao).FirstOrDefault()?.Url,
                KieuDeGiay = _context.kieuDeGiays.ToList().FirstOrDefault(x => x.IdKieuDeGiay == sanPham.IdKieuDeGiay)?.TenKieuDeGiay,
                LoaiGiay = _context.LoaiGiays.ToList().FirstOrDefault(x => x.IdLoaiGiay == sanPham.IdLoaiGiay)?.TenLoaiGiay,
                SoLuongTon = sanPham.SoLuongTon,
                Ma = sanPham.Ma
            };
        }

        public async Task<IEnumerable<SanPhamDanhSachViewModel>> GetListViewModelAsync()
        {
            var sanPhamChiTietViewModels = (await _context.sanPhamChiTiets.ToListAsync()).Where(it => it.TrangThai == 0).OrderByDescending(x => x.NgayTao).Select(item => CreateSanPhamDanhSachViewModel(item)).ToList();
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
            var listSanPham = await _context.sanPhamChiTiets
                .Include(it => it.SanPham)
                .Include(it => it.ThuongHieu)
                .Include(it => it.LoaiGiay)
                .Include(it => it.Anh)
                .Where(sp => sp.TrangThai == 0)
                .GroupBy(
                  gr => new
                  {
                      gr.IdChatLieu,
                      gr.IdSanPham,
                      gr.IdLoaiGiay,
                      gr.IdKieuDeGiay,
                      gr.IdThuongHieu,
                      gr.IdXuatXu,
                  })
                .Select(gr => gr.First())
                .ToListAsync();

            var itemShops = _mapper.Map<List<ItemShopViewModel>>(listSanPham.OrderByDescending(x => x.NgayTao));
            return itemShops;
        }

        public async Task<List<ItemShopViewModel>> GetDanhSachBienTheItemShopViewModelAsync()
        {
            var listSanPham = await _context.sanPhamChiTiets
                .Where(sp => sp.TrangThai == 0 && sp.TrangThaiSale == 2)
                .OrderByDescending(x => x.NgayTao)
                .AsNoTracking().ToListAsync();

            var itemShops = listSanPham.Select(sp => new ItemShopViewModel()
            {
                Anh = _context.Anh.Where(a => a.IdSanPhamChiTiet == sp.IdChiTietSp).OrderBy(a => a.NgayTao).FirstOrDefault()!.Url,
                GiaGoc = sp.GiaBan,
                GiaKhuyenMai = sp.GiaThucTe,
                MauSac = _context.mauSacs.FirstOrDefault(ms => ms.IdMauSac == sp.IdMauSac)!.TenMauSac,
                TheLoai = _context.LoaiGiays.FirstOrDefault(lg => lg.IdLoaiGiay == sp.IdLoaiGiay)!.TenLoaiGiay,
                KichCo = Convert.ToInt32(_context.kichCos.FirstOrDefault(kc => kc.IdKichCo == sp.IdKichCo).SoKichCo),
                IdChiTietSp = sp.IdChiTietSp,
                SoLanDanhGia = 32,
                TenSanPham = _context.SanPhams.FirstOrDefault(sp => sp.IdSanPham == sp.IdSanPham)!.TenSanPham,
                ThuongHieu = _context.thuongHieus.FirstOrDefault(th => th.IdThuongHieu == sp.IdThuongHieu)!.TenThuongHieu,
                GiaThucTe = sp.GiaThucTe,
                IsKhuyenMai = sp.TrangThaiSale == 2 ? true : false
            }).ToList();
            return itemShops;
        }

        public async Task<List<ItemShopViewModel>> GetDanhSachBienTheItemShopViewModelSaleAsync()
        {
            var listSanPham = await _context.sanPhamChiTiets
                .Where(sp => sp.TrangThai == 0 && sp.TrangThaiSale == 2)
                .OrderByDescending(x => x.NgayTao)
                .AsNoTracking().ToListAsync();

            var itemShops = listSanPham.Select(sp => new ItemShopViewModel()
            {
                Anh = _context.Anh.Where(a => a.IdSanPhamChiTiet == sp.IdChiTietSp).OrderBy(a => a.NgayTao).FirstOrDefault()!.Url,
                GiaGoc = sp.GiaBan,
                GiaKhuyenMai = sp.GiaThucTe,
                MauSac = _context.mauSacs.FirstOrDefault(ms => ms.IdMauSac == sp.IdMauSac)!.TenMauSac,
                TheLoai = _context.LoaiGiays.FirstOrDefault(lg => lg.IdLoaiGiay == sp.IdLoaiGiay)!.TenLoaiGiay,
                KichCo = Convert.ToInt32(_context.kichCos.FirstOrDefault(kc => kc.IdKichCo == sp.IdKichCo).SoKichCo),
                IdChiTietSp = sp.IdChiTietSp,
                SoLanDanhGia = 32,
                TenSanPham = _context.SanPhams.FirstOrDefault(sp => sp.IdSanPham == sp.IdSanPham)!.TenSanPham,
                ThuongHieu = _context.thuongHieus.FirstOrDefault(th => th.IdThuongHieu == sp.IdThuongHieu)!.TenThuongHieu,
                GiaThucTe = sp.GiaThucTe

            }).ToList();
            return itemShops;
        }

        public async Task<ItemDetailViewModel?> GetItemDetailViewModelAynsc(string id)
        {
            var sanPhamChiTiet = await _context.sanPhamChiTiets.
                Include(x => x.Anh).
                Include(x => x.SanPham).
                Include(x => x.ThuongHieu).
                Include(x => x.KichCo).
                Include(x => x.MauSac).
                Include(x => x.SanPhamYeuThichs).
                FirstOrDefaultAsync(sp => sp.IdChiTietSp == id);
            if (sanPhamChiTiet == null) return null;
            var itemDetailViewModel = _mapper.Map<ItemDetailViewModel>(sanPhamChiTiet);
            var lstBienThe = await _context.sanPhamChiTiets
                .Include(x => x.MauSac)
                .Include(x => x.KichCo)
                .Where(sp =>
                sp.TrangThai == 0 &&
                sp.IdKieuDeGiay == sanPhamChiTiet!.IdKieuDeGiay &&
                sp.IdLoaiGiay == sanPhamChiTiet.IdLoaiGiay &&
                sp.IdThuongHieu == sanPhamChiTiet.IdThuongHieu &&
                sp.IdXuatXu == sanPhamChiTiet.IdXuatXu &&
                sp.IdChatLieu == sanPhamChiTiet.IdChatLieu &&
                sp.IdSanPham == sanPhamChiTiet.IdSanPham
                ).ToListAsync();
            itemDetailViewModel.LstMauSac = lstBienThe.Select(x => x.MauSac.TenMauSac).Distinct().ToList()!;
            itemDetailViewModel.LstKichThuoc = lstBienThe.Where(sp => sp.IdMauSac == sanPhamChiTiet.IdMauSac).Select(x => x.KichCo.SoKichCo!.Value).Distinct().OrderBy(item => item).ToList();
            return itemDetailViewModel;
        }

        public async Task<ItemDetailViewModel?> GetItemDetailViewModelWhenSelectColorAynsc(string id, string mauSac)
        {
            var sanPhamGet = await _context.sanPhamChiTiets.FirstOrDefaultAsync(sp => sp.IdChiTietSp == id);

            var idMauSac = (await _context.mauSacs.FirstOrDefaultAsync(x => x.TenMauSac == mauSac))!.IdMauSac;
            if (sanPhamGet == null) return null;
            var lstBienThe = await _context.sanPhamChiTiets
                .Where(sp =>
                sp.TrangThai == 0 &&
                sp.IdKieuDeGiay == sanPhamGet!.IdKieuDeGiay &&
                sp.IdLoaiGiay == sanPhamGet.IdLoaiGiay &&
                sp.IdThuongHieu == sanPhamGet.IdThuongHieu &&
                sp.IdXuatXu == sanPhamGet.IdXuatXu &&
                sp.IdChatLieu == sanPhamGet.IdChatLieu &&
                sp.IdSanPham == sanPhamGet.IdSanPham &&
                sp.IdMauSac == idMauSac
                )
                .Include(x => x.MauSac)
                .Include(x => x.KichCo)
                .Include(x => x.Anh)
                .ToListAsync();
            var lstSize = lstBienThe.DistinctBy(sp => sp.KichCo.SoKichCo).OrderBy(item => item.KichCo.SoKichCo).ToList();
            var idSizeGet = lstSize.FirstOrDefault()!.KichCo.IdKichCo;
            var sanPhamChiTiet = lstBienThe.FirstOrDefault(sp => sp.IdKichCo == idSizeGet);
            var itemDetailViewModel = _mapper.Map<ItemDetailViewModel>(sanPhamChiTiet);
            itemDetailViewModel.LstKichThuoc = lstBienThe.Select(x => x.KichCo.SoKichCo!.Value).Distinct().OrderBy(item => item).ToList();
            return itemDetailViewModel;
        }

        public async Task<ItemDetailViewModel?> GetItemDetailViewModelWhenSelectSizeAynsc(string id, int size)
        {
            var sanPhamGet = await _context.sanPhamChiTiets.FirstOrDefaultAsync(sp => sp.IdChiTietSp == id);

            var idsSize = (await _context.kichCos.FirstOrDefaultAsync(x => x.SoKichCo == size))!.IdKichCo;
            var sanPhamChiTiet = (await _context.sanPhamChiTiets.Where(sp =>
                sp.IdXuatXu == sanPhamGet!.IdXuatXu &&
                sp.IdMauSac == sanPhamGet.IdMauSac &&
                sp.IdLoaiGiay == sanPhamGet.IdLoaiGiay &&
                sp.IdKieuDeGiay == sanPhamGet.IdKieuDeGiay &&
                sp.IdChatLieu == sanPhamGet.IdChatLieu &&
                sp.IdSanPham == sanPhamGet.IdSanPham &&
                sp.IdThuongHieu == sanPhamGet.IdThuongHieu &&
                sp.IdKichCo == idsSize).
                Include(x => x.Anh).
                Include(x => x.SanPham).
                Include(x => x.ThuongHieu).
                Include(x => x.KichCo).
                Include(x => x.MauSac).FirstOrDefaultAsync());
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

        public async Task<List<SanPhamChiTietExcelViewModel>> GetListSanPhamExcelAynsc()
        {
            var listSanPhamChiTiet = (await _context.sanPhamChiTiets
                .Include(sp => sp.ThuongHieu)
                .Include(sp => sp.MauSac)
                .Include(sp => sp.KichCo)
                .Include(sp => sp.LoaiGiay)
                .Include(sp => sp.KieuDeGiay)
                .Include(sp => sp.SanPham)
                .Include(sp => sp.ChatLieu)
                .Include(sp => sp.XuatXu)
                .ToListAsync()).Where(sp => sp.TrangThai == 0).ToList();
            return _mapper.Map<List<SanPhamChiTiet>, List<SanPhamChiTietExcelViewModel>>(listSanPhamChiTiet);
        }

        public async Task<SanPhamChiTietDTO> GetItemExcelAynsc(BienTheDTO bienTheDTO)
        {
            var chatLieu = await _context.ChatLieus.FirstOrDefaultAsync(cl => cl.TenChatLieu == bienTheDTO.ChatLieu);
            if (chatLieu == null)
            {
                chatLieu = new ChatLieu()
                {
                    IdChatLieu = Guid.NewGuid().ToString(),
                    MaChatLieu = !_context.ChatLieus.Any() ? "CL1" : "CL2" + (_context.ChatLieus.Count() + 1),
                    TenChatLieu = bienTheDTO.ChatLieu,
                    TrangThai = 0
                };
                await _context.ChatLieus.AddAsync(chatLieu);
            }

            var xuatXu = await _context.xuatXus.FirstOrDefaultAsync(cl => cl.Ten == bienTheDTO.XuatXu);
            if (xuatXu == null)
            {
                xuatXu = new XuatXu()
                {
                    IdXuatXu = Guid.NewGuid().ToString(),
                    Ma = !_context.xuatXus.Any() ? "XX1" : "XX" + (_context.ChatLieus.Count() + 1),
                    Ten = bienTheDTO.XuatXu,
                    TrangThai = 0
                };
                await _context.xuatXus.AddAsync(xuatXu);
            }

            var mauSac = await _context.mauSacs.FirstOrDefaultAsync(cl => cl.TenMauSac == bienTheDTO.MauSac);
            if (mauSac == null)
            {
                mauSac = new MauSac()
                {
                    IdMauSac = Guid.NewGuid().ToString(),
                    MaMauSac = !_context.mauSacs.Any() ? "MS1" : "MS" + (_context.mauSacs.Count() + 1),
                    TenMauSac = bienTheDTO.MauSac,
                    TrangThai = 0
                };
                await _context.mauSacs.AddAsync(mauSac);
            }

            var kichCo = await _context.kichCos.FirstOrDefaultAsync(cl => cl.SoKichCo == Convert.ToInt32(bienTheDTO.KichCo));
            if (kichCo == null)
            {
                kichCo = new KichCo()
                {
                    IdKichCo = Guid.NewGuid().ToString(),
                    MaKichCo = !_context.kichCos.Any() ? "KC1" : "KC" + (_context.kichCos.Count() + 1),
                    SoKichCo = Convert.ToInt32(bienTheDTO.KichCo),
                    TrangThai = 0
                };
                await _context.kichCos.AddAsync(kichCo);
            }

            var loaiGiay = await _context.LoaiGiays.FirstOrDefaultAsync(cl => cl.TenLoaiGiay == bienTheDTO.LoaiGiay);
            if (loaiGiay == null)
            {
                loaiGiay = new LoaiGiay()
                {
                    IdLoaiGiay = Guid.NewGuid().ToString(),
                    MaLoaiGiay = !_context.LoaiGiays.Any() ? "LG1" : "LG" + (_context.LoaiGiays.Count() + 1),
                    TenLoaiGiay = bienTheDTO.LoaiGiay,
                    TrangThai = 0
                };
                await _context.LoaiGiays.AddAsync(loaiGiay);
            }

            var kieuDeGiay = await _context.kieuDeGiays.FirstOrDefaultAsync(cl => cl.TenKieuDeGiay == bienTheDTO.KieuDeGiay);
            if (kieuDeGiay == null)
            {
                kieuDeGiay = new KieuDeGiay()
                {
                    IdKieuDeGiay = Guid.NewGuid().ToString(),
                    MaKieuDeGiay = !_context.kieuDeGiays.Any() ? "KDG1" : "KDG" + (_context.kieuDeGiays.Count() + 1),
                    TenKieuDeGiay = bienTheDTO.KieuDeGiay,
                    Trangthai = 0
                };
                await _context.kieuDeGiays.AddAsync(kieuDeGiay);
            }

            var sanPham = await _context.SanPhams.FirstOrDefaultAsync(cl => cl.TenSanPham == bienTheDTO.SanPham);
            if (sanPham == null)
            {
                sanPham = new SanPham()
                {
                    IdSanPham = Guid.NewGuid().ToString(),
                    MaSanPham = !_context.SanPhams.Any() ? "SP1" : "SP" + (_context.SanPhams.Count() + 1),
                    TenSanPham = bienTheDTO.SanPham,
                    Trangthai = 0
                };
                await _context.SanPhams.AddAsync(sanPham);
            }

            var thuongHieu = await _context.thuongHieus.FirstOrDefaultAsync(cl => cl.TenThuongHieu == bienTheDTO.ThuongHieu);
            if (thuongHieu == null)
            {
                thuongHieu = new ThuongHieu()
                {
                    IdThuongHieu = Guid.NewGuid().ToString(),
                    MaThuongHieu = !_context.thuongHieus.Any() ? "TH1" : "TH" + (_context.thuongHieus.Count() + 1),
                    TenThuongHieu = bienTheDTO.ThuongHieu,
                    TrangThai = 0
                };
                await _context.thuongHieus.AddAsync(thuongHieu);
            }

            await _context.SaveChangesAsync();

            var spDTO = new SanPhamChiTietDTO()
            {
                IdChatLieu = chatLieu.IdChatLieu,
                IdKichCo = kichCo.IdKichCo,
                IdKieuDeGiay = kieuDeGiay.IdKieuDeGiay,
                IdLoaiGiay = loaiGiay.IdLoaiGiay,
                IdMauSac = mauSac.IdMauSac,
                IdThuongHieu = thuongHieu.IdThuongHieu,
                IdSanPham = sanPham.IdSanPham,
                IdXuatXu = xuatXu.IdXuatXu,
            };
            return spDTO;
        }

        public async Task UpdateSoLuongSanPhamChiTietAynsc(string IdSanPhamChiTiet, int soLuong)
        {
            var sanPhamChiTiet = await _context.sanPhamChiTiets.FirstOrDefaultAsync(x => x.IdChiTietSp == IdSanPhamChiTiet);
            sanPhamChiTiet!.SoLuongTon = sanPhamChiTiet.SoLuongTon - soLuong;
            sanPhamChiTiet!.SoLuongDaBan = soLuong;
            await _context.SaveChangesAsync();
        }

        //Filter
        public async Task<FiltersVM> GetFiltersVMAynsc()
        {
            try
            {
                var query = _context.sanPhamChiTiets
                        .Where(sp => sp.TrangThai == (int)TrangThaiCoBan.HoatDong)
                        .Include(sp => sp.MauSac)
                        .Include(sp => sp.ThuongHieu)
                        .Include(sp => sp.LoaiGiay)
                        .Include(sp => sp.KichCo);

                var data = await query.ToListAsync();

                return new FiltersVM()
                {
                    LstItemFilterMauSac = GetListItemFilter(data, sp => sp.MauSac.TenMauSac!),
                    LstItemFilterKichCo = GetListItemFilter(data, sp => sp.KichCo.SoKichCo.ToString()!).OrderBy(x => x.Ten).ToList(),
                    LstItemFilterTheLoai = GetListItemFilter(data, sp => sp.LoaiGiay.TenLoaiGiay!),
                    LstItemFilterThuongHieu = GetListItemFilter(data, sp => sp.ThuongHieu.TenThuongHieu!),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new FiltersVM();
            }

        }

        private List<ItemFilter> GetListItemFilter(List<SanPhamChiTiet> data, Func<SanPhamChiTiet, string> selector)
        {
            return data
                .GroupBy(selector)
                .Select(group => new ItemFilter()
                {
                    Ten = group.Key,
                    SoLuong = group.Count(),
                })
                .ToList();
        }

        public async Task<bool> ProductIsNull(SanPhamChiTietCopyDTO sanPhamChiTietCopyDTO)
        {
            var sanPhamChiTiet = await _context.sanPhamChiTiets
                .Where(x =>
                x.IdSanPham == sanPhamChiTietCopyDTO.SanPhamChiTietData!.IdSanPham &&
                x.IdChatLieu == sanPhamChiTietCopyDTO.SanPhamChiTietData.IdChatLieu &&
                x.IdKichCo == sanPhamChiTietCopyDTO.SanPhamChiTietData.IdKichCo &&
                x.IdMauSac == sanPhamChiTietCopyDTO.SanPhamChiTietData.IdMauSac &&
                x.IdKieuDeGiay == sanPhamChiTietCopyDTO.SanPhamChiTietData.IdKieuDeGiay &&
                x.IdLoaiGiay == sanPhamChiTietCopyDTO.SanPhamChiTietData.IdLoaiGiay &&
                x.IdThuongHieu == sanPhamChiTietCopyDTO.SanPhamChiTietData.IdThuongHieu &&
                x.IdXuatXu == sanPhamChiTietCopyDTO.SanPhamChiTietData.IdXuatXu
                ).FirstOrDefaultAsync();
            return sanPhamChiTiet != null ? false : true;
        }


    }
}
