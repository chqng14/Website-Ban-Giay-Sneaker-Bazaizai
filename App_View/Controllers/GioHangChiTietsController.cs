using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Data.DbContext;
using App_Data.Models;

using App_Data.ViewModels.GioHangChiTiet;
using App_View.Services;
using App_View.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using DocumentFormat.OpenXml.VariantTypes;
using App_Data.ViewModels.SanPhamChiTietDTO;
using Microsoft.CodeAnalysis;
using System.Globalization;
using System.Net.Http;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Office2010.Excel;
using static App_Data.Repositories.TrangThai;

namespace App_View.Controllers
{
    public class GioHangChiTietsController : Controller
    {
        private readonly HttpClient httpClient;
        IGioHangChiTietservices GioHangChiTietservices;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        ISanPhamChiTietservice _SanPhamChiTietservice;
        IThongTinGHServices thongTinGHServices;
        IVoucherNguoiDungservices _voucherND;
        public GioHangChiTietsController(SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager, ISanPhamChiTietservice SanPhamChiTietservice, IVoucherNguoiDungservices voucherND)
        {
            httpClient = new HttpClient();
            GioHangChiTietservices = new GioHangChiTietservices();
            _SanPhamChiTietservice = SanPhamChiTietservice;
            thongTinGHServices = new ThongTinGHServices();
            _signInManager = signInManager;
            _userManager = userManager;
            _voucherND = voucherND;
        }
        public string GetIdNguoiDung()
        {
            var idNguoiDung = _userManager.GetUserId(User);
            ViewBag.idNguoiDung = idNguoiDung;
            return idNguoiDung;
        }
        public async Task<IActionResult> AddToCart(GioHangChiTietDTOCUD gioHangChiTietDTOCUD)
        {
            var IdCart = GetIdNguoiDung();
            var product = await _SanPhamChiTietservice.GetSanPhamChiTietViewModelByKeyAsync(gioHangChiTietDTOCUD.IdSanPhamCT);
            //var ListCart = (await GioHangChiTietservices.GetAllGioHang()).Where(c => c.IdNguoiDung == IdCart);
            if (IdCart != null)
            {
                var existing = (await GetGioHangChiTietDTOs()).FirstOrDefault(c => c.IdSanPhamCT == product.IdChiTietSp);
                if (existing != null)
                {
                    if (existing.SoLuong + gioHangChiTietDTOCUD.SoLuong <= product.SoLuongTon)
                    {
                        existing.SoLuong += gioHangChiTietDTOCUD.SoLuong;
                    }
                    else
                    {
                        var quantityCartUser = new List<string>();
                        quantityCartUser.Add("Số lượng bạn chọn đã đạt mức tối đa của sản phẩm này");
                        ViewData["QuantityCartUser"] = quantityCartUser;
                        existing.SoLuong = Convert.ToInt32(product.SoLuongTon);
                    }
                    await GioHangChiTietservices.UpdateGioHang(gioHangChiTietDTOCUD.IdSanPhamCT, Convert.ToInt32(existing.SoLuong), IdCart);
                }
                else
                {
                    var giohang = new GioHangChiTietDTOCUD();
                    giohang.IdGioHangChiTiet = Guid.NewGuid().ToString();
                    giohang.IdSanPhamCT = gioHangChiTietDTOCUD.IdSanPhamCT;
                    giohang.IdNguoiDung = GetIdNguoiDung();
                    giohang.SoLuong = gioHangChiTietDTOCUD.SoLuong;
                    giohang.GiaGoc = product.GiaBan;
                    giohang.GiaBan = product.GiaThucTe;
                    GioHangChiTietservices.CreateCartDetailDTO(giohang);
                }
            }
            else
            {
                var GioHangsession = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
                var existing = GioHangsession.FirstOrDefault(c => c.IdSanPhamCT == gioHangChiTietDTOCUD.IdSanPhamCT);

                if (existing != null)
                {
                    if (existing.SoLuong + gioHangChiTietDTOCUD.SoLuong <= product.SoLuongTon)
                    {
                        existing.SoLuong += gioHangChiTietDTOCUD.SoLuong;
                    }
                    else
                    {
                        TempData["quantityCartUser"] = "Số lượng bạn chọn đã đạt mức tối đa của sản phẩm này";
                        existing.SoLuong = Convert.ToInt32(product.SoLuongTon);
                    }
                    //await GioHangChiTietservices.UpdateGioHangNologin(gioHangChiTietDTOCUD.IdSanPhamCT, Convert.ToInt32(existing.SoLuong));
                }
                else
                {
                    var giohang = new GioHangChiTietDTO();
                    giohang.IdSanPhamCT = gioHangChiTietDTOCUD.IdSanPhamCT;
                    giohang.SoLuong = gioHangChiTietDTOCUD.SoLuong;
                    giohang.TenSanPham = product.SanPham;
                    giohang.TenMauSac = product.MauSac;
                    giohang.TenKichCo = product.KichCo;
                    giohang.TenThuongHieu = product.ThuongHieu;
                    giohang.LinkAnh = product.ListTenAnh;
                    giohang.GiaGoc = product.GiaBan;
                    giohang.GiaBan = product.GiaThucTe;
                    giohang.TrangThaiSanPham = product.TrangThai;
                    GioHangsession.Add(giohang);
                }
                SessionServices.SetObjToSession(HttpContext.Session, "Cart", GioHangsession);
            }
            return RedirectToAction("Details", "SanPhamChiTiets", new { id = product.IdChiTietSp });
        }

