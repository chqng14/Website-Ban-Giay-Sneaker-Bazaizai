using App_Data.DbContextt;
using App_Data.Models;
using App_Data.ViewModels.Voucher;
using App_View.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static App_Data.Repositories.TrangThai;

namespace App_View.Controllers
{
    public class VoucherNguoiDungController : Controller
    {

        private readonly BazaizaiContext _context;
        private readonly IVoucherNguoiDungServices _voucherND;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        public VoucherNguoiDungController(IVoucherNguoiDungServices voucherNDServices, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager)
        {
            _voucherND = voucherNDServices;
            _context = new BazaizaiContext();
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Voucher_wallet(int? loaiHinh)
        {
            var idNguoiDung = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(idNguoiDung))
            {
                return View();
            }

            var voucherNguoiDung = (await _voucherND.GetAllVoucherNguoiDungByID(idNguoiDung)).Where(c => c.TrangThai == (int)TrangThaiVoucherNguoiDung.KhaDung);

            if (loaiHinh.HasValue)
            {
                switch (loaiHinh)
                {
                    case (int)TrangThaiLoaiKhuyenMai.TienMat:
                        voucherNguoiDung = voucherNguoiDung.Where(v => v.LoaiHinhUuDai == 0).ToList();
                        break;
                    case (int)TrangThaiLoaiKhuyenMai.PhanTram:
                        voucherNguoiDung = voucherNguoiDung.Where(v => v.LoaiHinhUuDai == 1).ToList();
                        break;
                    case (int)TrangThaiLoaiKhuyenMai.Freeship:
                        voucherNguoiDung = voucherNguoiDung.Where(v => v.LoaiHinhUuDai == 2).ToList();
                        break;
                    default:
                        break;
                }
            }

            ViewBag.TatCa = voucherNguoiDung; // Gán danh sách lọc được vào ViewBag.TatCa

            return View(voucherNguoiDung);
        }

        [HttpPost]
        public async Task<IActionResult> SaveVoucher(string MaVoucher)
        {
            // Lấy ID của người dùng hiện tại từ UserManager
            var idNguoiDung = _userManager.GetUserId(User);

            // Kiểm tra nếu ID người dùng không tồn tại hoặc là chuỗi rỗng, thì trả về một View
            if (string.IsNullOrEmpty(idNguoiDung))
            {
                return RedirectToAction("Voucher_wallet");
            }
            if (MaVoucher != null)
            {
                // Nếu ID người dùng tồn tại, thì gọi phương thức AddVoucherNguoiDung để thêm Mã Voucher cho người dùng
                if (await _voucherND.AddVoucherNguoiDung(MaVoucher, idNguoiDung) == true)
                    return Ok(true);
            }
            return Ok(false);
        }
        public async Task<IActionResult> Voucher_wallet_history(int? TrangThai)
        {
            var idNguoiDung = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(idNguoiDung))
            {
                return View();
            }

            var voucherNguoiDung = (await _voucherND.GetAllVoucherNguoiDungByID(idNguoiDung)).Where(c => c.TrangThai != (int)TrangThaiVoucherNguoiDung.KhaDung);

            if (TrangThai.HasValue)
            {
                switch (TrangThai)
                {
                    case (int)TrangThaiVoucherNguoiDung.DaSuDung:
                        voucherNguoiDung = voucherNguoiDung.Where(v => v.TrangThai == 1).ToList();
                        break;
                    case (int)TrangThaiVoucherNguoiDung.HetHieuLuc:
                        voucherNguoiDung = voucherNguoiDung.Where(v => v.TrangThai == 2).ToList();
                        break;
                    default:
                        break;
                }
            }

            ViewBag.TatCa = voucherNguoiDung; // Gán danh sách lọc được vào ViewBag.TatCa

            return View(voucherNguoiDung);
        }
        public async Task<IActionResult> TotalSpending()
        {
            var NguoiDung = await _userManager.GetUserAsync(User);
            return View(NguoiDung);
        }

        public async Task<IActionResult> GetVoucherByMa(string ma)
        {
            var idNguoiDung = _userManager.GetUserId(User);
            await _voucherND.AddVoucherNguoiDung(ma, idNguoiDung);
            var Voucher = await _voucherND.GetVocherTaiQuay(ma);
            double mucuidai = 0;
            string IdVoucher = "";
            int loaiuudai = 0;
            if (Voucher != null)
            {
                mucuidai = (double)Voucher.MucUuDai;
                IdVoucher = Voucher.IdVouCher;
                loaiuudai = (int)Voucher.LoaiHinhUuDai;
            }
            return Json(new { mucuidai, IdVoucher, loaiuudai });
        }
    }
}
