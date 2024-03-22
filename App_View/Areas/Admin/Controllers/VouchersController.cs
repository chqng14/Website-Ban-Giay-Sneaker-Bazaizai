using App_Data.DbContext;
using App_Data.Models;
using App_Data.ViewModels.Voucher;
using App_Data.ViewModels.VoucherNguoiDung;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VouchersController : Controller
    {
        private readonly BazaizaiContext _context;
        private readonly IVoucherservices _VouchersV;
        private readonly IVoucherNguoiDungservices _voucherND;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IEmailSender _emailSender;
        private IViewRenderService _viewRenderService;
        public VouchersController(IVoucherservices Voucherservices, IVoucherNguoiDungservices voucherNDServices, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager, IEmailSender emailSender, IViewRenderService viewRenderService)
        {
            _voucherND = voucherNDServices;
            _VouchersV = Voucherservices;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _viewRenderService = viewRenderService;
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
            var lstVoucher = (await _VouchersV.GetAllVoucher())
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
                if (await _VouchersV.CreateVoucher(voucherDTO))
                {
                    return Ok(new { message = " Thêm mới thành công" });
                }
            }
            return Ok(new { error = "Thêm voucher thất bại" });

        }

        public async Task<ActionResult> Edit(string id)
        {
            var Voucher = await _VouchersV.GetVoucherDTOById(id);
            return View(Voucher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VoucherDTO voucherDTO)
        {
            if (ModelState.IsValid)
            {
                if (await _VouchersV.UpdateVoucher(voucherDTO))
                {
                    return RedirectToAction("ShowVoucher");
                }
            }
            return RedirectToAction("ShowVoucher");
        }

        public async Task<ActionResult> Delete(string id)
        {
            await _VouchersV.DeleteVoucher(id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteVoucherWithList(List<string> voucherIds)
        {
            if (voucherIds.Any())
            {
                if (await _VouchersV.DeleteVoucherWithList(voucherIds) == true)
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
                await _VouchersV.RestoreVoucherWithList(voucherIds);
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
                        string html = await _viewRenderService.RenderToStringAsync("Voucher/MailVoucher");

                        await _emailSender.SendEmailAsync(userKhachHang.Email, subject, html);
                    }

                    return Ok(true);
                }

            }

            return Ok(false);
        }
        public async Task<IActionResult> SendMail(string MaHd)
        {
            var userKhachHang = "";
            if (SessionServices.GetIdFomSession(HttpContext.Session, "Email") != null)
            {
                userKhachHang = SessionServices.GetIdFomSession(HttpContext.Session, "Email");
            }
            else
            {
                var user = _userManager.GetUserId(User);
                userKhachHang = (await _userManager.FindByIdAsync(user)).Email;
            }
            var sp = SessionServices.GetFomSession(HttpContext.Session, "sp");
            var subject = $"Đơn hàng #{MaHd} đã đặt thành công";
            string html = await _viewRenderService.RenderToStringAsync("HoaDon/Mail", sp);
            await _emailSender.SendEmailAsync(userKhachHang, subject, html);
            return Ok();
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
            var lstVoucher = (await _VouchersV.GetAllVoucher())
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
                if (await _VouchersV.CreateTaiQuay(voucherDTO))
                {
                    return Ok(new { message = " Thêm mới thành công" });
                }
            }
            return Ok(new { error = "Thêm voucher thất bại" });

        }
        public async Task<ActionResult> EditTaiQuay(string id)
        {
            var Voucher = await _VouchersV.GetVoucherDTOById(id);
            return View(Voucher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTaiQuay(VoucherDTO voucherDTO)
        {
            if (ModelState.IsValid)
            {
                if (await _VouchersV.UpdateTaiQuay(voucherDTO))
                {
                    return RedirectToAction("ShowVoucherTaiQuay");
                }
            }
            return View();
        }

        public async Task<ActionResult> DeleteTaiQuay(string id)
        {
            await _VouchersV.DeleteTaiQuay(id);
            return RedirectToAction("ShowVoucherTaiQuay");
        }

        public async Task<ActionResult> DeleteVoucherWithListTaiQuay(List<string> voucherIds)
        {
            if (voucherIds.Any())
            {
                if (await _VouchersV.DeleteVoucherWithListTaiQuay(voucherIds) == true)
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
                await _VouchersV.RestoreVoucherWithListTaiQuay(voucherIds);
                return Ok(true);
            }
            return Ok(false);
        }
        [HttpPost]
        public async Task<IActionResult> InVoucherTaiQuay(string idVoucher, int soLuong)
        {
            var IdAdmin = await _userManager.FindByEmailAsync("bazaizaistore@gmail.com");
            var VoucherKhaDung = await _VouchersV.GetVoucherDTOById(idVoucher);
            if (IdAdmin == null || soLuong <= 0 || VoucherKhaDung == null)
            {
                return Ok(false);
            }
            else if (await _VouchersV.AddVoucherCungBanTaiQuay(idVoucher, IdAdmin.Id, soLuong))
            {
                return Ok(true);
            }
            return Ok(false);
        }
        public async Task<IActionResult> ShowVoucherTaiQuayDaIn(int? trangThai)
        {
            return View();
        }

        public async Task<IActionResult> FilterVoucherTaiQuayDaIn(int? trangThai)
        {
            var IdAdmin = await _userManager.FindByEmailAsync("bazaizaistore@gmail.com");
            var voucherTaiQuay = (await _voucherND.GetAllVouCherNguoiDung()).Where(c => c.IdNguoiDung == IdAdmin.Id).ToList();
            List<string> idVoucher = new List<string>();
            foreach (var item in voucherTaiQuay.GroupBy(c => c.IdVouCher).Select(c => c.First()).ToList())
            {
                idVoucher.Add(item.IdVouCher);
            }
            List<Voucher> lstVoucherDaIn = new List<Voucher>();
            foreach (var item in idVoucher)
            {
                var lstVoucher = (await _VouchersV.GetAllVoucher()).FirstOrDefault(c => c.IdVoucher == item);
                lstVoucherDaIn.Add(lstVoucher);
            }
            if (trangThai != null)
            {
                lstVoucherDaIn = lstVoucherDaIn.Where(c => c.TrangThai == trangThai).ToList();
            }

            var voucherDTOs = lstVoucherDaIn.Select(voucher => new VoucherDTO
            {
                IdVoucher = voucher.IdVoucher,
                MaVoucher = voucher.MaVoucher,
                TenVoucher = voucher.TenVoucher,
                DieuKien = voucher.DieuKien,
                LoaiHinhUuDai = voucher.LoaiHinhUuDai,
                SoLuong = voucher.SoLuong,
                MucUuDai = voucher.MucUuDai,
                NgayBatDau = voucher.NgayBatDau,
                NgayTao = voucher.NgayTao,
                NgayKetThuc = voucher.NgayKetThuc,
                TrangThai = voucher.TrangThai,
                SoLuongVoucherDaIn = voucherTaiQuay.Count(c => c.NgayNhan != null && c.IdVouCher == voucher.IdVoucher),
                SoLuongVoucherChuaIN = voucherTaiQuay.Count(c => c.NgayNhan == null && c.IdVouCher == voucher.IdVoucher)
            }).ToList();


            return PartialView("_VoucherDaInTaiQuayPartial", voucherDTOs);
        }
        [HttpPost]
        public async Task<IActionResult> FilterListDetailsVoucherTaiQuayDaIn(string idVoucher, int trangThai = 0)
        {

            var lstVoucherDaIn = (await _voucherND.GetAllVouCherNguoiDung()).Where(c => c.IdVouCher == idVoucher && c.NgayNhan == null).ToList();
            List<VoucherNguoiDungDTO> voucherNguoiDungDTOList = new List<VoucherNguoiDungDTO>();
            foreach (var voucher in lstVoucherDaIn.Take(500))
            {
                VoucherNguoiDungDTO voucherNguoiDungDTO = new VoucherNguoiDungDTO();
                // Gán thông tin từ voucher vào voucherNguoiDungDTO
                voucherNguoiDungDTO.IdVouCherNguoiDung = voucher.IdVouCherNguoiDung;
                voucherNguoiDungDTO.IdVouCher = voucher.IdVouCher;
                voucherNguoiDungDTO.TenVoucher = voucher.TenVoucher;
                voucherNguoiDungDTO.LoaiHinhUuDai = voucher.LoaiHinhUuDai;
                voucherNguoiDungDTO.NgayBatDau = voucher.NgayBatDau;
                voucherNguoiDungDTO.NgayKetThuc = voucher.NgayKetThuc;
                voucherNguoiDungDTO.DieuKien = voucher.DieuKien;
                voucherNguoiDungDTO.MucUuDai = voucher.MucUuDai;
                voucherNguoiDungDTO.NgayTao = voucher.NgayTao;
                voucherNguoiDungDTO.TrangThai = voucher.TrangThai;
                // ...
                // Thêm voucherNguoiDungDTO vào danh sách voucherNguoiDungDTOList
                voucherNguoiDungDTOList.Add(voucherNguoiDungDTO);
            }
            if (trangThai != 0)
            {//In 1 lần 500 voucher
                voucherNguoiDungDTOList = voucherNguoiDungDTOList.Where(c => c.TrangThai == trangThai).ToList();
            }
            SessionServices.SetLstString(HttpContext.Session, "lstIdVoucherNguoiDung", voucherNguoiDungDTOList.Select(v => v.IdVouCherNguoiDung).Take(500).ToList());

            return PartialView("_FilterListDetailsVoucherTaiQuayDaIn", voucherNguoiDungDTOList);
        }
        public async Task<IActionResult> CapNhatVoucherNguoiDungDain()
        {
            var lstIdVoucherNguoiDung = SessionServices.GetLstString(HttpContext.Session, "lstIdVoucherNguoiDung");

            if (lstIdVoucherNguoiDung != null && lstIdVoucherNguoiDung.Any())
            {
                if (await _VouchersV.UpdateTrangThaiKhiXuat(lstIdVoucherNguoiDung))
                {
                    return Ok(true);
                }
                return Ok(false);
            }
            return Ok(false);
        }
        #endregion
    }
}
