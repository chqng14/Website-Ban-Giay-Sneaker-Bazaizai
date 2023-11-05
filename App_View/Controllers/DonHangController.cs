using App_Data.DbContextt;
using App_Data.Models;
using App_Data.ViewModels.DonHang;
using App_Data.ViewModels.DonHangChiTiet;
using App_Data.ViewModels.GioHangChiTiet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace App_View.Controllers
{
    public class DonHangController: Controller
    {
        private readonly HttpClient _httpclient;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly BazaizaiContext _bazaizaiContext;

        public DonHangController(HttpClient httpclient, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager)
        {
            _httpclient = httpclient;
            _signInManager = signInManager;
            _userManager = userManager;
            ;
            _bazaizaiContext = new BazaizaiContext();
        }

        public IActionResult DonHangs()
        {
            return View();
        }

        public async Task<IActionResult> LoadPartialViewDonHangs(string trangThai)
        {
            var idNguoiDung = _userManager.GetUserId(User);
            var lstDonHangViewModel = await _httpclient.GetFromJsonAsync<List<DonHangViewModel>>($"/api/DonHang/DonHangs?idNguoiDung={idNguoiDung}");
            lstDonHangViewModel = lstDonHangViewModel!.OrderByDescending(dh => dh.NgayTao).ToList();
            if (!string.IsNullOrEmpty(trangThai))
            {
                lstDonHangViewModel = lstDonHangViewModel!.Where(dh => dh.TrangThaiHoaDon == Convert.ToInt32(trangThai)).ToList();
            }
            return PartialView("_DonHangPartialView", lstDonHangViewModel);
        }

        public async Task<IActionResult> HuyDonHang(string idDonHang)
        {
            var donHang = await _bazaizaiContext.HoaDons.FirstOrDefaultAsync(dh => dh.IdHoaDon == idDonHang);
            donHang!.TrangThaiGiaoHang = 5;
            _bazaizaiContext.HoaDons.Update(donHang!);
            await _bazaizaiContext.SaveChangesAsync();
            return View("DonHangs");
        }

        public async Task<IActionResult> DonHangDetail(string idDonHang)
        {
            var hoaDonChiTiets = await _bazaizaiContext.hoaDonChiTiets
                .Where(hdct => hdct.IdHoaDon == idDonHang)
                .Include(it=>it.SanPhamChiTiet).ThenInclude(it=>it.Anh)
                .Include(it=>it.SanPhamChiTiet).ThenInclude(it=>it.SanPham)
                .Include(it=>it.SanPhamChiTiet).ThenInclude(it=>it.ThuongHieu)
                .Include(it=>it.SanPhamChiTiet).ThenInclude(it=>it.KichCo)
                .Include(it=>it.SanPhamChiTiet).ThenInclude(it=>it.MauSac)
                .Include(it=>it.HoaDon).ThenInclude(it=>it.ThongTinGiaoHang)
                .Include(it => it.HoaDon).ThenInclude(it=>it.PhuongThucThanhToanChiTiet)!.ThenInclude(it=>it.PhuongThucThanhToan)
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
            return View(donHangChiTietViewModel);
        }
    }
}
