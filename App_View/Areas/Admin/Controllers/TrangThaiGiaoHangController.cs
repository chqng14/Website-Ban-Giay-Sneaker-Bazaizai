using App_Data.DbContextt;
using App_Data.Models;
using App_Data.ViewModels.HoaDon;
using App_View.IServices;
using App_View.Models.ViewModels;
using App_View.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrangThaiGiaoHangController : Controller
    {
        private readonly IHoaDonServices _hoaDonServices;
        private readonly ISanPhamChiTietService sanPhamChiTietService;
        BazaizaiContext context;
        public TrangThaiGiaoHangController(ISanPhamChiTietService sanPhamChiTietService)
        {
            _hoaDonServices = new HoaDonServices();
            context = new BazaizaiContext();
            this.sanPhamChiTietService = sanPhamChiTietService;
        }
        public IActionResult QuanLyGiaoHang()
        {
            return View();
        }
        
    }
}
