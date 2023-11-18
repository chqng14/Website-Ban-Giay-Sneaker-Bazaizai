using App_Data.DbContextt;
using App_Data.Models;
using App_Data.ViewModels.DonHang;
using App_Data.ViewModels.DonHangChiTiet;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDon;
using App_View.IServices;
using App_View.Models;
using App_View.Models.Components;
using App_View.Models.Order;
using App_View.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using static App_Data.Repositories.TrangThai;

namespace App_View.Controllers
{
    public class DonHangController : Controller
    {
        private readonly HttpClient _httpclient;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly BazaizaiContext _bazaizaiContext;
        private readonly IHoaDonServices hoaDonServices;
        private ISanPhamChiTietService _sanPhamChiTietService;
        private IGioHangChiTietServices GioHangChiTietServices;
        private IMomoService _momoService;
        private PTThanhToanChiTietController PTThanhToanChiTietController;
        private PTThanhToanController PTThanhToanController;
        private IVnPayService _vnPayService;
        public DonHangController(HttpClient httpclient, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager, ISanPhamChiTietService sanPhamChiTietService, IMomoService momoService, IVnPayService vnPayService)
        {
            _httpclient = httpclient;
            _signInManager = signInManager;
            _userManager = userManager;
            _bazaizaiContext = new BazaizaiContext();
            hoaDonServices = new HoaDonServices();
            _sanPhamChiTietService = sanPhamChiTietService;
            GioHangChiTietServices = new GioHangChiTietServices();
            _momoService = momoService;
            PTThanhToanChiTietController = new PTThanhToanChiTietController();
            PTThanhToanController = new PTThanhToanController();
            _vnPayService = vnPayService;
        }

        public IActionResult DonHangs()
        {
            return View();
        }
        public async Task<IActionResult> GetHoaDonOnline(string trangThai, string searchMaDonHang)
        {
            var UserID = _userManager.GetUserId(User);
            var listHoaDon = await hoaDonServices.GetHoaDonOnline(UserID);
            listHoaDon = listHoaDon.OrderByDescending(dh => dh.NgayTao).ToList();

            if (!string.IsNullOrEmpty(trangThai))
            {
                listHoaDon = listHoaDon.Where(dh => dh.TrangThaiGiaoHang == Convert.ToInt32(trangThai)).ToList();
            }

            if (!string.IsNullOrEmpty(searchMaDonHang))
            {
                listHoaDon = listHoaDon.Where(dh => dh.MaHoaDon.ToUpper().Contains(searchMaDonHang.ToUpper())).ToList();
            }

            var pageSize = 4;
            var initialList = listHoaDon.Take(pageSize).ToList();

            return PartialView("GetHoaDonOnline", initialList);
        }

        [HttpGet]
        public async Task<IActionResult> GetMoreHoaDonOnline(string trangThai, int page, string searchMaDonHang)
        {
            var UserID = _userManager.GetUserId(User);
            var pageSize = 4;

            var listHoaDon = await hoaDonServices.GetHoaDonOnline(UserID);
            listHoaDon = listHoaDon.OrderByDescending(dh => dh.NgayTao).ToList();

            if (!string.IsNullOrEmpty(trangThai))
            {
                listHoaDon = listHoaDon.Where(dh => dh.TrangThaiGiaoHang == Convert.ToInt32(trangThai)).ToList();
            }

            if (!string.IsNullOrEmpty(searchMaDonHang))
            {
                listHoaDon = listHoaDon.Where(dh => dh.MaHoaDon.ToUpper().Contains(searchMaDonHang.ToUpper())).ToList();
            }

            var startIndex = (page - 1) * pageSize;
            var paginatedList = listHoaDon.Skip(startIndex).Take(pageSize).ToList();

            return PartialView("GetHoaDonOnline", paginatedList);
        }

        public async Task<IActionResult> DetailHoaDonOnline(string idHoaDon)
        {
            var UserID = _userManager.GetUserId(User);
            var listHoaDon = (await hoaDonServices.GetHoaDonOnline(UserID)).FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            return View(listHoaDon);
        }

