using App_Data.DbContext;
using App_Data.Models;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Twilio.Rest.Api.V2010.Account;
using static App_Data.Repositories.TrangThai;

namespace App_View.Controllers
{
    [Authorize]
    public class VoucherNguoiDungController : Controller
    {

        private readonly BazaizaiContext _context;
        private readonly IVoucherNguoiDungservices _voucherND;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IVoucherservices _VouchersV;
        public VoucherNguoiDungController(IVoucherNguoiDungservices voucherNDServices, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager, IVoucherservices Voucherservices)
        {
            _voucherND = voucherNDServices;
            _context = new BazaizaiContext();
            _signInManager = signInManager;
            _userManager = userManager;
            _VouchersV = Voucherservices;
        }
        public async Task<IActionResult> Voucher_wallet(int? loaiHinh)
        {
            return View();
        }
        public async Task<IActionResult> VoucherWalletPatial(int? loaiHinh)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.IsInRoleAsync(user, "Admin");
            if (role)
            {
                return Json(new { mess = "Vui lòng dùng tài khoản khách!" });
            }

            var idNguoiDung = user.Id;
            if (string.IsNullOrEmpty(idNguoiDung))
            {
            }
            var voucherNguoiDung = (await _voucherND.GetAllVoucherNguoiDungByID(idNguoiDung)).Where(c => c.TrangThai == (int)TrangThaiVoucherNguoiDung.KhaDung).ToList();

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
            return PartialView("_VoucherWalletPartial", voucherNguoiDung);
        }

        [HttpPost]
        public async Task<IActionResult> SaveVoucher(string MaVoucher)
        {
            // Lấy ID của người dùng hiện tại từ UserManager
            var idNguoiDung = _userManager.GetUserId(User);

            // Kiểm tra nếu ID người dùng không tồn tại hoặc là chuỗi rỗng, thì trả về một View
            if (string.IsNullOrEmpty(idNguoiDung))
            {
                return Ok(false);
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
            return View();
        }
        public async Task<IActionResult> Voucher_wallet_historyPatial(int? TrangThai)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.IsInRoleAsync(user, "Admin");
            if (role)
            {
                return Json(new { mess = "Vui lòng dùng tài khoản khách!" });
            }
            var idNguoiDung = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(idNguoiDung))
            {
                return View();
            }

            var voucherNguoiDung = (await _voucherND.GetAllVoucherNguoiDungByID(idNguoiDung)).Where(c => c.TrangThai != (int)TrangThaiVoucherNguoiDung.KhaDung).ToList();

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

            return PartialView("_VoucherHistoryPatial", voucherNguoiDung);
        }
        public async Task<IActionResult> TotalSpending()
        {
            var NguoiDung = await _userManager.GetUserAsync(User);
            return View(NguoiDung);
        }

        public async Task<IActionResult> GetVoucherByMa(string ma)
        {
            var idNguoiDung = _userManager.GetUserId(User);
            var voucherNguoiDung = await _voucherND.GetAllVoucherNguoiDungByID(idNguoiDung);
            var voucher = await _VouchersV.GetVoucherByMa(ma);
            foreach (var item in voucherNguoiDung)
            {
                if (item.IdVouCher == voucher.IdVoucher && item.TrangThai == (int)TrangThaiVoucherNguoiDung.KhaDung)
                {
                    return Json(new { mess = "Voucher đã có trong tài khoản của bạn và đã được áp dụng!", idvoucher = item.IdVouCher, mucuudai = item.MucUuDai, loaiuudai=item.LoaiHinhUuDai,dieukien=item.DieuKien });
                }
                else if (item.IdVouCher == voucher.IdVoucher && item.TrangThai == (int)TrangThaiVoucherNguoiDung.DaSuDung)
                {
                    return Json(new { mess = "Bạn đã sử dụng voucher này!" });
                }
            }
            var tongtien = SessionServices.GetIdFomSession(HttpContext.Session, "TongTien");
            if (voucher.DieuKien > Convert.ToDouble(tongtien))
            {
                return Json(new { mess = "Đơn hàng của bạn không đủ điều kiện để dùng Voucher này!" });
            }
            await _voucherND.AddVoucherNguoiDung(ma, idNguoiDung);
            var Voucher = await _voucherND.GetVocherTaiQuay(ma);
            double mucuidai = 0;
            string IdVoucher = "";
            int loaiuudai = 0;
            double dieukien = 0;
            if (Voucher != null)
            {
                mucuidai = (double)Voucher.MucUuDai;
                IdVoucher = Voucher.IdVouCher;
                loaiuudai = (int)Voucher.LoaiHinhUuDai;
                dieukien = (double)Voucher.DieuKien;
            }
            return Json(new { mucuidai, IdVoucher, loaiuudai, dieukien });
        }
    }
}
