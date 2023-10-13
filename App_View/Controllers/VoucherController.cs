using App_Data.DbContextt;
using App_Data.Models;
using App_View.IServices;
using Microsoft.AspNetCore.Mvc;
using static App_Data.Repositories.TrangThai;

namespace App_View.Controllers
{
    public class VoucherController : Controller
    {
        private readonly BazaizaiContext _context;
        private readonly IVoucherServices _voucherSV;

        public VoucherController(IVoucherServices voucherServices)
        {
            _voucherSV = voucherServices;
            _context = new BazaizaiContext();
        }
        public async Task<IActionResult> VoucherToCalm()
        {
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