        public async Task<IActionResult> HuyDonHang(string idHoaDon, string Lido)
        {
            var UserID = _userManager.GetUserId(User);
            var HoaDon = (await hoaDonServices.GetHoaDonOnline(UserID)).FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            if (HoaDon.LoaiThanhToan == "MOMO" && HoaDon.TrangThaiThanhToan == 1)
            {
                //var mess = await ReFund(HoaDon.MaHoaDon, (double)HoaDon.TongTien, Lido);
                await hoaDonServices.UpdateTrangThaiGiaoHangHoaDon(idHoaDon, 5, Lido);
                return Ok(new { idHoaDon = idHoaDon/*, mess = mess*/ });
            }
            else
            {
                await hoaDonServices.UpdateTrangThaiGiaoHangHoaDon(idHoaDon, 5, Lido);
                return Ok(new { idHoaDon = idHoaDon });
            }
        }

        public async Task<IActionResult> ReBuy(string idHoaDon)
        {
            var IdCart = _userManager.GetUserId(User);
            var listHoaDon = (await hoaDonServices.GetHoaDonOnline(IdCart)).FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            var message = new List<string>();
            foreach (var sp in listHoaDon.SanPham)
            {
                var product = await _sanPhamChiTietService.GetSanPhamChiTietViewModelByKeyAsync(sp.IdSanPhamChiTiet);
                var existing = (await GioHangChiTietServices.GetAllGioHang()).FirstOrDefault(x => x.IdSanPhamCT == product.IdChiTietSp && x.IdNguoiDung == IdCart);
                if (existing != null)
                {
                    if (existing.SoLuong + sp.SoLuong <= product.SoLuongTon)
                    {
                        existing.SoLuong += sp.SoLuong;
                    }
                    else
                    {
                        existing.SoLuong = Convert.ToInt32(product.SoLuongTon);
                        message.Add($"{product.SanPham} màu {product.MauSac} size {product.KichCo} số lượn chỉ còn {product.SoLuongTon},Trong giỏ hàng bạn đã có!");
                    }
                    await GioHangChiTietServices.UpdateGioHang(sp.IdSanPhamChiTiet, Convert.ToInt32(existing.SoLuong), IdCart);
                }
                else if (product.TrangThai == 1)
                {
                    message.Add($"{product.SanPham} màu {product.MauSac} size {product.KichCo} đã ngừng bán!");
                }
                else if (product.SoLuongTon == 0)
                {
                    message.Add($"{product.SanPham} màu {product.MauSac} size {product.KichCo} đã hết hàng!");
                }
                else
                {
                    var giohang = new GioHangChiTietDTOCUD();
                    giohang.IdGioHangChiTiet = Guid.NewGuid().ToString();
                    giohang.IdSanPhamCT = sp.IdSanPhamChiTiet;
                    giohang.IdNguoiDung = IdCart;
                    giohang.SoLuong = sp.SoLuong;
                    giohang.GiaGoc = product.GiaBan;
                    giohang.GiaBan = product.GiaThucTe;
                    GioHangChiTietServices.CreateCartDetailDTO(giohang);
                }
            }
            if (message.Any())
            {
                return Json(new { quantityError = message });
            }
            return RedirectToAction("ShowCartUser", "GioHangChiTiets");
        }

        //public async Task<int> ReFund(string maHoaDon, double Tien, string Lido)
        //{
        //    var transID = SessionServices.GetIdFomSession(HttpContext.Session, "transID");
        //    var model = new OrderInfoModel()
        //    {
        //        OrderId = maHoaDon,
        //        Amount = Tien,
        //        description = Lido,
        //        transId = long.Parse(transID),
        //    };
        //    var response = await _momoService.Refund(model);
        //    return response.ResultCode;
        //}

        public async Task<IActionResult> RePay(string idHoaDon)
        {
            var UserID = _userManager.GetUserId(User);
            var HoaDon = (await hoaDonServices.GetHoaDonOnline(UserID)).FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            SessionServices.SetIdToSession(HttpContext.Session, "idHoaDon", idHoaDon);
            if (HoaDon.LoaiThanhToan.ToLower() == "momo")
            {
                var url = await Momo(idHoaDon, HoaDon.MaHoaDon, (double)HoaDon.TongGia);
                return Ok(new { url = url });
            }
            else
            {
                var url = await VnPay(idHoaDon, (double)HoaDon.TongGia);
                return Ok(new { url = url });
            }
        }

