using App_Data.DbContextt;
using App_Data.Models;
using App_Data.Repositories;
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
        public async Task<IActionResult> VoucherToCalm(string LoaiHinh)
        {
            var idNguoiDung = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(idNguoiDung))
            {
                ViewBag.NguoiDung = null;
            }
            else ViewBag.NguoiDung = idNguoiDung;


            var allVouchers = (await _voucherSV.GetAllVoucher()).Where(c => c.TrangThai == 0);
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
            return View(allVouchers);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
