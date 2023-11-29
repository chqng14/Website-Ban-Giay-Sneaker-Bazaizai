using App_Data.DbContextt;
using App_Data.Models;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_View.IServices;
using App_View.Models.ViewModels;
using App_View.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrangThaiGiaoHangController : Controller
    {
        private readonly IVoucherNguoiDungServices _voucherNguoiDungServices;
        private readonly IVoucherServices _voucherServices;
        private readonly IHoaDonChiTietServices _hoaDonChiTietServices; private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IHoaDonServices _hoaDonServices;
        private readonly ISanPhamChiTietService sanPhamChiTietService;
        BazaizaiContext context;
        public TrangThaiGiaoHangController(ISanPhamChiTietService sanPhamChiTietService, IVoucherNguoiDungServices voucherNguoiDungServices, IVoucherServices voucherServices, IHoaDonChiTietServices hoaDonChiTietServices, SignInManager<NguoiDung> signInManager)
        {
            _hoaDonServices = new HoaDonServices();
            context = new BazaizaiContext();
            this.sanPhamChiTietService = sanPhamChiTietService;
            _voucherNguoiDungServices = voucherNguoiDungServices;
            _voucherServices = voucherServices;
            _hoaDonChiTietServices = hoaDonChiTietServices;
            _signInManager = signInManager;
        }
        public IActionResult QuanLyGiaoHang()
        {
            return View();
        }
        public async Task<IActionResult> QuanLyTrangThaiGiaoHangAsync(int trangThaiHD, string search)
        {
            var lstHoaDon = (await _hoaDonServices.GetHoaDon()).ToList();
            
           
            if (!string.IsNullOrEmpty(search))
            {
                lstHoaDon = lstHoaDon.Where(x => x.MaHoaDon.ToUpper().Contains(search.ToUpper())).ToList();
            }
            if (trangThaiHD == 0)
            {
                lstHoaDon = lstHoaDon.Where(x => x.TrangThaiGiaoHang!=0).ToList();
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDon);
            }
            if (trangThaiHD == 1)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 1);
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDonOnline);
            }
            if (trangThaiHD == 2)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 2);
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDonOnline);
            }
            if (trangThaiHD == 3)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 3 );
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDonOnline);
            }
            if (trangThaiHD == 4)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 4);
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDonOnline);
            }
            if (trangThaiHD == 5)
            {
                var lstHoaDonDaHuy = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 5);
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDonDaHuy);
            }
            return PartialView("QuanLyHoaDon", lstHoaDon);
        }
        [HttpPost]
        public async Task<IActionResult> ChiTietGiaoHangAsync(string id)
        {

            var hoaDon = (await _hoaDonServices.GetHoaDon()).FirstOrDefault(x => x.IdHoaDon == id);

            var hoaDonChiTiet = context.HoaDons.FirstOrDefault(x => x.IdHoaDon == hoaDon.IdHoaDon);

            return PartialView("ChiTietGiaoHang", hoaDonChiTiet);
        }
        [HttpPost]
        public IActionResult CapNhatTrangThai(string trangThaiGH,string id)
        {
            var hoaDon = context.HoaDons.FirstOrDefault(x => x.IdHoaDon == id);
            hoaDon.TrangThaiGiaoHang = Convert.ToInt32(trangThaiGH);
            context.HoaDons.Update(hoaDon);
            context.SaveChanges();
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> HuyHoaDon(string maHD, string lyDoHuy)
        {
            if (string.IsNullOrEmpty(maHD))
            {
                return Ok(new
                {
                    TrangThai = false,
                });
            }
            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(c => c.MaHoaDon == maHD);
            var user = await _userManager.GetUserAsync(User);
            var idUser = await _userManager.GetUserIdAsync(user);
            var hoadonchitiet = await _hoaDonChiTietServices.HuyHoaDon(maHD, lyDoHuy, idUser);
            if (hoaDon.hoaDonChiTietDTOs.Count() == 0)
            {
                return Ok(
                  new { TrangThai = true, }
                  );
            }
            
            return Ok(new
            {
                TrangThai = false,
            });
        }
        //public async Task<IActionResult> HuyHoaDon(string maHD, string lyDoHuy)
        //{
        //    if (string.IsNullOrEmpty(maHD))
        //    {
        //        return Ok(new
        //        {
        //            TrangThai = false,
        //        });
        //    }
        //    var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(c => c.MaHoaDon == maHD);
        //    var user = await _userManager.GetUserAsync(User);
        //    var idUser = await _userManager.GetUserIdAsync(user);
        //    var hoadonchitiet = await _hoaDonChiTietServices.HuyHoaDon(maHD, lyDoHuy, idUser);
        //    if (hoaDon.hoaDonChiTietDTOs.Count() == 0)
        //    {
        //        return Ok(
        //          new { TrangThai = true, }
        //          );
        //    }
        //    if (hoadonchitiet.Any())
        //    {
        //        foreach (var item in hoadonchitiet)
        //        {
        //            await sanPhamChiTietService.UpDatSoLuongAynsc(new SanPhamSoLuongDTO()
        //            {
        //                IdChiTietSanPham = item.IdSanPhamChiTiet,
        //                SoLuong = -(int)item.SoLuong,
        //            });
        //        }
        //        return Ok(
        //            new { TrangThai = true, }
        //            );
        //    }
        //    return Ok(new
        //    {
        //        TrangThai = false,
        //    });
        //}
    }
}