        #region User
        public async Task<IActionResult> ShowCartUser()
        {
            if (GetIdNguoiDung() == null)
            {
                return RedirectToAction("ShowCartNoLogin");
            }
            else
            {
                var giohang = await GetGioHangChiTietDTOs();
                if (giohang.Count == 0)
                {
                    return View("Empty");
                }
                else
                {
                    var (quantityErrorCount, outOfStockCount, stoppedSellingCount, message) = await KiemTraGioHang(giohang);
                    if (message.Any())
                    {
                        if (outOfStockCount > 0)
                        {
                            TempData["problemCount"] = $"Có {outOfStockCount} sản phẩm đã hết hàng.";
                        }
                        else if (stoppedSellingCount > 0)
                        {
                            TempData["problemCount"] = $"Có {stoppedSellingCount} sản phẩm đã ngừng bán.";
                        }
                        else if (quantityErrorCount > 0)
                        {
                            TempData["problemCount"] = $"Có {quantityErrorCount} sản phẩm không đủ số lượng.";
                        }
                        ViewData["QuantityErrorMessages"] = message;
                    }
                    return View(giohang);
                }
            }
        }
        public async Task<IActionResult> CheckOut()
        {
            if (GetIdNguoiDung() == null)
            {
                return RedirectToAction("CheckOutNoLogin");
            }
            else
            {
                var giohang = await GetGioHangChiTietDTOs();
                if (giohang.Count == 0)
                {
                    return View("Empty");
                }
                else
                {
                    var tongtien = giohang.Sum(c => c.GiaBan * c.SoLuong);
                    SessionServices.SetIdToSession(HttpContext.Session, "TongTien", Convert.ToString(tongtien));
                    var thongTinGH = await thongTinGHServices.GetThongTinByIdUser(GetIdNguoiDung());
                    ViewData["ThongTinGH"] = thongTinGH;
                    var voucherNguoiDung = (await _voucherND.GetAllVoucherNguoiDungByID(GetIdNguoiDung())).Where(c => c.TrangThai == (int)TrangThaiVoucherNguoiDung.KhaDung && c.LoaiHinhUuDai == 2 && c.DieuKien <= tongtien).ToList();
                    ViewData["VoucherFreeShip"] = voucherNguoiDung;
                    var voucher = (await _voucherND.GetAllVoucherNguoiDungByID(GetIdNguoiDung())).Where(c => c.TrangThai == (int)TrangThaiVoucherNguoiDung.KhaDung && c.LoaiHinhUuDai != 2 && c.DieuKien <= tongtien).OrderByDescending(c => c.LoaiHinhUuDai == 0 ? c.MucUuDai : (double)c.MucUuDai / tongtien).ToList();
                    ViewData["Voucher"] = voucher;
                    var (quantityErrorCount, outOfStockCount, stoppedSellingCount, message) = await KiemTraGioHang(giohang);
                    if (message.Any())
                    {
                        if (outOfStockCount > 0)
                        {
                            TempData["problemCount"] = $"Có {outOfStockCount} sản phẩm đã hết hàng.";
                        }
                        else if (stoppedSellingCount > 0)
                        {
                            TempData["problemCount"] = $"Có {stoppedSellingCount} sản phẩm đã ngừng bán.";
                        }
                        else if (quantityErrorCount > 0)
                        {
                            TempData["problemCount"] = $"Có {quantityErrorCount} sản phẩm không đủ số lượng.";
                        }
                        ViewData["QuantityErrorMessages"] = message;
                    }
                    return View(giohang);
                }
            }
        }
        public async Task<IActionResult> CapNhatSoLuongGioHang(string IdGioHangChiTiet, int SoLuong, string IdSanPhamChiTiet)
        {

            //var SumPrice = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:N0}đ", Convert.ToDouble((await _SanPhamChiTietservice.GetByKeyAsync(IdSanPhamChiTiet)).GiaBan * SoLuong));
            var giohang = await GetGioHangChiTietDTOs();
            var results = await Task.WhenAll(giohang.Select(async item =>
            {
                var sanPhamChiTiet = await _SanPhamChiTietservice.GetSanPhamChiTietViewModelByKeyAsync(item.IdSanPhamCT);

                if (sanPhamChiTiet.SoLuongTon == 0)
                {
                    return new
                    {
                        Item = item,
                        SanPhamChiTiet = sanPhamChiTiet,
                        Message = $"{sanPhamChiTiet.SanPham} màu {sanPhamChiTiet.MauSac} size {sanPhamChiTiet.KichCo} đã hết hàng, Vui lòng chọn sản phẩm khác!",
                        Idsanpham = sanPhamChiTiet.IdChiTietSp
                    };
                }
                if (SoLuong > sanPhamChiTiet.SoLuongTon)
                {
                    return new
                    {
                        Item = item,
                        SanPhamChiTiet = sanPhamChiTiet,
                        Message = $"Sản phẩm {sanPhamChiTiet.SanPham} màu {sanPhamChiTiet.MauSac} size {sanPhamChiTiet.KichCo} số lượng chỉ còn {sanPhamChiTiet.SoLuongTon}, Vui lòng chọn lại số lượng sản phẩm!",
                        Idsanpham = sanPhamChiTiet.IdChiTietSp
                    };
                }
                if (item.TrangThaiSanPham == 1)
                {
                    return new
                    {
                        Item = item,
                        SanPhamChiTiet = sanPhamChiTiet,
                        Message = $"Sản phẩm {sanPhamChiTiet.SanPham} màu {sanPhamChiTiet.MauSac} size {sanPhamChiTiet.KichCo} đã ngừng bán, Vui lòng chọn sản phẩm khác!",
                        Idsanpham = sanPhamChiTiet.IdChiTietSp
                    };
                }
                return null; // Trả về null nếu sản phẩm có sẵn để không bị lẫn vào danh sách outOfStockProducts
            }));

            var outOfStockProducts = results
                .Where(result => result != null)
                .Where(result => result.Idsanpham == IdSanPhamChiTiet)
                .Select(result => result.Message)
                .ToList();
            if (!outOfStockProducts.Any())
            {
                var jsonupdate = await GioHangChiTietservices.UpdateGioHang(IdSanPhamChiTiet, SoLuong, GetIdNguoiDung());
                var giohangupdate = await GetGioHangChiTietDTOs();
                double TongTien = giohangupdate.Sum(item => (double)item.GiaBan * (int)item.SoLuong);
                return Json(new { /*SumPrice = SumPrice,*/ TongTien = TongTien });
            }
            else
            {
                return Json(new { quantityError = outOfStockProducts });
            }
        }
        public async Task<List<GioHangChiTietDTO>> GetGioHangChiTietDTOs()
        {
            return (await GioHangChiTietservices.GetAllGioHang()).Where(c => c.IdNguoiDung == GetIdNguoiDung()).ToList();
        }
        public async Task<IActionResult> DeleteCart(string id)
        {
            var jsondelete = await GioHangChiTietservices.DeleteGioHang(id);
            return RedirectToAction("ShowCartUser");
        }
        public async Task<IActionResult> DeleteCartCheckOut(string id)
        {
            var jsondelete = await GioHangChiTietservices.DeleteGioHang(id);
            return RedirectToAction("CheckOut");
        }
        public async Task<IActionResult> DeleteAllCart()
        {
            List<GioHangChiTietDTO> giohang = await GetGioHangChiTietDTOs();
            foreach (var item in giohang)
            {
                var jsondelete = await GioHangChiTietservices.DeleteGioHang(item.IdGioHangChiTiet);
            }
            return RedirectToAction("ShowCartUser");
        }
        public async Task<IActionResult> DeleteAllProduct()
        {
            var giohang = await GetGioHangChiTietDTOs();
            foreach (var item in giohang)
            {
                var sanpham = await _SanPhamChiTietservice.GetSanPhamChiTietViewModelByKeyAsync(item.IdSanPhamCT);
                if (sanpham.TrangThai == 1 || sanpham.SoLuongTon == 0)
                {
                    var jsondelete = await GioHangChiTietservices.DeleteGioHang(item.IdGioHangChiTiet);
                }
            }
            return RedirectToAction("ShowCartUser");
        }
        #endregion

