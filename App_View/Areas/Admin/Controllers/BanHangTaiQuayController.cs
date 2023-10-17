using App_Data.IRepositories;
using App_Data.Models;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BanHangTaiQuayController : Controller
    {
        private readonly IHoaDonServices _hoaDonServices;
        public BanHangTaiQuayController()
        {
            _hoaDonServices = new HoaDonServices();
        }

        [HttpGet]
        public async Task<IActionResult> DanhSachHoaDonCho()
        {
            var listHoaDonCho = await _hoaDonServices.GetAllHoaDonCho();
            return View(listHoaDonCho.OrderBy(c=>Convert.ToInt32(c.MaHoaDon.Substring(2,c.MaHoaDon.Length-2))));
        }
        [HttpPost]
        public async Task<IActionResult> TaoHoaDonTaiQuay() {
            var hoaDonMoi = new HoaDon() { 
                IdHoaDon = Guid.NewGuid().ToString(),
            };
            var newHoaDon = await _hoaDonServices.TaoHoaDonTaiQuay(hoaDonMoi);
            return Json(newHoaDon.MaHoaDon);
        }
    }
}
