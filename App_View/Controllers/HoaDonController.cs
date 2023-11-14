using App_Data.DbContextt;
using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.HoaDonChiTietDTO;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_Data.ViewModels.ThongTinGHDTO;
using App_View.IServices;
using App_View.Models;
using App_View.Models.Momo;
using App_View.Models.Order;
using App_View.Services;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
		IGioHangChiTietServices gioHangChiTietServices;
		IHoaDonServices hoaDonServices;
		IHoaDonChiTietServices hoaDonChiTietServices;
		ThongTinGHController ThongTinGHController;
		PTThanhToanChiTietController PTThanhToanChiTietController;
		PTThanhToanController PTThanhToanController;
		private IVnPayService _vnPayService;
		private IEmailSender _emailSender;
		private IViewRenderService _viewRenderService;
		public HoaDonController(SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager, ISanPhamChiTietService sanPhamChiTietService, ThongTinGHController thongTinGHController, IMomoService momoService, IVnPayService vnPayService, IEmailSender emailSender, IViewRenderService viewRenderService)
		{
			_sanPhamChiTietService = sanPhamChiTietService;
			_signInManager = signInManager;
			_userManager = userManager;
			gioHangChiTietServices = new GioHangChiTietServices();
			hoaDonServices = new HoaDonServices();
			hoaDonChiTietServices = new HoaDonChiTietServices();
			ThongTinGHController = thongTinGHController;
			_momoService = momoService;
			PTThanhToanChiTietController = new PTThanhToanChiTietController();
			PTThanhToanController = new PTThanhToanController();
			_vnPayService = vnPayService;
			_emailSender = emailSender;
			_viewRenderService = viewRenderService;
		}
		#region User
		public async Task<IActionResult> DataBill(ThongTinGHDTO thongTinGHDTO)
		{
			thongTinGHDTO.IdThongTinGH = Guid.NewGuid().ToString();
			thongTinGHDTO.IdNguoiDung = _userManager.GetUserId(User);
			thongTinGHDTO.TrangThai = (int)TrangThaiThongTinGH.HoatDong;
			var listcart = (await gioHangChiTietServices.GetAllGioHang()).Where(c => c.IdNguoiDung == _userManager.GetUserId(User));
			var (quantityErrorCount, outOfStockCount, stoppedSellingCount, message) = await KiemTraGioHang(listcart);
			if (!message.Any())
			{
				await ThongTinGHController.CreateThongTinBill(thongTinGHDTO);
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
			var (quantityErrorCount, outOfStockCount, stoppedSellingCount, message) = await KiemTraGioHang(listcart);
			if (!message.Any())
			{
				var hoadon = new HoaDonDTO()
				{
					IdHoaDon = Guid.NewGuid().ToString(),
					IdNguoiDung = UserID,
					IdKhachHang = null,
					IdThongTinGH = hoaDonDTO.IdThongTinGH,
					IdVoucher = hoaDonDTO.IdVoucher,
					IdNguoiSuaGanNhat = null,
					NgayTao = DateTime.Now,
					NgayShip = null,
					NgayNhan = null,
					NgayThanhToan = null,
					NgayGiaoDuKien = hoaDonDTO.NgayGiaoDuKien,
					TienGiam = hoaDonDTO.TienGiam == null ? 0 : hoaDonDTO.TienGiam,
					TongTien = hoaDonDTO.TongTien,
					TienShip = hoaDonDTO.TienShip,
					MoTa = hoaDonDTO.MoTa,
					LiDoHuy = null,
					TrangThaiGiaoHang = (int)TrangThaiGiaoHang.ChoXacNhan,
					TrangThaiThanhToan = (int)TrangThaiHoaDon.ChuaThanhToan
				};
				var mahd = await hoaDonServices.CreateHoaDon(hoadon);
				var tien = (double)(hoadon.TongTien + hoadon.TienShip - (hoadon.TienGiam ?? 0));
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
					var sp = new List<SanPhamTest>()
					{
						new SanPhamTest()
						{
						TenSanPham = item.TenSanPham,
						TenMauSac = item.TenMauSac,
						TenKichCo = item.TenKichCo,
						SoLuong = item.SoLuong,
						GiaBan = item.GiaBan
						}
					};
					SessionServices.SetspToSession(HttpContext.Session, "sp", sp);
					await gioHangChiTietServices.DeleteGioHang(item.IdGioHangChiTiet);
					var product = await _sanPhamChiTietService.GetByKeyAsync(item.IdSanPhamCT);
					await _sanPhamChiTietService.UpDatSoLuongAynsc(sanphamupdate);
				}
				if (hoaDonDTO.LoaiThanhToan.ToLower() == "momo")
				{
					var url = await Momo(hoadon.IdHoaDon, mahd, tien);
					return Ok(new { url = url, idHoaDon = hoadon.IdHoaDon });
				}
				if (hoaDonDTO.LoaiThanhToan.ToLower() == "VnPay")
				{
					var url = await VnPay(hoadon.IdHoaDon, tien);
					return Ok(new { url = url, idHoaDon = hoadon.IdHoaDon });
				}
				else
				{
					await Cod(hoadon.IdHoaDon, tien);
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
			thongTinGHDTO.TrangThai = (int)TrangThaiThongTinGH.HoatDong;
			var listcart = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
			var (quantityErrorCount, outOfStockCount, stoppedSellingCount, message) = await KiemTraGioHang(listcart);
			if (!message.Any())
			{
				await ThongTinGHController.CreateThongTinBill(thongTinGHDTO);
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
			var (quantityErrorCount, outOfStockCount, stoppedSellingCount, message) = await KiemTraGioHang(listcart);
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
					NgayShip = null,
					NgayNhan = null,
					NgayThanhToan = null,
					NgayGiaoDuKien = hoaDonDTO.NgayGiaoDuKien,
					TienGiam = hoaDonDTO.TienGiam == null ? 0 : hoaDonDTO.TienGiam,
					TongTien = hoaDonDTO.TongTien,
					TienShip = hoaDonDTO.TienShip,
					MoTa = hoaDonDTO.MoTa,
					TrangThaiGiaoHang = (int)TrangThaiGiaoHang.ChoXacNhan,
					TrangThaiThanhToan = (int)TrangThaiHoaDon.ChuaThanhToan
				};
				var mahd = await hoaDonServices.CreateHoaDon(hoadon);
				var tien = (double)(hoadon.TongTien + hoadon.TienShip - (hoadon.TienGiam ?? 0));
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
					var product = await _sanPhamChiTietService.GetByKeyAsync(item.IdSanPhamCT);
					await _sanPhamChiTietService.UpDatSoLuongAynsc(sanphamupdate);
				}
				listcart.Clear();
				if (hoaDonDTO.LoaiThanhToan == "Momo")
				{
					var url = await Momo(hoadon.IdHoaDon, mahd, tien);
					return Ok(new { url = url, idHoaDon = hoadon.IdHoaDon });
				}
				if (hoaDonDTO.LoaiThanhToan == "VnPay")
				{
					var url = await VnPay(hoadon.IdHoaDon, tien);
					return Ok(new { url = url, idHoaDon = hoadon.IdHoaDon });
				}
				else
				{
					await Cod(hoadon.IdHoaDon, tien);
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

		#region Chung
		public async Task<ActionResult<HoaDonViewModel>> Order(string idHoaDon)
		{
			var idpt = SessionServices.GetIdFomSession(HttpContext.Session, "idPay");
			if (!string.IsNullOrEmpty(idHoaDon))
			{
				await hoaDonServices.UpdateTrangThaiHoaDon(idHoaDon, (int)TrangThaiHoaDon.ChuaThanhToan);
				await PTThanhToanChiTietController.Edit(idpt, (int)PTThanhToanChiTiet.ChuaThanhToan); string payment = await hoaDonServices.GetPayMent(idHoaDon);
				var order = (await hoaDonServices.GetHoaDon()).FirstOrDefault(c => c.IdHoaDon == idHoaDon);
				order.LoaiThanhToan = payment;
				await SendMail(order.MaHoaDon);
				return View(order);
			}
			else
			{
				var idHoaDonSession = SessionServices.GetIdFomSession(HttpContext.Session, "idHoaDon");
				string payment = await hoaDonServices.GetPayMent(idHoaDonSession);
				if (payment == "MOMO")
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
				else if (payment == "VNPAY")
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
				var order = (await hoaDonServices.GetHoaDon()).FirstOrDefault(c => c.IdHoaDon == idHoaDonSession);
				order.LoaiThanhToan = payment;
				return View(order);
			}

		}
		public async Task<string> Momo(string IdHoaDon, string MaHd, double tien)
		{
			var TenNguoiNhan = _userManager.GetUserName(User);
			var model = new OrderInfoModel()
			{
				FullName = TenNguoiNhan,
				OrderId = MaHd,
				OrderInfo = "Thanh toán tại Bazazai Store",
				Amount = tien,
			};
			SessionServices.SetIPNToSession(HttpContext.Session, "IPN", model);
			var response = await _momoService.CreatePaymentAsync(model, IdHoaDon);
			var Pay = await PTThanhToanController.GetPTThanhToanByName("Momo");
			var idPay = await PTThanhToanChiTietController.CreatePTThanhToanChiTiet(IdHoaDon, Pay, tien);
			SessionServices.SetIdToSession(HttpContext.Session, "idPay", idPay);
			SessionServices.SetIdToSession(HttpContext.Session, "idHoaDon", IdHoaDon);
			return response.PayUrl;
		}
		public async Task<IActionResult> Cod(string IdHoaDon, double tien)
		{
			var Pay = await PTThanhToanController.GetPTThanhToanByName("COD");
			var idPay = await PTThanhToanChiTietController.CreatePTThanhToanChiTiet(IdHoaDon, Pay, tien);
			SessionServices.SetIdToSession(HttpContext.Session, "idPay", idPay);
			return Ok();
		}
		public async Task<string> VnPay(string idHoaDon, double tien)
		{
			var model = new PaymentInformationModel()
			{
				Amount = tien,
				OrderDescription = "Thanh toán tại Bazazai Store",
				OrderType = "200000",
			};
			var url = await _vnPayService.CreatePaymentUrl(model, HttpContext);
			var Pay = await PTThanhToanController.GetPTThanhToanByName("VnPay");
			var idPay = await PTThanhToanChiTietController.CreatePTThanhToanChiTiet(idHoaDon, Pay, tien);
			SessionServices.SetIdToSession(HttpContext.Session, "idPay", idPay);
			SessionServices.SetIdToSession(HttpContext.Session, "idHoaDon", idHoaDon);
			return url;
		}
		private async Task<Tuple<int, int, int, List<string>>> KiemTraGioHang(IEnumerable<GioHangChiTietDTO> listcart)
		{
			var message = new List<string>();
			int quantityErrorCount = 0;
			int outOfStockCount = 0;
			int stoppedSellingCount = 0;

			foreach (var item in listcart)
			{
				var product = await _sanPhamChiTietService.GetSanPhamChiTietViewModelByKeyAsync(item.IdSanPhamCT);

				if (item.SoLuong > product.SoLuongTon && product.SoLuongTon == 0)
				{
					message.Add($"Sản phẩm {product.SanPham} màu {product.MauSac} size {product.KichCo} đã hết hàng, Vui lòng chọn sản phẩm khác!");
					outOfStockCount++;
				}
				if (item.TrangThaiSanPham == 1 || item.TrangThaiSanPham != product.TrangThai)
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

			return System.Tuple.Create(quantityErrorCount, outOfStockCount, stoppedSellingCount, message);
		}
		#endregion

		public async Task<IActionResult> SendMail(string MaHd)
		{
			var user = _userManager.GetUserId(User);
			var userKhachHang = await _userManager.FindByIdAsync(user);
			var sp = SessionServices.GetFomSession(HttpContext.Session, "sp");
			var subject = $"Đơn hàng #{MaHd} đã đặt thành công";
			string html = await _viewRenderService.RenderToStringAsync("HoaDon/Mail", sp);
			await _emailSender.SendEmailAsync(userKhachHang.Email, subject, html);
			return Ok();
		}
		//public async Task<IActionResult> CallBack()
		//{
		//    var response = _vnPayService.PaymentExecute(Request.Query);
		//    var idpt = SessionServices.GetIdFomSession(HttpContext.Session, "idPay");
		//    var idHoaDonSession = SessionServices.GetIdFomSession(HttpContext.Session, "idHoaDon");
		//    if (response.Result.VnPayResponseCode == "00")
		//    {
		//        await PTThanhToanChiTietController.Edit(idpt, (int)PTThanhToanChiTiet.DaThanhToan);
		//        await hoaDonServices.UpdateTrangThaiHoaDon(idHoaDonSession, (int)TrangThaiHoaDon.DaThanhToan);
		//        await hoaDonServices.UpdateNgayHoaDon(idHoaDonSession, DateTime.Now, null, null);
		//    }
		//    else
		//    {
		//        await PTThanhToanChiTietController.Edit(idpt, (int)PTThanhToanChiTiet.ChuaThanhToan);
		//        await hoaDonServices.UpdateTrangThaiHoaDon(idHoaDonSession, (int)TrangThaiHoaDon.ChuaThanhToan);
		//    }
		//    string payment = await hoaDonServices.GetPayMent(idHoaDonSession);
		//    var order = (await hoaDonServices.GetHoaDon()).FirstOrDefault(c => c.IdHoaDon == idHoaDonSession);
		//    order.LoaiThanhToan = payment;
		//    return View(order);
		//}
	}
}