        #region Nologin
        public async Task<IActionResult> ShowCartNoLogin()
        {
            var GioHangsession = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
            if (GioHangsession.Count == 0)
            {
                return View("Empty");
            }
            else
            {
                var (quantityErrorCount, outOfStockCount, stoppedSellingCount, message) = await KiemTraGioHang(GioHangsession);
                if (message.Any())
                {
                    if (outOfStockCount > 0)
                    {
                        TempData["problemCount"] = $"Có {outOfStockCount} sản phẩm đã hết hàng.";
                    }
                    else if (stoppedSellingCount > 0)
                    {
                        TempData["problemCount"] = $"Có {stoppedSellingCount} sản phẩm đã ngừng bán.";
                    }
                    else if (quantityErrorCount > 0)
                    {
                        TempData["problemCount"] = $"Có {quantityErrorCount} sản phẩm không đủ số lượng.";
                    }
                    ViewData["QuantityErrorMessages"] = message;
                }
                return View(GioHangsession);
            }
        }
        public async Task<IActionResult> CheckOutNoLogin()
        {
            var GioHangsession = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
            if (GioHangsession.Count == 0)
            {
                return View("Empty");
            }
            else
            {
                var (quantityErrorCount, outOfStockCount, stoppedSellingCount, message) = await KiemTraGioHang(GioHangsession);
                if (message.Any())
                {
                    if (outOfStockCount > 0)
                    {
                        TempData["problemCount"] = $"Có {outOfStockCount} sản phẩm đã hết hàng.";
                    }
                    else if (stoppedSellingCount > 0)
                    {
                        TempData["problemCount"] = $"Có {stoppedSellingCount} sản phẩm đã ngừng bán.";
                    }
                    else if (quantityErrorCount > 0)
                    {
                        TempData["problemCount"] = $"Có {quantityErrorCount} sản phẩm không đủ số lượng.";
                    }
                    ViewData["QuantityErrorMessages"] = message;
                }
                return View(GioHangsession);
            }
        }
        public async Task<IActionResult> CapNhatSoLuongGioHangNologin(string IdGioHangChiTiet, int SoLuong, string IdSanPhamChiTiet)
        {

            var GioHangsession = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
            var results = await Task.WhenAll(GioHangsession.Select(async item =>
            {
                var sanPhamChiTiet = await _SanPhamChiTietservice.GetSanPhamChiTietViewModelByKeyAsync(item.IdSanPhamCT);

                // Thêm điều kiện kiểm tra số lượng tồn đã hết
                if (item.SoLuong + SoLuong > sanPhamChiTiet.SoLuongTon && sanPhamChiTiet.SoLuongTon == 0)
                {
                    return new
                    {
                        Item = item,
                        SanPhamChiTiet = sanPhamChiTiet,
                        Message = $"{sanPhamChiTiet.SanPham} màu {sanPhamChiTiet.MauSac} size {sanPhamChiTiet.KichCo} đã hết hàng, Vui lòng chọn sản phẩm khác!",
                        Idsanpham = sanPhamChiTiet.IdChiTietSp
                    };
                }
                if (item.SoLuong + SoLuong > sanPhamChiTiet.SoLuongTon)
                {
                    return new
                    {
                        Item = item,
                        SanPhamChiTiet = sanPhamChiTiet,
                        Message = $"Sản phẩm {sanPhamChiTiet.SanPham} màu {sanPhamChiTiet.MauSac} size {sanPhamChiTiet.KichCo} số lượng chỉ còn {sanPhamChiTiet.SoLuongTon}, Vui lòng chọn lại số lượng sản phẩm!",
                        Idsanpham = sanPhamChiTiet.IdChiTietSp
                    };
                }
                if (item.TrangThaiSanPham != sanPhamChiTiet.TrangThai)
                {
                    return new
                    {
                        Item = item,
                        SanPhamChiTiet = sanPhamChiTiet,
                        Message = $"Sản phẩm {sanPhamChiTiet.SanPham} màu {sanPhamChiTiet.MauSac} size {sanPhamChiTiet.KichCo} đã ngừng bán, Vui lòng chọn sản phẩm khác!",
                        Idsanpham = sanPhamChiTiet.IdChiTietSp
                    };
                }
                return null; // Trả về null nếu sản phẩm có sẵn để không bị lẫn vào danh sách outOfStockProducts
            }));

            var outOfStockProducts = results
                .Where(result => result != null)
                .Where(result => result.Idsanpham == IdSanPhamChiTiet)
                .Select(result => result.Message)
                .ToList();
            if (!outOfStockProducts.Any())
            {
                var existingProduct = GioHangsession.FirstOrDefault(x => x.IdSanPhamCT == IdSanPhamChiTiet);
                //var a = GioHangsession.Find(c => c.IdSanPhamCT == IdSanPhamChiTiet);
                existingProduct.SoLuong = SoLuong;
                double TongTien = 0;
                foreach (var item in GioHangsession)
                {
                    TongTien += (double)item.GiaBan * (int)item.SoLuong;
                }
                SessionServices.SetObjToSession(HttpContext.Session, "Cart", GioHangsession);
                return Json(new { /*SumPrice = SumPrice,*/ TongTien = TongTien });
            }
            else
            {
                return Json(new { quantityError = outOfStockProducts });
            }

        }
        public async Task<IActionResult> DeleteCartNoLogin(string id)
        {
            var GioHangsession = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
            var prodct = GioHangsession.FirstOrDefault(c => c.IdSanPhamCT == id);
            GioHangsession.Remove(prodct);
            SessionServices.SetObjToSession(HttpContext.Session, "Cart", GioHangsession);
            return RedirectToAction("ShowCartNoLogin");
        }
        public async Task<IActionResult> DeleteCartCheckOutNoLogin(string id)
        {
            var GioHangsession = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
            var prodct = GioHangsession.FirstOrDefault(c => c.IdSanPhamCT == id);
            GioHangsession.Remove(prodct);
            SessionServices.SetObjToSession(HttpContext.Session, "Cart", GioHangsession);
            return RedirectToAction("CheckOutNoLogin");
        }
        public async Task<IActionResult> DeleteAllCartNoLogin()
        {
            var GioHangsession = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
            GioHangsession.Clear();
            SessionServices.SetObjToSession(HttpContext.Session, "Cart", GioHangsession);
            return RedirectToAction("ShowCartNoLogin");
        }
        #endregion

