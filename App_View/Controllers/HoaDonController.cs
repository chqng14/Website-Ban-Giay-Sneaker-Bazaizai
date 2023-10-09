using App_Data.Models;
using App_Data.ViewModels.HoaDonChiTietDTO;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App_View.Controllers
{
    public class HoaDonController : Controller
    {

        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        ISanPhamChiTietService _sanPhamChiTietService;
        IThongTinGHServices thongTinGHServices;
        IGioHangChiTietServices gioHangChiTietServices;
        IHoaDonServices hoaDonServices;
        IHoaDonChiTietServices hoaDonChiTietServices;
        ThongTinGHController ThongTinGHController;
        private readonly GioHangChiTietsController _GioHangChiTietsController;
        public HoaDonController(SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager, ISanPhamChiTietService sanPhamChiTietService, ThongTinGHController thongTinGHController, GioHangChiTietsController gioHangChiTietsController)
        {

            _sanPhamChiTietService = sanPhamChiTietService;
            thongTinGHServices = new ThongTinGHServices();
            _signInManager = signInManager;
            _userManager = userManager;
            gioHangChiTietServices = new GioHangChiTietServices();
            hoaDonServices = new HoaDonServices();
            hoaDonChiTietServices = new HoaDonChiTietServices();
            ThongTinGHController = thongTinGHController;
            _GioHangChiTietsController = gioHangChiTietsController;
        }
        // GET: HoaDonControlle1
        public ActionResult Index()
        {
            return View();
        }

        // GET: HoaDonControlle1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HoaDonControlle1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HoaDonControlle1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HoaDonControlle1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HoaDonControlle1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HoaDonControlle1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HoaDonControlle1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<IActionResult> DataBill(ThongTinGiaoHang thongTinGiaoHang)
        {
            thongTinGiaoHang.IdNguoiDung = _userManager.GetUserId(User);
            await ThongTinGHController.CreateThongTin(thongTinGiaoHang);
            TempData["ThongTinGiaoHang"] = thongTinGiaoHang;
            return RedirectToAction("CheckOut", "GioHangChiTiets");
        }
        public async Task<IActionResult> ThanhToan(HoaDonChiTietDTO hoaDonChiTietDTO)
        {
            var UserID = _userManager.GetUserId(User);
            var listcart = (await gioHangChiTietServices.GetAllGioHang()).Where(c => c.IdNguoiDung == UserID);
            var idthongtinGH = TempData["ThongTinGiaoHang"] as ThongTinGiaoHang;
            var hoadon = new HoaDon()
            {
                IdNguoiDung = UserID,
                IdKhachHang = null,
                IdThongTinGH = idthongtinGH.IdThongTinGH,
                IdVoucher = null,
                MaHoaDon = "HD" + DateTime.Now.ToString("ddMMyyyyhhmmss"),
                NgayTao = DateTime.Now,
                NgayShip = DateTime.Now.AddDays(2),
                NgayNhan = DateTime.Now.AddDays(4),
                NgayThanhToan = DateTime.Now.AddDays(4),
                TienGiam = 0,
                TongTien = 2000000,
                TienShip = 30000,
                MoTa = "0",
                TrangThai = 0,
                TrangThaiThanhToan = 0
            };
            await hoaDonServices.CreateHoaDon(hoadon);
            foreach (var item in listcart)
            {
                await hoaDonChiTietServices.CreateHoaDonChiTiet(new HoaDonChiTietDTO()
                {
                    IdHoaDonChiTiet = Guid.NewGuid().ToString(),
                    IdHoaDon = hoadon.IdHoaDon,
                    IdSanPhamChiTiet = item.IdSanPhamCT,
                    SoLuong = item.SoLuong,
                    GiaGoc = item.GiaGoc,
                    GiaBan = item.GiaBan,
                    TrangThai = 0
                });
                //await CartDetailServices.RemoveItem(item.Id);
                //var product = await ProductDetailServices.GetById(item.IdProduct);
                //await ProductDetailServices.UpdateSoLuong(product.Id, item.SoLuongCart);
            }
            return RedirectToAction("ShowCartUser", "GioHangChiTiets");
        }
    }
}
