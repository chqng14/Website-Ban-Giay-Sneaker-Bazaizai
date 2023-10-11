using App_Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BanHangTaiQuayController : Controller
    {
        public IActionResult DanhSachHoaDonCho()
        {
            var fakeData = new List<HoaDon>() {
                new HoaDon() {MaHoaDon = "HD1"},
                new HoaDon() {MaHoaDon = "HD2"}
            };
            return View();
        }
    }
}
