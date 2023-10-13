using App_Data.DbContextt;
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
        private readonly IVoucherServices _voucherSV;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;

        public VoucherController(IVoucherServices voucherServices, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager)
        {
            _voucherSV = voucherServices;
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


            var allVouchers = (await _voucherSV.GetAllVoucher()).Where(c => c.TrangThai == 0);

            var tienMatVouchers = allVouchers.Where(c => c.LoaiHinhUuDai == (int)TrangThaiLoaiKhuyenMai.TienMat);
            var phanTramVouchers = allVouchers.Where(c => c.LoaiHinhUuDai == (int)TrangThaiLoaiKhuyenMai.PhanTram);
            var freeShipVouchers = allVouchers.Where(c => c.LoaiHinhUuDai == (int)TrangThaiLoaiKhuyenMai.Freeship);

            ViewBag.TheoTienMat = tienMatVouchers;
            ViewBag.TheoPhanTram = phanTramVouchers;
            ViewBag.FreeShip = freeShipVouchers;

            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
