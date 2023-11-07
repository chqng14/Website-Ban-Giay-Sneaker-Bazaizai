using App_Data.DbContextt;
using App_Data.Models;
using App_Data.ViewModels.DonHang;
using App_Data.ViewModels.DonHangChiTiet;
using App_Data.ViewModels.GioHangChiTiet;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace App_View.Controllers
{
    public class DonHangController : Controller
    {
        private readonly HttpClient _httpclient;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly BazaizaiContext _bazaizaiContext;
        private readonly IHoaDonServices hoaDonServices;

        public DonHangController(HttpClient httpclient, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager)
        {
            _httpclient = httpclient;
            _signInManager = signInManager;
            _userManager = userManager;
            _bazaizaiContext = new BazaizaiContext();
            hoaDonServices = new HoaDonServices();
        }

        public IActionResult DonHangs()
        {
            return View();
        }
        public async Task<IActionResult> GetHoaDonOnline(string trangThai)
        {
            var UserID = _userManager.GetUserId(User);
            var listHoaDon = await hoaDonServices.GetHoaDonOnline(UserID);
            listHoaDon = listHoaDon.OrderByDescending(dh => dh.NgayTao).ToList();
            if (!string.IsNullOrEmpty(trangThai))
            {
                listHoaDon = listHoaDon.Where(dh => dh.TrangThaiGiaoHang == Convert.ToInt32(trangThai)).ToList();
            }
            return PartialView("GetHoaDonOnline", listHoaDon);
        }
        public async Task<IActionResult> DetailHoaDonOnline(string idHoaDon)
        {
            var UserID = _userManager.GetUserId(User);
            var listHoaDon = (await hoaDonServices.GetHoaDonOnline(UserID)).FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            return View(listHoaDon);
        }

        public async Task<IActionResult> HuyDonHang(string idHoaDon, string Lido)
        {
            await hoaDonServices.UpdateTrangThaiGiaoHangHoaDon(idHoaDon, 5, Lido);
            return Ok(new { idHoaDon = idHoaDon });
        }

    }
}