        private async Task<Tuple<int, int, int, List<string>>> KiemTraGioHang(IEnumerable<GioHangChiTietDTO> listcart)
        {
            var message = new List<string>();
            int quantityErrorCount = 0;
            int outOfStockCount = 0;
            int stoppedSellingCount = 0;

            foreach (var item in listcart)
            {
                var product = await _SanPhamChiTietservice.GetSanPhamChiTietViewModelByKeyAsync(item.IdSanPhamCT);

                if (item.SoLuong > product.SoLuongTon && product.SoLuongTon == 0)
                {
                    message.Add($"Sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} đã hết hàng, Vui lòng chọn sản phẩm khác!");
                    outOfStockCount++;
                }
                else if (item.TrangThaiSanPham == 1 || item.TrangThaiSanPham != product.TrangThai)
                {
                    message.Add($"Sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} đã ngừng bán, Vui lòng chọn sản phẩm khác!");
                    stoppedSellingCount++;
                }
                else if (item.SoLuong > product.SoLuongTon)
                {
                    message.Add($"Số lượng sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} chỉ còn {product.SoLuongTon}, Vui lòng chọn lại số lượng!");
                    quantityErrorCount++;
                }
            }

            return System.Tuple.Create(quantityErrorCount, outOfStockCount, stoppedSellingCount, message);
        }

