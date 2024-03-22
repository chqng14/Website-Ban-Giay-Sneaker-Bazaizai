using App_Data.DbContext;
using App_Data.Models;
using App_Data.ViewModels.DonHang;
using App_Data.ViewModels.DonHangChiTiet;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_View.IServices;
using App_View.Models;
using App_View.Models.Components;
using App_View.Models.Order;
using App_View.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using static App_Data.Repositories.TrangThai;

namespace App_View.Controllers
{
    [Authorize]
    public class DonHangController : Controller
    {
        private readonly HttpClient _httpclient;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly BazaizaiContext _bazaizaiContext;
        private readonly IHoaDonServices hoaDonServices;
        private ISanPhamChiTietservice _SanPhamChiTietservice;
        private IGioHangChiTietservices GioHangChiTietservices;
        private IMomoService _momoService;
        private PTThanhToanChiTietController PTThanhToanChiTietController;
        private PTThanhToanController PTThanhToanController;
        private IVnPayService _vnPayService;
        public DonHangController(HttpClient httpclient, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager, ISanPhamChiTietservice SanPhamChiTietservice, IMomoService momoService, IVnPayService vnPayService)
        {
            _httpclient = httpclient;
            _signInManager = signInManager;
            _userManager = userManager;
            _bazaizaiContext = new BazaizaiContext();
            hoaDonServices = new HoaDonServices();
            _SanPhamChiTietservice = SanPhamChiTietservice;
            GioHangChiTietservices = new GioHangChiTietservices();
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
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "NhanVien");
            if (role)
            {
                return Json(new { mess = "Vui lòng dùng tài khoản khách!" });
            }
            var listHoaDon = await hoaDonServices.GetHoaDonOnline(user.Id);
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
            if (!initialList.Any())
            {
                return Json(new { error = "Không có đơn hàng!" });

            }
            return PartialView("GetHoaDonOnline", initialList);
        }

        [HttpGet]
        public async Task<IActionResult> GetMoreHoaDonOnline(string trangThai, int page, string searchMaDonHang)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "NhanVien");
            if (role)
            {
                return Json(new { mess = "Vui lòng dùng tài khoản khách!" });
            }
            var pageSize = 4;

            var listHoaDon = await hoaDonServices.GetHoaDonOnline(user.Id);
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
            var listHoaDon = await hoaDonServices.GetHoaDonOnlineById(idHoaDon, UserID);
            return View(listHoaDon);
        }

        public async Task<IActionResult> HuyDonHang(string idHoaDon, string Lido)
        {
            var UserID = _userManager.GetUserId(User);
            var HoaDon = await hoaDonServices.GetHoaDonOnlineById(idHoaDon, UserID);
            var ngayCapNhatGanNhat = DateTime.Now;
            if (HoaDon.LoaiThanhToan == "MOMO" && HoaDon.TrangThaiGiaoHang == 1)
            {
                if (HoaDon.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.ChoXacNhan)
                {
                    await hoaDonServices.UpdateTrangThaiGiaoHangHoaDon(idHoaDon, UserID, (int)TrangThaiGiaoHang.DaHuy, Lido, ngayCapNhatGanNhat);
                    foreach (var item in HoaDon.SanPham)
                    {
                        var sanphamupdate = new SanPhamSoLuongDTO()
                        {
                            IdChiTietSanPham = item.IdSanPhamChiTiet,
                            SoLuong = -(int)item.SoLuong
                        };
                        await _SanPhamChiTietservice.UpDatSoLuongAynsc(sanphamupdate);
                    }
                }
                else
                {
                    await hoaDonServices.UpdateTrangThaiGiaoHangHoaDon(idHoaDon, UserID, (int)TrangThaiGiaoHang.ChoHuy, Lido, ngayCapNhatGanNhat);
                }
                return Ok(new { idHoaDon = idHoaDon/*, mess = mess*/ });
            }
            else
            {
                if (HoaDon.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.ChoXacNhan)
                {
                    foreach (var item in HoaDon.SanPham)
                    {
                        var sanphamupdate = new SanPhamSoLuongDTO()
                        {
                            IdChiTietSanPham = item.IdSanPhamChiTiet,
                            SoLuong = -(int)item.SoLuong
                        };
                        await _SanPhamChiTietservice.UpDatSoLuongAynsc(sanphamupdate);
                    }
                    await hoaDonServices.UpdateTrangThaiGiaoHangHoaDon(idHoaDon, UserID, (int)TrangThaiGiaoHang.DaHuy, Lido, ngayCapNhatGanNhat);
                }
                else
                {
                    await hoaDonServices.UpdateTrangThaiGiaoHangHoaDon(idHoaDon, UserID, (int)TrangThaiGiaoHang.ChoHuy, Lido, ngayCapNhatGanNhat);
                }
                return Ok(new { idHoaDon = idHoaDon });
            }
        }

        public async Task<IActionResult> ReBuy(string idHoaDon)
        {
            var UserID = _userManager.GetUserId(User);
            var listHoaDon = await hoaDonServices.GetHoaDonOnlineById(idHoaDon, UserID);
            var message = new List<string>();
            foreach (var sp in listHoaDon.SanPham)
            {
                var product = await _SanPhamChiTietservice.GetSanPhamChiTietViewModelByKeyAsync(sp.IdSanPhamChiTiet);
                var existing = (await GioHangChiTietservices.GetAllGioHang()).FirstOrDefault(x => x.IdSanPhamCT == product.IdChiTietSp && x.IdNguoiDung == UserID);
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
                    await GioHangChiTietservices.UpdateGioHang(sp.IdSanPhamChiTiet, Convert.ToInt32(existing.SoLuong), UserID);
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
                    giohang.IdNguoiDung = UserID;
                    giohang.SoLuong = sp.SoLuong;
                    giohang.GiaGoc = product.GiaBan;
                    giohang.GiaBan = product.GiaThucTe;
                    GioHangChiTietservices.CreateCartDetailDTO(giohang);
                }
            }
            if (message.Any())
            {
                return Json(new { quantityError = message });
            }
            return RedirectToAction("ShowCartUser", "GioHangChiTiets");
        }

        public async Task<IActionResult> DanhGia(string idHoaDon)
        {
            if (idHoaDon == null)
            {
                return Ok(new { mess = "Đơn hàng chưa thanh toán" });
            }
            var UserID = _userManager.GetUserId(User);
            var HoaDon = await hoaDonServices.GetHoaDonOnlineById(idHoaDon, UserID);
            return PartialView("_PatialDanhGia", HoaDon);
        }

        public async Task<IActionResult> RePay(string idHoaDon)
        {
            var UserID = _userManager.GetUserId(User);
            var HoaDon = await hoaDonServices.GetHoaDonOnlineById(idHoaDon, UserID);
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
            var url =  _vnPayService.RePaymentUrl(model, HttpContext, idHoaDon);
            var Pay = await PTThanhToanController.GetPTThanhToanByName("VnPay");
            var idPay = await PTThanhToanChiTietController.CreatePTThanhToanChiTiet(idHoaDon, Pay, tien);
            SessionServices.SetIdToSession(HttpContext.Session, "idPay", idPay);
            return url;
        }

        public async Task<IActionResult> CallBack(string? idHoaDon)
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
                if (response.VnPayResponseCode == "00")
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
