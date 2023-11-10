using App_Data.DbContextt;
using App_Data.Models;
using App_Data.ViewModels.HoaDonChiTietDTO;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Mvc;
using static App_Data.Repositories.TrangThai;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BanHangTaiQuayController : Controller
    {
        private readonly IHoaDonServices _hoaDonServices;
        private readonly ISanPhamChiTietService _sanPhamChiTietService;
        private readonly IHoaDonChiTietServices _hoaDonChiTietServices;
        private readonly BazaizaiContext _bazaizaiContext;
        //tesst
        public BanHangTaiQuayController(ISanPhamChiTietService sanPhamChiTietService)
        {
            HttpClient httpClient = new HttpClient();
            _hoaDonServices = new HoaDonServices();
            _sanPhamChiTietService = sanPhamChiTietService;
            _hoaDonChiTietServices = new HoaDonChiTietServices();
            _bazaizaiContext = new BazaizaiContext();
        }

        [HttpGet]
        public async Task<IActionResult> DanhSachHoaDonCho()
        {
            var listHoaDonCho = await _hoaDonServices.GetAllHoaDonCho();
            var listsanpham = await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync();
            foreach (var item in listHoaDonCho)
            {
                foreach (var item2 in item.hoaDonChiTietDTOs)
                {
                    var sanpham = listsanpham.FirstOrDefault(c => c.IdChiTietSp == item2.IdSanPhamChiTiet);
                    item2.TenSanPham = sanpham.TenSanPham + "/" + sanpham.MauSac + "/" + sanpham.KichCo;
                    //item2.masanpham  = sanpham
                }
            }
            return View(listHoaDonCho.OrderBy(c => Convert.ToInt32(c.MaHoaDon.Substring(2, c.MaHoaDon.Length - 2))));
        }
        [HttpPost]
        public async Task<IActionResult> TaoHoaDonTaiQuay()
        {
            var hoaDonMoi = new HoaDon()
            {
                IdHoaDon = Guid.NewGuid().ToString(),
            };
            var newHoaDon = await _hoaDonServices.TaoHoaDonTaiQuay(hoaDonMoi);
            return Json(newHoaDon.MaHoaDon);
        }

        [HttpPost]
        public async Task<IActionResult> ThemSanPhamVaoHoaDon(string maHD, string idSanPham)
        {
            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(hd => hd.MaHoaDon == maHD);
            var sanPham = (await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync()).FirstOrDefault(c => c.IdChiTietSp == idSanPham);
            if (sanPham.SoLuongTon == 0)
            {
                return Ok(new { TrangThai = false });
            }
            var hoaDonChitiet = new HoaDonChiTiet()
            {
                IdHoaDon = hoaDon.Id,
                IdHoaDonChiTiet = Guid.NewGuid().ToString(),
                IdSanPhamChiTiet = idSanPham.ToString(),
                SoLuong = 1,
                TrangThai = (int)TrangThaiHoaDonChiTiet.ChoTaiQuay,
                GiaBan = sanPham.GiaThucTe,
                GiaGoc = sanPham.GiaGoc,
            };
            var hoaDonChiTietTraLai = await _hoaDonChiTietServices.ThemSanPhamVaoHoaDon(hoaDonChitiet);

            await _sanPhamChiTietService.UpDatSoLuongAynsc(new SanPhamSoLuongDTO()
            {
                IdChiTietSanPham = sanPham.IdChiTietSp,
                SoLuong = 1
            });
            var tongTienThayDoi = hoaDonChiTietTraLai.GiaGoc;
            var soTienTraLaiThayDoi = hoaDonChiTietTraLai.GiaBan;
            var SoTienKhyenMaiGiam = tongTienThayDoi - soTienTraLaiThayDoi;
            return Ok(new
            {
                TrangThai = true,
                IdHoaDon = hoaDonChiTietTraLai.IdHoaDon,
                IdHoaDonChiTiet = hoaDonChiTietTraLai.IdHoaDonChiTiet,
                IdSanPhamChiTiet = hoaDonChiTietTraLai.IdSanPhamChiTiet,
                SoLuong = hoaDonChiTietTraLai.SoLuong,
                GiaBan = hoaDonChiTietTraLai.GiaBan,
                GiaGoc = hoaDonChiTietTraLai.GiaGoc,
                TenSanPham = sanPham.TenSanPham + "/" + sanPham.MauSac + "/" + sanPham.KichCo,
                TongTienThayDoi = tongTienThayDoi,
                SoTienTraLaiThayDoi = soTienTraLaiThayDoi,
                SoTienKhyenMaiGiam = SoTienKhyenMaiGiam,
                SoTienVoucherGiam = 0,
            });
        }

        [HttpGet]
        public async Task<IActionResult> LoadPartialViewDanhSachSanPham(string tukhoa)
        {
            if (!string.IsNullOrWhiteSpace(tukhoa))
                return PartialView("_DanhSachSanPhamPartialView", (await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync()).Where(c => c.TenSanPham.ToLower().Replace(" ", "").Contains(tukhoa.ToLower().Replace(" ", ""))));
            else
                return PartialView("_DanhSachSanPhamPartialView", await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync());
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSoLuong(string maHD, string idSanPham, int SoLuongMoi)
        {
            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(hd => hd.MaHoaDon == maHD);
            var sanPham = (await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync()).FirstOrDefault(c => c.IdChiTietSp == idSanPham);

            var soLuongThayDoi = await _hoaDonChiTietServices.UpdateSoLuong(hoaDon.Id, idSanPham, SoLuongMoi, sanPham.SoLuongTon.ToString());
            if (soLuongThayDoi != "")
            {
                var tongTienThayDoi = Convert.ToInt32(soLuongThayDoi) * (double)hoaDon.hoaDonChiTietDTOs.FirstOrDefault(c => c.IdSanPhamChiTiet == idSanPham).GiaGoc;
                var soTienTraLaiThayDoi = Convert.ToInt32(soLuongThayDoi) * (double)hoaDon.hoaDonChiTietDTOs.FirstOrDefault(c => c.IdSanPhamChiTiet == idSanPham).GiaBan;
                var SoTienKhyenMaiGiam = tongTienThayDoi - soTienTraLaiThayDoi;
                await _sanPhamChiTietService.UpDatSoLuongAynsc(new SanPhamSoLuongDTO()
                {
                    IdChiTietSanPham = idSanPham,
                    SoLuong = Convert.ToInt32(soLuongThayDoi)
                });
                if (Convert.ToInt32(sanPham.SoLuongTon) - Convert.ToInt32(soLuongThayDoi) != 0)
                {
                    return Ok(new
                    {
                        TrangThai = true,
                        SoLuongConLai = Convert.ToInt32(sanPham.SoLuongTon) - Convert.ToInt32(soLuongThayDoi),
                        TongTienThayDoi = tongTienThayDoi,
                        SoTienTraLaiThayDoi = soTienTraLaiThayDoi,
                        SoTienKhyenMaiGiam = SoTienKhyenMaiGiam,
                        SoTienVoucherGiam = 0,

                    });
                }
                else
                {
                    return Ok(new
                    {
                        TrangThai = true,
                        TongTienThayDoi = tongTienThayDoi,
                        SoTienTraLaiThayDoi = soTienTraLaiThayDoi,
                        SoTienKhyenMaiGiam = SoTienKhyenMaiGiam,
                        SoLuongConLai = "Hết hàng",
                        SoTienVoucherGiam = 0,

                    });
                }
            }
            else
            {
                var soLuongTrongHoaDon = hoaDon.hoaDonChiTietDTOs.FirstOrDefault(c => c.IdSanPhamChiTiet == idSanPham).SoLuong;
                if (sanPham.SoLuongTon > 0)
                    return Ok(new
                    {
                        TrangThai = false,
                        SoLuongConLai = sanPham.SoLuongTon,
                        SoLuongCu = soLuongTrongHoaDon,
                    });
                else
                    return Ok(new
                    {
                        TrangThai = false,
                        SoLuongConLai = "Hết hàng",
                        SoLuongCu = soLuongTrongHoaDon,
                    });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> XoaSanPhamKhoiHoaDon(string maHD, string idSanPham)
        {
            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(hd => hd.MaHoaDon == maHD);
            var soLuongThayDoi = await _hoaDonChiTietServices.XoaSanPhamKhoiHoaDon(hoaDon.Id, idSanPham);
            var tongTienThayDoi = -Convert.ToInt32(soLuongThayDoi) * (double)hoaDon.hoaDonChiTietDTOs.FirstOrDefault(c => c.IdSanPhamChiTiet == idSanPham).GiaGoc;
            var soTienTraLaiThayDoi = -Convert.ToInt32(soLuongThayDoi) * (double)hoaDon.hoaDonChiTietDTOs.FirstOrDefault(c => c.IdSanPhamChiTiet == idSanPham).GiaBan;
            var SoTienKhyenMaiGiam = tongTienThayDoi - soTienTraLaiThayDoi;
            await _sanPhamChiTietService.UpDatSoLuongAynsc(new SanPhamSoLuongDTO()
            {
                IdChiTietSanPham = idSanPham,
                SoLuong = (-Convert.ToInt32(soLuongThayDoi))
            });
            var sanPham = (await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync()).FirstOrDefault(c => c.IdChiTietSp == idSanPham);
            return Ok(new
            {
                SoLuongConLai = sanPham.SoLuongTon,
                TongTienThayDoi = tongTienThayDoi,
                SoTienTraLaiThayDoi = soTienTraLaiThayDoi,
                SoTienKhyenMaiGiam = SoTienKhyenMaiGiam,
                SoTienVoucherGiam = 0,
            });
        }
        [HttpGet]
        public async Task<IActionResult> HoaDonDuocChon(string maHD)
        {
            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(hd => hd.MaHoaDon == maHD);
            double tongTienGoc = 0;
            double tienPhaiTra = 0;
            string ngayTao = ((DateTime)hoaDon.NgayTao).ToString("dd/MM/yyyy HH:mm");
            foreach (var item in hoaDon.hoaDonChiTietDTOs)
            {
                tongTienGoc += (double)item.GiaGoc * (int)item.SoLuong;
                tienPhaiTra += (double)item.GiaBan * (int)item.SoLuong;
            }
            return Ok(new
            {
                TongTienGoc = tongTienGoc,
                TienPhaiTra = tienPhaiTra,
                SoTienKhuyenMaiGiam = tongTienGoc - tienPhaiTra,
                SoTienVoucherGiam = 0,
                MaHoaDon = hoaDon.MaHoaDon,
                NgayTao = ngayTao
            });
        }
    }
}