        public async Task<string> Momo(string idHoaDon, string mahd, double tien)
        {
            var TenNguoiNhan = _userManager.GetUserName(User);
            var model = new OrderInfoModel()
            {
                FullName = TenNguoiNhan,
                OrderId = mahd,
                OrderInfo = "Thanh toán tại Bazazai Store",
                Amount = tien,
            };
            SessionServices.SetIPNToSession(HttpContext.Session, "IPN", model);
            var response = await _momoService.RePaymentAsync(model, idHoaDon);
            var Pay = await PTThanhToanController.GetPTThanhToanByName("Momo");
            var idPay = await PTThanhToanChiTietController.CreatePTThanhToanChiTiet(idHoaDon, Pay, tien);
            SessionServices.SetIdToSession(HttpContext.Session, "idPay", idPay);
            return response.PayUrl;
        }

        public async Task<string> VnPay(string idHoaDon, double tien)
        {
            var model = new PaymentInformationModel()
            {
                Amount = tien,
                OrderDescription = "Thanh toán tại Bazazai Store",
                OrderType = "200000",
            };
            var url = await _vnPayService.RePaymentUrl(model, HttpContext, idHoaDon);
            var Pay = await PTThanhToanController.GetPTThanhToanByName("VnPay");
            var idPay = await PTThanhToanChiTietController.CreatePTThanhToanChiTiet(idHoaDon, Pay, tien);
            SessionServices.SetIdToSession(HttpContext.Session, "idPay", idPay);
            return url;
        }

        public async Task<IActionResult> CallBack(string idHoaDon)
        {
            var idpt = SessionServices.GetIdFomSession(HttpContext.Session, "idPay");
            var idHoaDonSession = SessionServices.GetIdFomSession(HttpContext.Session, "idHoaDon");
            string payment = await hoaDonServices.GetPayMent(idHoaDonSession);
            if (payment.ToUpper() == "MOMO")
            {
                OrderInfoModel orderInfoModel = SessionServices.GetIPNFomSession(HttpContext.Session, "IPN");
                var jsonresponse = await _momoService.IPN(orderInfoModel);
                if (jsonresponse.ResultCode == 0)
                {
                    //SessionServices.SetIdToSession(HttpContext.Session, "transID", Convert.ToString(jsonresponse.TransId));
                    await PTThanhToanChiTietController.Edit(idpt, (int)PTThanhToanChiTiet.DaThanhToan);
                    await hoaDonServices.UpdateTrangThaiHoaDon(idHoaDonSession, (int)TrangThaiHoaDon.DaThanhToan);
                    await hoaDonServices.UpdateNgayHoaDon(idHoaDonSession, DateTime.Now, null, null);
                }
                else
                {
                    await PTThanhToanChiTietController.Edit(idpt, (int)PTThanhToanChiTiet.ChuaThanhToan);
                    await hoaDonServices.UpdateTrangThaiHoaDon(idHoaDonSession, (int)TrangThaiHoaDon.ChuaThanhToan);
                }
            }
            else
            {
                var response = _vnPayService.PaymentExecute(Request.Query);
                if (response.Result.VnPayResponseCode == "00")
                {
                    await PTThanhToanChiTietController.Edit(idpt, (int)PTThanhToanChiTiet.DaThanhToan);
                    await hoaDonServices.UpdateTrangThaiHoaDon(idHoaDonSession, (int)TrangThaiHoaDon.DaThanhToan);
                    await hoaDonServices.UpdateNgayHoaDon(idHoaDonSession, DateTime.Now, null, null);
                }
                else
                {
                    await PTThanhToanChiTietController.Edit(idpt, (int)PTThanhToanChiTiet.ChuaThanhToan);
                    await hoaDonServices.UpdateTrangThaiHoaDon(idHoaDonSession, (int)TrangThaiHoaDon.ChuaThanhToan);
                }
            }
            return RedirectToAction("DetailHoaDonOnline", "DonHang", new { idHoaDon = idHoaDonSession });
        }
    }
}
