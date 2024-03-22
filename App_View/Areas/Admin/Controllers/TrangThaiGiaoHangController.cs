using App_Data.DbContext;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_View.IServices;
using App_View.Models.ViewModels;
using App_View.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System.Linq;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,NhanVien")]
    public class TrangThaiGiaoHangController : Controller
    {
        private readonly IVoucherNguoiDungservices _VoucherNguoiDungservices;
        private readonly IVoucherservices _Voucherservices;
        private readonly IHoaDonChiTietservices _HoaDonChiTietservices; private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IHoaDonServices _hoaDonServices;
        private readonly ISanPhamChiTietservice SanPhamChiTietservice;
        BazaizaiContext context;
        public TrangThaiGiaoHangController(ISanPhamChiTietservice SanPhamChiTietservice, IVoucherNguoiDungservices VoucherNguoiDungservices, IVoucherservices Voucherservices, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager)
        {
            _hoaDonServices = new HoaDonServices();
            context = new BazaizaiContext();
            this.SanPhamChiTietservice = SanPhamChiTietservice;
            _VoucherNguoiDungservices = VoucherNguoiDungservices;
            _Voucherservices = Voucherservices;
            _HoaDonChiTietservices = new HoaDonChiTietservices();
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult QuanLyGiaoHang()
        {
            return View();
        }
        public async Task<IActionResult> QuanLyTrangThaiGiaoHangAsync(int trangThaiHD, string search)
        {
            var lstHoaDon = (await _hoaDonServices.GetHoaDon()).ToList();
            ViewBag.NguoiDung = context.NguoiDungs.AsNoTracking().ToList();
            ViewBag.HoaDon = (await _hoaDonServices.GetAllHoaDon()).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                lstHoaDon = lstHoaDon.Where(x => x.MaHoaDon.ToUpper().Contains(search.ToUpper())).ToList();
            }
            if (trangThaiHD == 0)
            {
                lstHoaDon = lstHoaDon.Where(x => x.TrangThaiGiaoHang!=0&& x.TrangThaiGiaoHang != 5 && x.TrangThaiGiaoHang != 7).ToList();
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDon);
            }
            if (trangThaiHD == 1)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 1);
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDonOnline);
            }
            if (trangThaiHD == 2)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 2);
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDonOnline);
            }
            if (trangThaiHD == 3)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 3 );
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDonOnline);
            }
            if (trangThaiHD == 4)
            {
                var lstHoaDonOnline = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 4);
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDonOnline);
            }
            if (trangThaiHD == 5)
            {
                var lstHoaDonDaHuy = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 5).ToList();
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDonDaHuy);
            }
            if (trangThaiHD == 7)
            {
                var lstHoaDonDaHuy = lstHoaDon.Where(x => x.TrangThaiGiaoHang == 7);
                return PartialView("QuanLyTrangThaiGiaoHang", lstHoaDonDaHuy);
            }
            return PartialView("QuanLyHoaDon", lstHoaDon);
        }
        [HttpPost]
        public async Task<IActionResult> ChiTietGiaoHangAsync(string id)
        {

            var hoaDon = (await _hoaDonServices.GetHoaDon()).FirstOrDefault(x => x.IdHoaDon == id);
            ViewBag.TenNguoiNhan =hoaDon?.TenNguoiNhan;
            ViewBag.Sdt = context.ThongTinGiaoHangs.AsNoTracking().FirstOrDefault(x => x.IdThongTinGH == hoaDon.IdThongTinGH).SDT;
            var hoaDonChiTiet = context.HoaDons.FirstOrDefault(x => x.IdHoaDon == hoaDon.IdHoaDon);

            return PartialView("ChiTietGiaoHang", hoaDonChiTiet);
        }
        [HttpPost]
        public async Task<IActionResult> CapNhatTrangThaiAsync(string trangThaiGH,string id,string? lyDoHuy)
        {
            var hoaDon1 = context.HoaDons.FirstOrDefault(x => x.IdHoaDon == id);
            var hoaDonAdmin = (await _hoaDonServices.GetAllHoaDon()).FirstOrDefault(x => x.IdHoaDon == id);
			if (hoaDonAdmin.TrangThaiGiaoHang == 1 && hoaDonAdmin.TrangThaiThanhToan == 0 && (hoaDonAdmin.LoaiThanhToan == "VNPAY" || hoaDonAdmin.LoaiThanhToan == "MOMO"))
			{
				return Ok(new { thongBao = "Đơn hàng này chưa thanh toán", trangThai = false });
			}
			else
			if (Convert.ToInt32(trangThaiGH) == 2)
            {
                var ngayCapNhatGanNhat = DateTime.Now;
				var sanPhamCT = (await SanPhamChiTietservice.GetAllListSanPhamChiTietViewModelAsync()).ToList();
				var user = await _userManager.GetUserAsync(User);
                var idUser = await _userManager.GetUserIdAsync(user);
               
                var hoadonchitiet =  context.HoaDonChiTiets.Where(x=>x.IdHoaDon== id);
                
                if (hoadonchitiet.Any())
                {
					foreach(var item in hoadonchitiet)
					{
                        var spct = sanPhamCT.FirstOrDefault(x => x.IdChiTietSp == item.IdSanPhamChiTiet);
						if (spct.SoLuongTon< item.SoLuong)
						{
                            return Ok(new { thongBao = $"Sản phẩm {spct.SanPham} không đủ trong giỏ hàng", trangThai = false }
                        );
						}
					}
					var updateTrangThaiHoaDon = await _hoaDonServices.UpdateTrangThaiGiaoHangHoaDon(id, idUser, Convert.ToInt32(trangThaiGH), lyDoHuy, ngayCapNhatGanNhat);
					foreach (var item in hoadonchitiet)
                    {
                        await SanPhamChiTietservice.UpDatSoLuongAynsc(new SanPhamSoLuongDTO()
                        {
                            IdChiTietSanPham = item.IdSanPhamChiTiet,
                            SoLuong = (int)item.SoLuong,
                        });
                    }
                    return Ok(
                        new { thongBao = "Cập nhập trạng thái thành công", trangThai = true }
                        );
                }
                return Ok(new
                {
					thongBao = "Cập nhập thất bại",
					trangThai = false
				});
            }
            else
            {
                DateTime newUpdate = DateTime.Now;
                var user = await _userManager.GetUserAsync(User);
                var idUser = await _userManager.GetUserIdAsync(user);
                var hoadonchitiet = await _hoaDonServices.UpdateTrangThaiGiaoHangHoaDon(id, idUser, Convert.ToInt32(trangThaiGH), lyDoHuy, newUpdate);
                return Ok(
                        new { thongBao = "Cập nhập trạng thái thành công", trangThai = true }
                        );
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> HuyDonHangCho(string id, string lyDoHuy,int trangthai)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Ok(new
                {
                    TrangThai = false,
                });
            }
            DateTime newUpdate = DateTime.Now;
            var user = await _userManager.GetUserAsync(User);
            var idUser = await _userManager.GetUserIdAsync(user);
            var hoadonchitiet = await _hoaDonServices.UpdateTrangThaiGiaoHangHoaDon(id, idUser, trangthai, lyDoHuy, newUpdate);
            return Ok();
        }
        public async Task<IActionResult> XacNhanHuy(string id,string lyDoHuy)
        {
            var hoadonchitiet = context.HoaDonChiTiets.Where(x => x.IdHoaDon == id);
            var hoadon = context.HoaDons.FirstOrDefault(x => x.IdHoaDon == id);
            if(hoadon.TrangThaiGiaoHang==0)
            {
                hoadon.TrangThaiThanhToan = 2;
                hoadon.LiDoHuy = lyDoHuy;
            }
            else
            hoadon.TrangThaiGiaoHang = 5;
            context.Update(hoadon);
            context.SaveChanges();
            if (hoadonchitiet.Any())
            {
                foreach (var item in hoadonchitiet)
                {
                    await SanPhamChiTietservice.UpDatSoLuongAynsc(new SanPhamSoLuongDTO()
                    {
                        IdChiTietSanPham = item.IdSanPhamChiTiet,
                        SoLuong = -(int)item.SoLuong,
                    });
                }
                return Ok(
                    new { TrangThai = true, }
                    );
            }
            return Ok(new
            {
                TrangThai = false,
            });
        }
    }
}
