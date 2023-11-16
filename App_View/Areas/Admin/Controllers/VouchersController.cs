using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Data.DbContextt;
using App_Data.Models;
using App_View.IServices;
using App_View.Services;
using App_Data.ViewModels.Voucher;
using App_Data.ViewModels.SanPhamChiTietDTO;
using AutoMapper;
using App_Data.Repositories;
using static App_Data.Repositories.TrangThai;
using Microsoft.AspNetCore.Identity;
using App_Data.ViewModels.VoucherNguoiDung;
using System.Net.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using DocumentFormat.OpenXml.Spreadsheet;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VouchersController : Controller
    {
        private readonly BazaizaiContext _context;
        private readonly IVoucherServices _voucherSV;
        private readonly IVoucherNguoiDungServices _voucherND;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IEmailSender _emailSender;
        public VouchersController(IVoucherServices voucherServices, IVoucherNguoiDungServices voucherNDServices, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager, IEmailSender emailSender)
        {
            _voucherND = voucherNDServices;
            _voucherSV = voucherServices;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _context = new BazaizaiContext();
        }
        #region Online

        public async Task<IActionResult> ShowVoucher(int? trangThai)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FilterVoucherByStatus(int? trangThai)
        {
            var lstVoucher = (await _voucherSV.GetAllVoucher())
               .Where(c => c.TrangThai >= 0 && c.TrangThai <= 3).OrderByDescending(c => c.NgayTao).ToList();
            if (trangThai != null)
            {
                lstVoucher = lstVoucher.Where(c => c.TrangThai == trangThai).OrderByDescending(c => c.NgayTao).ToList();
            }

            return PartialView("_VoucherPartial", lstVoucher);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VoucherDTO voucherDTO)
        {
            if (ModelState.IsValid)
            {
                if (await _voucherSV.CreateVoucher(voucherDTO))
                {
                    return Ok(new { message = " Thêm mới thành công" });
                }
            }
            return Ok(new { error = "Thêm voucher thất bại" });

        }

        public async Task<ActionResult> Edit(string id)
        {
            var Voucher = await _voucherSV.GetVoucherDTOById(id);
            return View(Voucher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VoucherDTO voucherDTO)
        {
            if (ModelState.IsValid)
            {
                if (await _voucherSV.UpdateVoucher(voucherDTO))
                {
                    return RedirectToAction("ShowVoucher");
                }
            }
            return View();
        }

        public async Task<ActionResult> Delete(string id)
        {
            await _voucherSV.DeleteVoucher(id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteVoucherWithList(List<string> voucherIds)
        {
            if (voucherIds.Any())
            {
                if (await _voucherSV.DeleteVoucherWithList(voucherIds) == true)
                {
                    return Ok(true);
                }
                return Ok(false);
            }
            return Ok(false);
        }
        public async Task<ActionResult> RestoreVoucherWithList(List<string> voucherIds)
        {
            if (voucherIds.Any())
            {
                await _voucherSV.RestoreVoucherWithList(voucherIds);
                return Ok(true);
            }
            return Ok(false);
        }

        public async Task<ActionResult> GiveVouchersToUsers(string? maVoucher)
        {
            ViewBag.MaVoucher = maVoucher;

            var lstUser = await _userManager.Users.ToListAsync();
            if (lstUser.Any())
            {
                ViewBag.User = lstUser;
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GiveVouchersToUsers([FromBody] AddVoucherRequestDTO addVoucherRequestDTO)
        {

            if (addVoucherRequestDTO.MaVoucher == null)
            {
                return Ok(false);
            }
            if (addVoucherRequestDTO.UserId.Any())
            {
                var ketQuaThemVoucher = await _voucherND.AddVoucherNguoiDungTuAdmin(addVoucherRequestDTO);

                if (ketQuaThemVoucher == "Tặng voucher thành công")
                {
                    foreach (var userId in addVoucherRequestDTO.UserId)
                    {
                        var userKhachHang = await _userManager.FindByIdAsync(userId);

                        var subject = "Bạn đã nhận được Voucher từ Bazaizai Store";
                        var body = "Chào bạn,\n\n" +
                                   "Chúng tôi có một voucher đặc biệt dành cho bạn vào ngày hôm nay:\n" +
                                   "Nhanh tay đến trang web của chúng tôi để sử dụng nó.\n" +
                                   "Đừng bỏ lỡ cơ hội này!\n\n" +
                                   "Trân trọng,\n" +
                                   "Nhóm hỗ trợ của chúng tôi";
                        string html = "<p>Hot! Khuyến mại 100% cho tất cả khách hàng trong ngày 2/11/2023</p>";
                        html += "<img src='/images/logo.jpg' alt='Hình ảnh logo' />";

                        await _emailSender.SendEmailAsync(userKhachHang.Email, subject, body + html);
                    }

                    return Ok(true);
                }

            }

            return Ok(false);
        }
        [HttpPost]
        public async Task<IActionResult> GiveVoucherForNewUser(string MaVoucher)
        {
            var ketQuaThemVoucher = await _voucherND.TangVoucherNguoiDungMoi(MaVoucher);
            if (ketQuaThemVoucher == true)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }
        #endregion
        #region TaiQuay
        //Tại quầy
        //[Description("Hoạt động của tại quầy")]
        //HoatDongTaiQuay = 6,
        //    [Description("Không hoạt động của tại quầy")]
        //KhongHoatDongTaiQuay = 7,
        //    [Description("Chưa hoạt động của tại quầy")]
        //ChuaHoatDongTaiQuay = 8,
        //    [Description("Đã huỷ cứng")]
        //DaHuyTaiQuay = 9
        public async Task<IActionResult> ShowVoucherTaiQuay(int? trangThai)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FilterVoucherByStatusTaiQuay(int? trangThai)
        {
            var lstVoucher = (await _voucherSV.GetAllVoucher())
                .Where(c => c.TrangThai >= 6 && c.TrangThai <= 9).OrderByDescending(c => c.NgayTao).ToList();

            if (trangThai != null)
            {
                lstVoucher = lstVoucher.Where(c => c.TrangThai == trangThai).OrderByDescending(c => c.NgayTao).ToList();
            }

            return PartialView("_VoucherPartialTaiQuay", lstVoucher);
        }
        public IActionResult CreateTaiQuay()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaiQuay(VoucherDTO voucherDTO)
        {
            if (ModelState.IsValid)
            {
                if (await _voucherSV.CreateTaiQuay(voucherDTO))
                {
                    return Ok(new { message = " Thêm mới thành công" });
                }
            }
            return Ok(new { error = "Thêm voucher thất bại" });

        }
        public async Task<ActionResult> EditTaiQuay(string id)
        {
            var Voucher = await _voucherSV.GetVoucherDTOById(id);
            return View(Voucher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTaiQuay(VoucherDTO voucherDTO)
        {
            if (ModelState.IsValid)
            {
                if (await _voucherSV.UpdateTaiQuay(voucherDTO))
                {
                    return RedirectToAction("ShowVoucherTaiQuay");
                }
            }
            return View();
        }

        public async Task<ActionResult> DeleteTaiQuay(string id)
        {
            await _voucherSV.DeleteTaiQuay(id);
            return RedirectToAction("ShowVoucherTaiQuay");
        }

        public async Task<ActionResult> DeleteVoucherWithListTaiQuay(List<string> voucherIds)
        {
            if (voucherIds.Any())
            {
                if (await _voucherSV.DeleteVoucherWithListTaiQuay(voucherIds) == true)
                {
                    return Ok(true);
                }
                return Ok(false);
            }
            return Ok(false);
        }
        public async Task<ActionResult> RestoreVoucherWithListTaiQuay(List<string> voucherIds)
        {
            if (voucherIds.Any())
            {
                await _voucherSV.RestoreVoucherWithListTaiQuay(voucherIds);
                return Ok(true);
            }
            return Ok(false);
        }
        [HttpPost]
        public async Task<IActionResult> InVoucherTaiQuay(string idVoucher, int soLuong)
        {
            var IdAdmin = await _userManager.FindByEmailAsync("adminhehehe@gmail.com");
            var VoucherKhaDung = await _voucherSV.GetVoucherDTOById(idVoucher);
            if (IdAdmin == null || soLuong <= 0|| VoucherKhaDung == null)
            {
                return Ok(false);
            }
            else if (await _voucherSV.AddVoucherCungBanTaiQuay(idVoucher, IdAdmin.Id, soLuong))
            {
                return Ok(true);
            }
            return Ok(false);
        }

        public async Task<IActionResult> ShowVoucherTaiQuayDaIn(int? trangThai)
        {
            var IdAdmin = await _userManager.FindByEmailAsync("adminhehehe@gmail.com");
            var voucherTaiQuay = (await _voucherND.GetAllVouCherNguoiDung()).Where(c => c.IdNguoiDung == IdAdmin.Id).ToList();
            List<string> idVoucher = new List<string>();
            foreach (var item in voucherTaiQuay.GroupBy(c => c.IdVouCher).Select(c => c.First()).ToList())
            {
                idVoucher.Add(item.IdVouCher);
            }
            List<Voucher> lstVoucherDaIn = new List<Voucher>();
            foreach (var item in idVoucher)
            {
                var lstVoucher = (await _voucherSV.GetAllVoucher()).FirstOrDefault(c => c.IdVoucher == item);
                lstVoucherDaIn.Add(lstVoucher);
            }
            return View(lstVoucherDaIn);
        }
        [HttpPost]
        public async Task<IActionResult> FilterVoucherByStatusTaiQuayDaIn(int? trangThai)
        {
            var lstVoucher = (await _voucherSV.GetAllVoucher())
                .Where(c => c.TrangThai >= 6 && c.TrangThai <= 9).OrderByDescending(c => c.NgayTao).ToList();

            if (trangThai != null)
            {
                lstVoucher = lstVoucher.Where(c => c.TrangThai == trangThai).OrderByDescending(c => c.NgayTao).ToList();
            }

            return PartialView("_VoucherPartialTaiQuay", lstVoucher);
        }
        #endregion
    }
}
