using App_Data.DbContextt;
using App_Data.Models;
using App_Data.ViewModels.DonHang;
using App_Data.ViewModels.DonHangChiTiet;
using App_Data.ViewModels.GioHangChiTiet;
using App_View.IServices;
using App_View.Models.Components;
using App_View.Models.Order;
using App_View.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

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

        public DonHangController(HttpClient httpclient, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager, ISanPhamChiTietService sanPhamChiTietService, IMomoService momoService)
        {
            _httpclient = httpclient;
            _signInManager = signInManager;
            _userManager = userManager;
            _bazaizaiContext = new BazaizaiContext();
            hoaDonServices = new HoaDonServices();
            _sanPhamChiTietService = sanPhamChiTietService;
            GioHangChiTietServices = new GioHangChiTietServices();
            _momoService = momoService;
            //PTThanhToanChiTietController = new PTThanhToanChiTietController();
            //PTThanhToanController = new PTThanhToanController();
        }

        public IActionResult DonHangs()
        {
            return View();
        }
        public async Task<IActionResult> GetHoaDonOnline(string trangThai)
        {
            var UserID = _userManager.GetUserId(User);
            var listHoaDon = await hoaDonServices.GetHoaDonOnline(UserID);
            listHoaDon = listHoaDon.OrderByDescending(dh => dh.NgayTao).ToList();

            if (!string.IsNullOrEmpty(trangThai))
            {
                listHoaDon = listHoaDon.Where(dh => dh.TrangThaiGiaoHang == Convert.ToInt32(trangThai)).ToList();
            }

            var pageSize = 4; // Số lượng đơn hàng muốn hiển thị ban đầu
            var initialList = listHoaDon.Take(pageSize).ToList();

            return PartialView("GetHoaDonOnline", initialList);
        }

        [HttpGet]
        public async Task<IActionResult> GetMoreHoaDonOnline(string trangThai, int page)
        {
            var UserID = _userManager.GetUserId(User);
            var pageSize = 4; // Số lượng đơn hàng muốn hiển thị trong mỗi lần tải thêm

            var listHoaDon = await hoaDonServices.GetHoaDonOnline(UserID);
            listHoaDon = listHoaDon.OrderByDescending(dh => dh.NgayTao).ToList();

            if (!string.IsNullOrEmpty(trangThai))
            {
                listHoaDon = listHoaDon.Where(dh => dh.TrangThaiGiaoHang == Convert.ToInt32(trangThai)).ToList();
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
                        var quantityCartUser = new List<string>();
                        quantityCartUser.Add("Số lượng bạn chọn đã đạt mức tối đa của sản phẩm này");
                        ViewData["QuantityCartUser"] = quantityCartUser;
                        existing.SoLuong = Convert.ToInt32(product.SoLuongTon);
                    }
                    await GioHangChiTietServices.UpdateGioHang(sp.IdSanPhamChiTiet, Convert.ToInt32(existing.SoLuong), IdCart);
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
            return RedirectToAction("ShowCartUser", "GioHangChiTiets");
        }

        public async Task<int> ReFund(string maHoaDon, double Tien, string Lido)
        {
            var transID = SessionServices.GetIdFomSession(HttpContext.Session, "transID");
            var model = new OrderInfoModel()
            {
                OrderId = maHoaDon,
                Amount = Tien,
                description = Lido,
                transId = long.Parse(transID),
            };
            var response = await _momoService.Refund(model);
            return response.ResultCode;
        }

        //public async Task<string> Pay(string idHoaDon, string maHoaDon, double tien)
        //{
        //    var TenNguoiNhan = _userManager.GetUserName(User);
        //    var model = new OrderInfoModel()
        //    {
        //        FullName = TenNguoiNhan,
        //        OrderId = maHoaDon,
        //        OrderInfo = "Thanh toán tại Bazazai Store",
        //        Amount = tien,
        //    };
        //    SessionServices.SetIPNToSession(HttpContext.Session, "IPN", model);
        //    var response = await _momoService.CreatePaymentAsync(model, idHoaDon);
        //    var Pay = await PTThanhToanController.GetPTThanhToanByName("Momo");
        //    var idPay = await PTThanhToanChiTietController.CreatePTThanhToanChiTiet(idHoaDon, Pay, tien);
        //    SessionServices.SetIdToSession(HttpContext.Session, "idPay", idPay);
        //    return response.PayUrl;
        //}
    }
}
