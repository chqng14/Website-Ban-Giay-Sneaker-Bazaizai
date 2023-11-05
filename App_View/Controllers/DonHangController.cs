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
                listHoaDon = listHoaDon!.Where(dh => dh.TrangThaiGiaoHang == Convert.ToInt32(trangThai)).ToList();
            }
            return PartialView("GetHoaDonOnline", listHoaDon);
        }
        public async Task<IActionResult> DetailHoaDonOnline(string idHoaDon)
        {
            var UserID = _userManager.GetUserId(User);
            var listHoaDon = (await hoaDonServices.GetHoaDonOnline(UserID)).FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            return View(listHoaDon);
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
            var donHangChiTietViewModel = await _httpclient.GetFromJsonAsync<DonHangChiTietViewModel>($"/api/DonHang/GetDonHangDetail?idDonHang={idDonHang}");
            return View(donHangChiTietViewModel);
        }
    }
}
