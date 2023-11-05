using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.ViewModels.DonHang;
using App_Data.ViewModels.DonHangChiTiet;
using App_Data.ViewModels.GioHangChiTiet;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly BazaizaiContext _bazaizaiContext;
        public DonHangController()
        {
            _bazaizaiContext = new BazaizaiContext();
        }


        [HttpGet("DonHangs")]
        public async Task<List<DonHangViewModel>> GetDonHangs(string idNguoiDung)
        {
            var lstModelHoaDon = await _bazaizaiContext
                .HoaDons
                .Where(hd => hd.IdNguoiDung == idNguoiDung)
                .Include(hd=>hd.HoaDonChiTiet)!.ThenInclude(it=>it.SanPhamChiTiet).ThenInclude(it=>it.MauSac)
                .Include(hd => hd.HoaDonChiTiet)!.ThenInclude(it => it.SanPhamChiTiet).ThenInclude(it => it.Anh)
                .Include(hd=>hd.HoaDonChiTiet)!.ThenInclude(it=>it.SanPhamChiTiet).ThenInclude(it=>it.KichCo)
                .Include(hd=>hd.HoaDonChiTiet)!.ThenInclude(it=>it.SanPhamChiTiet).ThenInclude(it=>it.SanPham)
                .Include(hd=>hd.HoaDonChiTiet)!.ThenInclude(it=>it.SanPhamChiTiet).ThenInclude(it=>it.ThuongHieu)
                .AsNoTracking().ToListAsync();
            return lstModelHoaDon.Select(hd => 
            new DonHangViewModel()
            {
                IdDonHang = hd.IdHoaDon,
                MaDonHang = hd.MaHoaDon,
                NgayTao = hd.NgayTao.GetValueOrDefault().ToString("dd-MM-yyyy"),
                PhiShip = hd.TienShip.GetValueOrDefault(),
                TongTien = hd.TongTien.GetValueOrDefault(),
                TrangThaiHoaDon = hd.TrangThaiGiaoHang.GetValueOrDefault(),
                NgayGiaoDuKien = hd.NgayGiaoDuKien.GetValueOrDefault().ToString("dd-MM-yyyy"),
                SanPhamGioHangViewModels = hd.HoaDonChiTiet!.Select(hdct => new SanPhamGioHangViewModel()
                {
                    Anh = hdct.SanPhamChiTiet.Anh.OrderBy(a => a.NgayTao).FirstOrDefault()!.Url,
                    GiaSanPham = hdct.GiaBan.GetValueOrDefault(),
                    IdSanPhamChiTiet = hdct.IdSanPhamChiTiet,
                    SoLuong = hdct.SoLuong.GetValueOrDefault(),
                    TenSanPham = $"{hdct.SanPhamChiTiet.SanPham.TenSanPham} {hdct.SanPhamChiTiet.MauSac.TenMauSac} {hdct.SanPhamChiTiet.KichCo.SoKichCo}",
                    TenThuongHieu = hdct.SanPhamChiTiet.ThuongHieu.TenThuongHieu
                })
                .ToList()
            })
                .ToList();
        }

        [HttpGet("GetDonHangDetail")]
        public async Task<DonHangChiTietViewModel> GetDonHangDetails(string idDonHang)
        {
            var hoaDonChiTiets = await _bazaizaiContext.hoaDonChiTiets
               .Where(hdct => hdct.IdHoaDon == idDonHang)
               .Include(it => it.SanPhamChiTiet).ThenInclude(it => it.Anh)
               .Include(it => it.SanPhamChiTiet).ThenInclude(it => it.SanPham)
               .Include(it => it.SanPhamChiTiet).ThenInclude(it => it.ThuongHieu)
               .Include(it => it.SanPhamChiTiet).ThenInclude(it => it.KichCo)
               .Include(it => it.SanPhamChiTiet).ThenInclude(it => it.MauSac)
               .Include(it => it.HoaDon).ThenInclude(it => it.ThongTinGiaoHang)
               .Include(it => it.HoaDon).ThenInclude(it => it.PhuongThucThanhToanChiTiet)!.ThenInclude(it => it.PhuongThucThanhToan)
               .ToListAsync();
            var donHangChiTietViewModel = new DonHangChiTietViewModel()
            {
                IdDonHang = idDonHang,
                MaDonHang = hoaDonChiTiets.FirstOrDefault()!.HoaDon.MaHoaDon,
                VoucherShop = hoaDonChiTiets.FirstOrDefault()?.HoaDon.TienGiam,
                NgayTao = hoaDonChiTiets.FirstOrDefault()!.HoaDon.NgayTao.GetValueOrDefault().ToString("dd-MM-yyyy"),
                PhiShip = hoaDonChiTiets.FirstOrDefault()!.HoaDon.TienShip.GetValueOrDefault(),
                TongTien = (hoaDonChiTiets.FirstOrDefault()!.HoaDon.TongTien.GetValueOrDefault() - hoaDonChiTiets.FirstOrDefault()?.HoaDon.TienGiam + hoaDonChiTiets.FirstOrDefault()!.HoaDon.TienShip.GetValueOrDefault()).GetValueOrDefault(),
                TrangThaiHoaDon = hoaDonChiTiets.FirstOrDefault()!.HoaDon.TrangThaiGiaoHang.GetValueOrDefault(),
                SanPhamGioHangViewModels = hoaDonChiTiets.Select(hdct => new SanPhamGioHangViewModel()
                {
                    Anh = hdct.SanPhamChiTiet.Anh.OrderBy(a => a.NgayTao).FirstOrDefault()!.Url,
                    GiaSanPham = hdct.SanPhamChiTiet.GiaBan.GetValueOrDefault(),
                    IdSanPhamChiTiet = hdct.SanPhamChiTiet.IdChiTietSp,
                    SoLuong = hdct.SoLuong.GetValueOrDefault(),
                    TenSanPham = $"{hdct.SanPhamChiTiet.SanPham.TenSanPham} {hdct.SanPhamChiTiet.MauSac.TenMauSac} {hdct.SanPhamChiTiet.KichCo.SoKichCo}",
                })
                .ToList(),
                DiaChiNhanHang = hoaDonChiTiets.FirstOrDefault()?.HoaDon.ThongTinGiaoHang?.DiaChi,
                NgayGiaoDuKien = hoaDonChiTiets.FirstOrDefault()!.HoaDon.NgayGiaoDuKien.GetValueOrDefault().ToString("dd-MM-yyyy"),
                SDT = hoaDonChiTiets.FirstOrDefault()?.HoaDon.ThongTinGiaoHang?.SDT,
                TenNguoiNhan = hoaDonChiTiets.FirstOrDefault()?.HoaDon.ThongTinGiaoHang?.TenNguoiNhan,
                //PhuongThucThanhToan = hoaDonChiTiets.FirstOrDefault()?.HoaDon.PhuongThucThanhToanChiTiet!.FirstOrDefault()!.PhuongThucThanhToan.TenPhuongThucThanhToan
            };
            return donHangChiTietViewModel;
        }

    }
}
