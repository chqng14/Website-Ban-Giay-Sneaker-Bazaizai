using App_Data.DbContext;
using App_Data.Models;
using App_View.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static App_Data.Repositories.TrangThai;

namespace App_View.Controllers
{

    public class VoucherController : Controller
    {
        private readonly BazaizaiContext _context;
        private readonly IVoucherservices _VouchersV;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;

        public VoucherController(IVoucherservices Voucherservices, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager)
        {
            _VouchersV = Voucherservices;
            _context = new BazaizaiContext();
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> VoucherToCalm()
        {
            var idNguoiDung = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(idNguoiDung))
            {
                ViewBag.NguoiDung = null;
            }
            else ViewBag.NguoiDung = idNguoiDung;
            return View();
        }
        public async Task<IActionResult> VoucherLstToCalm(string LoaiHinh)
        {
            var idNguoiDung = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(idNguoiDung))
            {
                ViewBag.NguoiDung = null;
            }
            else ViewBag.NguoiDung = idNguoiDung;

            var allVouchers = (await _VouchersV.GetAllVoucher()).Where(c => c.TrangThai == 0 && c.SoLuong > 0);
            switch (LoaiHinh)
            {
                case "TienMat":
                    allVouchers = allVouchers.Where(c => c.LoaiHinhUuDai == (int)TrangThaiLoaiKhuyenMai.TienMat);
                    break;
                case "PhanTram":
                    allVouchers = allVouchers.Where(c => c.LoaiHinhUuDai == (int)TrangThaiLoaiKhuyenMai.PhanTram);
                    break;
                case "FreeShip":
                    allVouchers = allVouchers.Where(c => c.LoaiHinhUuDai == (int)TrangThaiLoaiKhuyenMai.Freeship);
                    break;
                default:
                    break;
            }
            ViewBag.TatCaVoucher = allVouchers;
            return PartialView("_VoucherLstToCalm", allVouchers);
        }

        public async Task<IActionResult> GetVoucherById(string idVoucher)
        {
            var Voucher = await _VouchersV.GetVoucherDTOById(idVoucher);
            double mucuidai = 0;
            string IdVoucher = "";
            int loaiuudai = 0;
            if (Voucher != null)
            {
                mucuidai = (double)Voucher.MucUuDai;
                IdVoucher = Voucher.IdVoucher;
                loaiuudai = (int)Voucher.LoaiHinhUuDai;
            }
            return Json(new { mucuidai, IdVoucher, loaiuudai });
        }

        public async Task<IActionResult> UpdateVoucherAfterUseIt(string idVoucher)
        {
            var idNguoiDung = _userManager.GetUserId(User);
            if (await _VouchersV.UpdateVoucherAfterUseIt(idVoucher, idNguoiDung))
            {
                return Ok();
            }
            return BadRequest();
        }
        public async Task<IActionResult> UpdateVouchersoluong(string idVoucher)
        {
            if (await _VouchersV.UpdateVouchersoluong(idVoucher))
            {
                return Ok();
            }
            return BadRequest();
        }
        public async Task<IActionResult> VoucherDetails(string ma)
        {
            var Voucher = await _VouchersV.GetVoucherByMa(ma);
            return View(Voucher);
        }
        public async Task<IActionResult> VoucherDetailsPartial(string ma)
        {
            var Voucher = await _VouchersV.GetVoucherByMa(ma);
            return PartialView("_VoucherDetailsPartial", Voucher);
        }

        public async Task<IActionResult> GetVoucherByMa(string ma)
        {
            var Voucher = await _VouchersV.GetVoucherByMa(ma);
            double mucuidai = 0;
            string IdVoucher = "";
            int loaiuudai = 0;
            if (Voucher != null)
            {
                mucuidai = (double)Voucher.MucUuDai;
                IdVoucher = Voucher.IdVoucher;
                loaiuudai = (int)Voucher.LoaiHinhUuDai;
            }
            return Json(new { mucuidai, IdVoucher, loaiuudai });
        }
    }
}