        public async Task<IActionResult> GetGioHangMiniModel()
        {
            var idNguoiDung = GetIdNguoiDung();
            var data = new List<SanPhamGioHangViewModel>();
            if (idNguoiDung != null)
            {
                data = await httpClient.GetFromJsonAsync<List<SanPhamGioHangViewModel>>($"https://localhost:7038/api/GioHangChiTiet/Get-List-SanPhamGioHangVM/{idNguoiDung}");
            }
            else
            {
                var GioHangsession = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
                data = GioHangsession.Select(gh => new SanPhamGioHangViewModel()
                {
                    Anh = gh.LinkAnh.OrderBy(item => item).ToList().FirstOrDefault(),
                    GiaSanPham = Convert.ToDouble(gh.GiaBan),
                    IdSanPhamChiTiet = gh.IdSanPhamCT.ToString(),
                    SoLuong = Convert.ToInt32(gh.SoLuong),
                    TenSanPham = gh.TenSanPham + " " + gh.TenMauSac + " " + gh.TenKichCo,

                }).ToList();
            }
            return ViewComponent("Cart");
        }

        public async Task<IActionResult> DeleteLineCartMini(string id)
        {
            var idNguoiDung = GetIdNguoiDung();
            if (idNguoiDung != null)
            {
                await GioHangChiTietservices.DeleteGioHang(id);
            }
            else
            {
                var GioHangsession = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
                var prodct = GioHangsession.FirstOrDefault(c => c.IdSanPhamCT == id);
                GioHangsession.Remove(prodct);
                SessionServices.SetObjToSession(HttpContext.Session, "Cart", GioHangsession);
            }
            return ViewComponent("Cart");
        }
    }
}
