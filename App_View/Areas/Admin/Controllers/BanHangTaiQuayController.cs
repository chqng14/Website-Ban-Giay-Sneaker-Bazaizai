using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.HoaDonChiTietDTO;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static App_Data.Repositories.TrangThai;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BanHangTaiQuayController : Controller
    {
        private readonly IHoaDonServices _hoaDonServices;
        private readonly ISanPhamChiTietService _sanPhamChiTietService;
        private readonly IHoaDonChiTietServices _hoaDonChiTietServices;
        private readonly BazaizaiContext _bazaizaiContext; 
            //tesst
        public BanHangTaiQuayController(ISanPhamChiTietService sanPhamChiTietService)
        {
            HttpClient httpClient = new HttpClient();
            _hoaDonServices = new HoaDonServices();
            _sanPhamChiTietService = sanPhamChiTietService;
            _hoaDonChiTietServices = new HoaDonChiTietServices();
            _bazaizaiContext = new BazaizaiContext();
        }

        [HttpGet]
        public async Task<IActionResult> DanhSachHoaDonCho()
        {
            var listHoaDonCho = await _hoaDonServices.GetAllHoaDonCho();
            var listsanpham = await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync();
            foreach (var item in listHoaDonCho)
            {
                foreach (var item2 in item.hoaDonChiTietDTOs)
                {
                    var sanpham = listsanpham.FirstOrDefault(c => c.IdChiTietSp == item2.IdSanPhamChiTiet);
                    item2.TenSanPham = sanpham.TenSanPham + "/" + sanpham.MauSac + "/" + sanpham.KichCo;
                    //item2.masanpham  = sanpham
                }
            }
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
            var hoaDon =    (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(hd=>hd.MaHoaDon == maHD);
            var sanPham = (await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync()).FirstOrDefault(c => c.IdChiTietSp == idSanPham);
            var hoaDonChitiet = new HoaDonChiTiet()
                {
                    IdHoaDon = hoaDon.Id,
                    IdHoaDonChiTiet = Guid.NewGuid().ToString(),
                    IdSanPhamChiTiet = idSanPham.ToString(),
                    SoLuong = 1,
                    TrangThai = (int)TrangThaiHoaDonChiTiet.ChoTaiQuay,
                GiaBan = sanPham.GiaThucTe,
                GiaGoc = sanPham.GiaGoc,
            };
            var hoaDonChiTietTraLai = await _hoaDonChiTietServices.ThemSanPhamVaoHoaDon(hoaDonChitiet);
          
            return Ok(new HoaDonChiTietTaiQuay()
            {
                IdHoaDon =  hoaDonChiTietTraLai.IdHoaDon,
                IdHoaDonChiTiet = hoaDonChiTietTraLai.IdHoaDonChiTiet,
                IdSanPhamChiTiet = hoaDonChiTietTraLai.IdSanPhamChiTiet,
                SoLuong = hoaDonChiTietTraLai.SoLuong,
                GiaBan = hoaDonChiTietTraLai.GiaBan,
                GiaGoc = hoaDonChiTietTraLai.GiaGoc,
                TenSanPham = sanPham.TenSanPham+"/"+sanPham.MauSac+"/"+sanPham.KichCo,
                TrangThai = hoaDonChiTietTraLai.TrangThai
            });
        }

        [HttpGet]
        public async Task<IActionResult> LoadPartialViewDanhSachSanPham(string tukhoa)
        {
            if (!string.IsNullOrWhiteSpace(tukhoa))
                return PartialView("_DanhSachSanPhamPartialView", (await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync()).Where(c => c.TenSanPham.ToLower().Replace(" ", "").Contains(tukhoa.ToLower().Replace(" ", ""))));
            else
                return PartialView("_DanhSachSanPhamPartialView", await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync());
        }
    }
}
