using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BanHangTaiQuayController : Controller
    {
        private readonly IHoaDonServices _hoaDonServices;
        private readonly ISanPhamChiTietService _sanPhamChiTietService;
        private readonly BazaizaiContext _bazaizaiContext; 
            //tesst
        public BanHangTaiQuayController(ISanPhamChiTietService sanPhamChiTietService)
        {
            HttpClient httpClient = new HttpClient();
            _hoaDonServices = new HoaDonServices();
            _sanPhamChiTietService = sanPhamChiTietService;
            _bazaizaiContext = new BazaizaiContext();
        }

        [HttpGet]
        public async Task<IActionResult> DanhSachHoaDonCho()
        {
            var listHoaDonCho = await _hoaDonServices.GetAllHoaDonCho();
            return View(listHoaDonCho.OrderBy(c => Convert.ToInt32(c.MaHoaDon.Substring(2, c.MaHoaDon.Length - 2))));
        }
        [HttpPost]
        public async Task<IActionResult> TaoHoaDonTaiQuay()
        {
            var hoaDonMoi = new HoaDon()
            {
                IdHoaDon = Guid.NewGuid().ToString(),
            };
            var newHoaDon = await _hoaDonServices.TaoHoaDonTaiQuay(hoaDonMoi);
            return Json(newHoaDon.MaHoaDon);
        }

        [HttpPost]
        public async Task<IActionResult> ThemSanPhamVaoHoaDon(string maHD, string idSanPham)
        {
            var hoaDon = (await _bazaizaiContext.HoaDons.ToListAsync()).FirstOrDefault(hd=>hd.MaHoaDon == maHD);
            var sanPhamChiTiet = await _sanPhamChiTietService.GetByKeyAsync(idSanPham);
            var check = _bazaizaiContext.hoaDonChiTiets.FirstOrDefault(hdct=>hdct.IdHoaDon == hoaDon.IdHoaDon && hdct.IdSanPhamChiTiet == idSanPham);
            if (check == null)
            {
                await _bazaizaiContext.hoaDonChiTiets.AddAsync(new HoaDonChiTiet()
                {
                    GiaBan = sanPhamChiTiet.GiaThucTe,
                    GiaGoc = sanPhamChiTiet.GiaBan,
                    IdHoaDon = hoaDon.IdHoaDon,
                    IdHoaDonChiTiet = Guid.NewGuid().ToString(),
                    IdSanPhamChiTiet = idSanPham.ToString(),
                    SoLuong = 1,
                    TrangThai = 0
                });
                await _bazaizaiContext.SaveChangesAsync();
            }
            
            return Ok();
            
        }

        [HttpGet]
        public async Task<IActionResult> LoadPartialViewDanhSachSanPham(string tukhoa)
        {
            if (!string.IsNullOrWhiteSpace(tukhoa))
                return PartialView("_DanhSachSanPhamPartialView", (await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync()).Where(c => c.TenSanPham.ToLower().Replace(" ", "").Contains(tukhoa.ToLower())));
            else
                return PartialView("_DanhSachSanPhamPartialView", await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync());
        }
    }
}
