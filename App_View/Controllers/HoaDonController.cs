using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.HoaDonChiTietDTO;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_Data.ViewModels.ThongTinGHDTO;
using App_View.IServices;
using App_View.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static App_Data.Repositories.TrangThai;

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
        public async Task<IActionResult> DataBill(ThongTinGHDTO thongTinGHDTO)
        {
            thongTinGHDTO.IdThongTinGH = Guid.NewGuid().ToString();
            thongTinGHDTO.IdNguoiDung = _userManager.GetUserId(User);
            var listcart = (await gioHangChiTietServices.GetAllGioHang()).Where(c => c.IdNguoiDung == _userManager.GetUserId(User));
            var results = await Task.WhenAll(listcart.Select(async item =>
            {
                var sanPhamChiTiet = await _sanPhamChiTietService.GetSanPhamChiTietViewModelByKeyAsync(item.IdSanPhamCT);
                return new
                {
                    Item = item,
                    SanPhamChiTiet = sanPhamChiTiet,
                    Message = $"{sanPhamChiTiet.SanPham} màu {sanPhamChiTiet.MauSac} size {sanPhamChiTiet.KichCo} đã hết, Vui lòng chọn sản phẩm khác!"
                };
            }));

            var outOfStockProducts = results
                .Where(result => result.Item.SoLuong > result.SanPhamChiTiet.SoLuongTon)
                .Select(result => result.Message).ToList();
            if (outOfStockProducts == null)
            {
                await ThongTinGHController.CreateThongTin(thongTinGHDTO);
                return Ok(new { idThongTinGH = thongTinGHDTO.IdThongTinGH });
            }
            else
            {
                return Ok(new { quantityError = outOfStockProducts });
            }
        }
        public async Task<IActionResult> ThanhToan(HoaDonDTO hoaDonDTO)
        {
            var UserID = _userManager.GetUserId(User);
            var listcart = (await gioHangChiTietServices.GetAllGioHang()).Where(c => c.IdNguoiDung == UserID);
            var results = await Task.WhenAll(listcart.Select(async item =>
            {
                var sanPhamChiTiet = await _sanPhamChiTietService.GetSanPhamChiTietViewModelByKeyAsync(item.IdSanPhamCT);

                // Thêm điều kiện kiểm tra số lượng tồn đã hết
                if (item.SoLuong > sanPhamChiTiet.SoLuongTon && sanPhamChiTiet.SoLuongTon == 0)
                {
                    return new
                    {
                        Item = item,
                        SanPhamChiTiet = sanPhamChiTiet,
                        Message = $"{sanPhamChiTiet.SanPham} màu {sanPhamChiTiet.MauSac} size {sanPhamChiTiet.KichCo} đã hết hàng, Vui lòng chọn sản phẩm khác!"
                    };
                }
                if (item.SoLuong > sanPhamChiTiet.SoLuongTon)
                {
                    return new
                    {
                        Item = item,
                        SanPhamChiTiet = sanPhamChiTiet,
                        Message = $"Sản phẩm {sanPhamChiTiet.SanPham} màu {sanPhamChiTiet.MauSac} size {sanPhamChiTiet.KichCo} chỉ còn {sanPhamChiTiet.SoLuongTon}, Vui lòng chọn lại số lượng sản phẩm!"
                    };
                }
                if (item.TrangThaiSanPham == 1)
                {
                    return new
                    {
                        Item = item,
                        SanPhamChiTiet = sanPhamChiTiet,
                        Message = $"Số lượng sản phẩm {sanPhamChiTiet.SanPham} màu {sanPhamChiTiet.MauSac} size {sanPhamChiTiet.KichCo} đã ngừng bán, Vui lòng chọn sản phẩm khác!"
                    };
                }
                return null; // Trả về null nếu sản phẩm có sẵn để không bị lẫn vào danh sách outOfStockProducts
            }));

            var outOfStockProducts = results
                .Where(result => result != null)
                .Select(result => result.Message)
                .ToList();
            if (outOfStockProducts == null)
            {
                var hoadon = new HoaDonDTO()
                {
                    IdHoaDon = Guid.NewGuid().ToString(),
                    IdNguoiDung = UserID,
                    IdKhachHang = null,
                    IdThongTinGH = hoaDonDTO.IdThongTinGH,
                    IdVoucher = hoaDonDTO.IdVoucher,
                    NgayTao = DateTime.Now,
                    NgayShip = DateTime.Now.AddDays(2),
                    NgayNhan = DateTime.Now.AddDays(4),
                    NgayThanhToan = DateTime.Now.AddDays(4),
                    NgayGiaoDuKien = hoaDonDTO.NgayGiaoDuKien,
                    TienGiam = hoaDonDTO.TienGiam,
                    TongTien = hoaDonDTO.TongTien,
                    TienShip = hoaDonDTO.TienShip,
                    MoTa = hoaDonDTO.MoTa,
                    TrangThaiGiaoHang = (int)TrangThaiGiaoHang.ChoXacNhan,
                    TrangThaiThanhToan = (int)TrangThaiHoaDon.ChuaThanhToan
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
                        TrangThai = (int)TrangThaiHoaDonChiTiet.ChuaThanhToan
                    });
                    var sanphamupdate = new SanPhamSoLuongDTO()
                    {
                        IdChiTietSanPham = item.IdSanPhamCT,
                        SoLuong = (int)item.SoLuong
                    };
                    await gioHangChiTietServices.DeleteGioHang(item.IdGioHangChiTiet);
                    var product = await _sanPhamChiTietService.GetByKeyAsync(item.IdSanPhamCT);
                    await _sanPhamChiTietService.UpDatSoLuongAynsc(sanphamupdate);
                }
                return Ok(new { idHoaDon = hoadon.IdHoaDon });
            }
            else
            {
                return Ok(new { quantityError = outOfStockProducts });
            }

        }
        public async Task<ActionResult<HoaDonViewModel>> Order(string idHoaDon)
        {
            var order = (await hoaDonServices.GetHoaDon()).FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            return View(order);
        }
    }
}
