using App_Data.DbContextt;
using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.HoaDonChiTietDTO;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_Data.ViewModels.ThongTinGHDTO;
using App_View.IServices;
using App_View.Models.Momo;
using App_View.Models.Order;
using App_View.Services;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;
using static App_Data.Repositories.TrangThai;
using static Google.Apis.Requests.BatchRequest;

namespace App_View.Controllers
{
    public class HoaDonController : Controller
    {
        private IMomoService _momoService;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        ISanPhamChiTietService _sanPhamChiTietService;
        IThongTinGHServices thongTinGHServices;
        IGioHangChiTietServices gioHangChiTietServices;
        IHoaDonServices hoaDonServices;
        IHoaDonChiTietServices hoaDonChiTietServices;
        ThongTinGHController ThongTinGHController;
        private readonly GioHangChiTietsController _GioHangChiTietsController;
        private readonly IConfiguration _configuration;
        PTThanhToanChiTietController PTThanhToanChiTietController;
        PTThanhToanController PTThanhToanController;
        public HoaDonController(SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager, ISanPhamChiTietService sanPhamChiTietService, ThongTinGHController thongTinGHController, GioHangChiTietsController gioHangChiTietsController, IMomoService momoService)
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
            _momoService = momoService;
            PTThanhToanChiTietController = new PTThanhToanChiTietController();
            PTThanhToanController = new PTThanhToanController();
        }
        #region User
        public async Task<IActionResult> DataBill(ThongTinGHDTO thongTinGHDTO)
        {
            thongTinGHDTO.IdThongTinGH = Guid.NewGuid().ToString();
            thongTinGHDTO.IdNguoiDung = _userManager.GetUserId(User);
            var listcart = (await gioHangChiTietServices.GetAllGioHang()).Where(c => c.IdNguoiDung == _userManager.GetUserId(User));
            var message = new List<string>();
            int quantityErrorCount = 0;
            int outOfStockCount = 0;
            int stoppedSellingCount = 0;
            foreach (var item in listcart)
            {
                var product = await _sanPhamChiTietService.GetSanPhamChiTietViewModelByKeyAsync(item.IdSanPhamCT);
                if (item.SoLuong > product.SoLuongTon && product.SoLuongTon == 0)
                {
                    message.Add($"Sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} đã hết hàng , Vui lòng chọn sản phẩm khác!");
                    outOfStockCount++;
                }
                if (item.TrangThaiSanPham != product.TrangThai)
                {
                    message.Add($"Sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} đã ngừng bán, Vui lòng chọn sản phẩm khác!");
                    stoppedSellingCount++;
                }
                if (item.SoLuong > product.SoLuongTon)
                {
                    message.Add($"Số lượng sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} chỉ còn {product.SoLuongTon}, Vui lòng chọn lại số lượng!");
                    quantityErrorCount++;
                }
            }
            if (!message.Any())
            {
                await ThongTinGHController.CreateThongTin(thongTinGHDTO);
                return Ok(new { idThongTinGH = thongTinGHDTO.IdThongTinGH });
            }
            else
            {
                if (outOfStockCount > 0)
                {
                    return Ok(new { title = $"Có {outOfStockCount} sản phẩm đã hết hàng.", message = message });
                }
                else if (stoppedSellingCount > 0)
                {
                    return Ok(new { title = $"Có {stoppedSellingCount} sản phẩm đã ngừng bán.", message = message });
                }
                else if (quantityErrorCount > 0)
                {
                    return Ok(new { title = $"Có {quantityErrorCount} sản phẩm không đủ số lượng.", message = message });
                }
                else
                {
                    return Ok(new { title = "Có lỗi xảy ra." });
                }
            }
        }
        public async Task<IActionResult> ThanhToan(HoaDonDTO hoaDonDTO)
        {
            var UserID = _userManager.GetUserId(User);
            var listcart = (await gioHangChiTietServices.GetAllGioHang()).Where(c => c.IdNguoiDung == UserID);
            var message = new List<string>();
            int quantityErrorCount = 0;
            int outOfStockCount = 0;
            int stoppedSellingCount = 0;
            foreach (var item in listcart)
            {
                var product = await _sanPhamChiTietService.GetSanPhamChiTietViewModelByKeyAsync(item.IdSanPhamCT);
                if (item.SoLuong > product.SoLuongTon && product.SoLuongTon == 0)
                {
                    message.Add($"Sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} đã hết hàng , Vui lòng chọn sản phẩm khác!");
                    outOfStockCount++;
                }
                if (item.TrangThaiSanPham != product.TrangThai)
                {
                    message.Add($"Sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} đã ngừng bán, Vui lòng chọn sản phẩm khác!");
                    stoppedSellingCount++;
                }
                if (item.SoLuong > product.SoLuongTon)
                {
                    message.Add($"Số lượng sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} chỉ còn {product.SoLuongTon}, Vui lòng chọn lại số lượng!");
                    quantityErrorCount++;
                }
            }
            if (!message.Any())
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
                    TienGiam = hoaDonDTO.TienGiam == null ? 0 : hoaDonDTO.TienGiam,
                    TongTien = hoaDonDTO.TongTien,
                    TienShip = hoaDonDTO.TienShip,
                    MoTa = hoaDonDTO.MoTa,
                    TrangThaiGiaoHang = (int)TrangThaiGiaoHang.ChoXacNhan,
                    TrangThaiThanhToan = (int)TrangThaiHoaDon.ChuaThanhToan
                };
                var mahd = await hoaDonServices.CreateHoaDon(hoadon);
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
                if (hoaDonDTO.LoaiThanhToan == "Momo")
                {
                    var tien = (double)(hoadon.TongTien + hoadon.TienShip - (hoadon.TienGiam == null ? 0 : hoadon.TienGiam));
                    var TenNguoiNhan = _userManager.GetUserName(User);
                    var model = new OrderInfoModel()
                    {
                        FullName = TenNguoiNhan,
                        OrderId = mahd,
                        OrderInfo = "Thanh toán tại Bazazai Store",
                        Amount = tien,
                    };
                    SessionServices.SetIPNToSession(HttpContext.Session, "IPN", model);
                    SessionServices.SetIdToSession(HttpContext.Session, "idhd", hoadon.IdHoaDon);
                    var response = await _momoService.CreatePaymentAsync(model, hoadon.IdHoaDon);
                    var Pay = await PTThanhToanController.GetPTThanhToanByName("Momo");
                    var idPay = await PTThanhToanChiTietController.CreatePTThanhToanChiTiet(hoadon.IdHoaDon, Pay, tien);
                    SessionServices.SetIdToSession(HttpContext.Session, "idPay", idPay);
                    return Ok(new { url = response.PayUrl, idHoaDon = hoadon.IdHoaDon });
                }
                else
                {
                    return Ok(new { idHoaDon = hoadon.IdHoaDon });
                }
            }
            else
            {
                if (outOfStockCount > 0)
                {
                    return Ok(new { title = $"Có {outOfStockCount} sản phẩm đã hết hàng.", message = message });
                }
                else if (stoppedSellingCount > 0)
                {
                    return Ok(new { title = $"Có {stoppedSellingCount} sản phẩm đã ngừng bán.", message = message });
                }
                else if (quantityErrorCount > 0)
                {
                    return Ok(new { title = $"Có {quantityErrorCount} sản phẩm không đủ số lượng.", message = message });
                }
                else
                {
                    return Ok(new { title = "Có lỗi xảy ra." });
                }
            }
        }
        #endregion

        #region Nologin
        public async Task<IActionResult> DataBillNologin(ThongTinGHDTO thongTinGHDTO)
        {
            thongTinGHDTO.IdThongTinGH = Guid.NewGuid().ToString();
            var listcart = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
            var message = new List<string>();
            int quantityErrorCount = 0;
            int outOfStockCount = 0;
            int stoppedSellingCount = 0;
            foreach (var item in listcart)
            {
                var product = await _sanPhamChiTietService.GetSanPhamChiTietViewModelByKeyAsync(item.IdSanPhamCT);
                if (item.SoLuong > product.SoLuongTon && product.SoLuongTon == 0)
                {
                    message.Add($"Sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} đã hết hàng , Vui lòng chọn sản phẩm khác!");
                    outOfStockCount++;
                }
                if (item.TrangThaiSanPham != product.TrangThai)
                {
                    message.Add($"Sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} đã ngừng bán, Vui lòng chọn sản phẩm khác!");
                    stoppedSellingCount++;
                }
                if (item.SoLuong > product.SoLuongTon)
                {
                    message.Add($"Số lượng sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} chỉ còn {product.SoLuongTon}, Vui lòng chọn lại số lượng!");
                    quantityErrorCount++;
                }
            }
            if (!message.Any())
            {
                await ThongTinGHController.CreateThongTin(thongTinGHDTO);
                return Ok(new { idThongTinGH = thongTinGHDTO.IdThongTinGH });
            }
            else
            {
                if (outOfStockCount > 0)
                {
                    return Ok(new { title = $"Có {outOfStockCount} sản phẩm đã hết hàng.", message = message });
                }
                else if (stoppedSellingCount > 0)
                {
                    return Ok(new { title = $"Có {stoppedSellingCount} sản phẩm đã ngừng bán.", message = message });
                }
                else if (quantityErrorCount > 0)
                {
                    return Ok(new { title = $"Có {quantityErrorCount} sản phẩm không đủ số lượng.", message = message });
                }
                else
                {
                    return Ok(new { title = "Có lỗi xảy ra." });
                }
            }
        }
        public async Task<IActionResult> ThanhToanNologin(HoaDonDTO hoaDonDTO)
        {
            var listcart = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
            var message = new List<string>();
            int quantityErrorCount = 0;
            int outOfStockCount = 0;
            int stoppedSellingCount = 0;
            foreach (var item in listcart)
            {
                var product = await _sanPhamChiTietService.GetSanPhamChiTietViewModelByKeyAsync(item.IdSanPhamCT);
                if (item.SoLuong > product.SoLuongTon && product.SoLuongTon == 0)
                {
                    message.Add($"Sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} đã hết hàng , Vui lòng chọn sản phẩm khác!");
                    outOfStockCount++;
                }
                if (item.TrangThaiSanPham != product.TrangThai)
                {
                    message.Add($"Sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} đã ngừng bán, Vui lòng chọn sản phẩm khác!");
                    stoppedSellingCount++;
                }
                if (item.SoLuong > product.SoLuongTon)
                {
                    message.Add($"Số lượng sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} chỉ còn {product.SoLuongTon}, Vui lòng chọn lại số lượng!");
                    quantityErrorCount++;
                }
            }
            if (!message.Any())
            {
                var hoadon = new HoaDonDTO()
                {
                    IdHoaDon = Guid.NewGuid().ToString(),
                    IdNguoiDung = null,
                    IdKhachHang = null,
                    IdThongTinGH = hoaDonDTO.IdThongTinGH,
                    IdVoucher = hoaDonDTO.IdVoucher,
                    NgayTao = DateTime.Now,
                    NgayShip = DateTime.Now.AddDays(2),
                    NgayNhan = DateTime.Now.AddDays(4),
                    NgayThanhToan = DateTime.Now.AddDays(4),
                    NgayGiaoDuKien = hoaDonDTO.NgayGiaoDuKien,
                    TienGiam = hoaDonDTO.TienGiam == null ? 0 : hoaDonDTO.TienGiam,
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
                    listcart.Clear();
                    var product = await _sanPhamChiTietService.GetByKeyAsync(item.IdSanPhamCT);
                    await _sanPhamChiTietService.UpDatSoLuongAynsc(sanphamupdate);
                }
                return Ok(new { idHoaDon = hoadon.IdHoaDon });
            }
            else
            {
                if (outOfStockCount > 0)
                {
                    return Ok(new { title = $"Có {outOfStockCount} sản phẩm đã hết hàng.", message = message });
                }
                else if (stoppedSellingCount > 0)
                {
                    return Ok(new { title = $"Có {stoppedSellingCount} sản phẩm đã ngừng bán.", message = message });
                }
                else if (quantityErrorCount > 0)
                {
                    return Ok(new { title = $"Có {quantityErrorCount} sản phẩm không đủ số lượng.", message = message });
                }
                else
                {
                    return Ok(new { title = "Có lỗi xảy ra." });
                }
            }
        }
        #endregion
        public async Task<ActionResult<HoaDonViewModel>> Order(string idHoaDon)
        {
            if (!string.IsNullOrEmpty(idHoaDon))
            {
                var order = (await hoaDonServices.GetHoaDon()).FirstOrDefault(c => c.IdHoaDon == idHoaDon);
                return View(order);
            }
            else
            {
                OrderInfoModel orderInfoModel = SessionServices.GetIPNFomSession(HttpContext.Session, "IPN");
                var idhd = SessionServices.GetIdFomSession(HttpContext.Session, "idhd");
                var idpt = SessionServices.GetIdFomSession(HttpContext.Session, "idPay");
                var jsonresponse = await _momoService.IPN(orderInfoModel);
                //var order1 = await OrderIPN();
                if (jsonresponse.ResultCode == 0)
                {
                    await PTThanhToanChiTietController.Edit(idpt, (int)PTThanhToanChiTiet.DaThanhToan);
                    await hoaDonServices.UpdateHoaDon(idhd, (int)TrangThaiHoaDon.DaThanhToan);
                    await hoaDonChiTietServices.UpdateHoaDonChiTiet(idhd, (int)TrangThaiHoaDonChiTiet.DaThanhToan);
                    var order = (await hoaDonServices.GetHoaDon()).FirstOrDefault(c => c.IdHoaDon == idhd);
                    //HttpContext.Session.Remove("idPay");
                    //HttpContext.Session.Remove("idhd");
                    //HttpContext.Session.Remove("IPN");
                    return View(order);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        //public async Task<int> OrderIPN()
        //{
        //    string id = SessionServices.GetIdFomSession(HttpContext.Session, "id");
        //    OrderInfoModel orderInfoModel = SessionServices.GetIPNFomSession(HttpContext.Session, "IPN");
        //    var jsonresponse = await _momoService.IPN(orderInfoModel);
        //    var test = jsonresponse.ResultCode;
        //    return test;
        //}
        //[HttpPost]
        //public async Task<IActionResult> CreatePaymentUrl(HoaDonDTO hoaDonDTO)
        //{
        //    var TenNguoiNhan = _userManager.GetUserName(User);
        //    var model = new OrderInfoModel()
        //    {
        //        FullName = TenNguoiNhan,
        //        OrderId = hoaDonDTO.IdHoaDon,
        //        OrderInfo = "Thanh toán tại Bazazai Store",
        //        Amount = (double)(hoaDonDTO.TongTien + hoaDonDTO.TienShip - hoaDonDTO.TienGiam == null ? 0 : hoaDonDTO.TienGiam),
        //    };
        //    var response = await _momoService.CreatePaymentAsync(model, hoaDonDTO.IdHoaDon);
        //    return Ok(new { url = response.PayUrl });
        //}
    }
}
