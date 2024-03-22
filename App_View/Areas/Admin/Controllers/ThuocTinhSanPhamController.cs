using App_Data.DbContext;
using App_Data.Models;
using App_Data.Models.ViewModels.MauSac;
using App_Data.ViewModels.ChatLieuDTO;
using App_Data.ViewModels.KichCoDTO;
using App_Data.ViewModels.KieuDeGiayDTO;
using App_Data.ViewModels.LoaiGiayDTO;
using App_Data.ViewModels.SanPhamChiTiet.SanPhamDTO;
using App_Data.ViewModels.SanPhamChiTiet.ThuongHieuDTO;
using App_Data.ViewModels.ThuocTinh;
using App_Data.ViewModels.XuatXu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ThuocTinhSanPhamController : Controller
    {
        private readonly BazaizaiContext _context;
        private readonly HttpClient _httpClient;
        public ThuocTinhSanPhamController(HttpClient httpClient)
        {
            _context = new BazaizaiContext();
            _httpClient = httpClient;
        }

        public IActionResult LoadPartialViewDanhSachTenSanPham()
        {
            var lstTenSP = _context
                .SanPhams
                .AsNoTracking()
                .Select(it => new ThuocTinhViewModel()
                {
                    Id = it.IdSanPham,
                    Ma = it.MaSanPham,
                    Ten = it.TenSanPham,
                    SoBienTheDangDung = _context.SanPhamChiTiets.Where(sp => sp.IdSanPham == it.IdSanPham).Count(),
                    TrangThai = it.Trangthai == 0 ? "Hoạt động" : "Không hoạt động"
                })
                .AsEnumerable()
                .OrderBy(it => int.Parse(it.Ma!.Substring(2)))
                .ToList();

            return PartialView("_DanhSachThuocTinhSanPhamPartialView",lstTenSP);
        }

        public IActionResult LoadPartialViewDanhSachThuongHieu()
        {
            var lstThuongHieu = _context
                .ThuongHieus
                .AsNoTracking()
                .Select(it => new ThuocTinhViewModel()
                {
                    Id = it.IdThuongHieu,
                    Ma = it.MaThuongHieu,
                    Ten = it.TenThuongHieu,
                    SoBienTheDangDung = _context.SanPhamChiTiets.Where(sp => sp.IdThuongHieu == it.IdThuongHieu).Count(),
                    TrangThai = it.TrangThai == 0 ? "Hoạt động" : "Không hoạt động"
                })
                .AsEnumerable()
                .OrderBy(it => int.Parse(it.Ma!.Substring(2)))
                .ToList();

            return PartialView("_DanhSachThuocTinhThuongHieuPartialView", lstThuongHieu);
        }

        public IActionResult LoadPartialViewDanhSachLoaiGiay()
        {
            var lstLoaiGiay = _context
                .LoaiGiays
                .AsNoTracking()
                .Select(it => new ThuocTinhViewModel()
                {
                    Id = it.IdLoaiGiay,
                    Ma = it.MaLoaiGiay,
                    Ten = it.TenLoaiGiay,
                    SoBienTheDangDung = _context.SanPhamChiTiets.Where(sp => sp.IdLoaiGiay == it.IdLoaiGiay).Count(),
                    TrangThai = it.TrangThai == 0 ? "Hoạt động" : "Không hoạt động"
                })
                .AsEnumerable()
                .OrderBy(it => int.Parse(it.Ma!.Substring(2)))
                .ToList();

            return PartialView("_DanhSachThuocTinhLoaiGiayPartialView", lstLoaiGiay);
        }

        public IActionResult LoadPartialViewDanhSachKieuDeGiay()
        {
            var lstKieuDeGiay = _context
                .KieuDeGiays
                .AsNoTracking()
                .Select(it => new ThuocTinhViewModel()
                {
                    Id = it.IdKieuDeGiay,
                    Ma = it.MaKieuDeGiay,
                    Ten = it.TenKieuDeGiay,
                    SoBienTheDangDung = _context.SanPhamChiTiets.Where(sp => sp.IdKieuDeGiay == it.IdKieuDeGiay).Count(),
                    TrangThai = it.Trangthai == 0 ? "Hoạt động" : "Không hoạt động"
                })
                .AsEnumerable()
                .OrderBy(it => int.Parse(it.Ma!.Substring(3)))
                .ToList();

            return PartialView("_DanhSachThuocTinhKieuDeGiayPartialView", lstKieuDeGiay);
        }

        public IActionResult LoadPartialViewDanhSachXuatXu()
        {
            var lstXuatXu = _context
                .XuatXus
                .AsNoTracking()
                .Select(it => new ThuocTinhViewModel()
                {
                    Id = it.IdXuatXu,
                    Ma = it.Ma,
                    Ten = it.Ten,
                    SoBienTheDangDung = _context.SanPhamChiTiets.Where(sp => sp.IdXuatXu == it.IdXuatXu).Count(),
                    TrangThai = it.TrangThai == 0 ? "Hoạt động" : "Không hoạt động"
                })
                .AsEnumerable()
                .OrderBy(it => int.Parse(it.Ma!.Substring(2)))
                .ToList();

            return PartialView("_DanhSachThuocTinhXuatXuPartialView", lstXuatXu);
        }

        public IActionResult LoadPartialViewDanhSachChatLieu()
        {
            var lstChatLieu = _context
                .ChatLieus
                .AsNoTracking()
                .Select(it => new ThuocTinhViewModel()
                {
                    Id = it.IdChatLieu,
                    Ma = it.MaChatLieu,
                    Ten = it.TenChatLieu,
                    SoBienTheDangDung = _context.SanPhamChiTiets.Where(sp => sp.IdChatLieu == it.IdChatLieu).Count(),
                    TrangThai = it.TrangThai == 0 ? "Hoạt động" : "Không hoạt động"
                })
                .AsEnumerable()
                .OrderBy(it => int.Parse(it.Ma!.Substring(2)))
                .ToList();

            return PartialView("_DanhSachThuocTinhChatLieuPartialView", lstChatLieu);
        }

        public IActionResult LoadPartialViewDanhSachMauSac()
        {
            var lstMauSac = _context
                .MauSacs
                .AsNoTracking()
                .Select(it => new ThuocTinhViewModel()
                {
                    Id = it.IdMauSac,
                    Ma = it.MaMauSac,
                    Ten = it.TenMauSac,
                    SoBienTheDangDung = _context.SanPhamChiTiets.Where(sp => sp.IdMauSac == it.IdMauSac).Count(),
                    TrangThai = it.TrangThai == 0 ? "Hoạt động" : "Không hoạt động"
                })
                .AsEnumerable()
                .OrderBy(it => int.Parse(it.Ma!.Substring(2)))
                .ToList();

            return PartialView("_DanhSachThuocTinhMauSacPartialView", lstMauSac);
        }

        public IActionResult LoadPartialViewDanhSachKichCo()
        {
            var lstKichCo = _context
                .KichCos
                .AsNoTracking()
                .Select(it => new ThuocTinhViewModel()
                {
                    Id = it.IdKichCo,
                    Ma = it.MaKichCo,
                    Ten = it.SoKichCo.ToString(),
                    SoBienTheDangDung = _context.SanPhamChiTiets.Where(sp => sp.IdKichCo == it.IdKichCo).Count(),
                    TrangThai = it.TrangThai == 0 ? "Hoạt động" : "Không hoạt động"
                })
                .AsEnumerable()
                .OrderBy(it => int.Parse(it.Ma!.Substring(2)))
                .ToList();

            return PartialView("_DanhSachThuocTinhKichCoPartialView", lstKichCo);
        }

        public IActionResult DanhSachThuocTinhSanPham()
        {
            return View();
        }


        #region ThaoTac
        //Xoa San Pham
        public async Task<IActionResult> DeleteSanPham(string idSanPham)
        {
            var response = await _httpClient.DeleteAsync($"/api/SanPham/XoaSanPham={idSanPham}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        [HttpPost]
        public async Task<IActionResult> EditSanPham([FromBody]SanPhamDTO sanPham)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/SanPham/SuaSanPham",sanPham);
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        public async Task<IActionResult> DeleteThuongHieu(string idThuongHieu)
        {
            var response = await _httpClient.DeleteAsync($"/api/ThuongHieu/{idThuongHieu}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        [HttpPost]
        public async Task<IActionResult> EditThuongHieu([FromBody] ThuongHieuDTO thuongHieu)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/thuonghieu/sua-thuong-hieu", thuongHieu);
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        public async Task<IActionResult> DeleteLoaiGiay(string idLoaiGiay)
        {
            var response = await _httpClient.DeleteAsync($"/api/LoaiGiay/Delete?idLoaiGiay={idLoaiGiay}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        [HttpPost]
        public async Task<IActionResult> EditLoaiGiay([FromBody] LoaiGiayDTO loaiGiay)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/loaigiay/sua-loai-giay", loaiGiay);
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        public async Task<IActionResult> DeleteKieuDeGiay(string idKieuDeGiay)
        {
            var response = await _httpClient.DeleteAsync($"/api/KieuDeGiay/XoaKieuDeGiay={idKieuDeGiay}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        [HttpPost]
        public async Task<IActionResult> EditKieuDeGiay([FromBody] KieuDeGiayDTO kieuDeGiayDTO)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/kieudegiay/sua-kieu-de-giay", kieuDeGiayDTO);
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        public async Task<IActionResult> DeleteXuatXu(string idXuatXu)
        {
            var response = await _httpClient.DeleteAsync($"/api/XuatXu/DeleteXuatXu/{idXuatXu}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        [HttpPost]
        public async Task<IActionResult> EditXuatXu([FromBody] XuatXuDTO xuatXuDTO)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/xuatxu/sua-xuat-xu", xuatXuDTO);
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        public async Task<IActionResult> DeleteChatLieu(string idChatLieu)
        {
            var response = await _httpClient.DeleteAsync($"/api/ChatLieu/XoaChatLieu/{idChatLieu}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        [HttpPost]
        public async Task<IActionResult> EditChatLieu([FromBody] ChatLieuDTO chatLieuDTO)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/chatlieu/sua-chat-lieu", chatLieuDTO);
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        public async Task<IActionResult> DeleteMauSac(string idMauSac)
        {
            var response = await _httpClient.DeleteAsync($"/api/MauSac/DeleteMauSac/{idMauSac}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        [HttpPost]
        public async Task<IActionResult> EditMauSac([FromBody] MauSacDTO mauSacDTO)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/mausac/sua-mau-sac", mauSacDTO);
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        public async Task<IActionResult> DeleteKichCo(string idKichCo)
        {
            var response = await _httpClient.DeleteAsync($"/api/KichCo/{idKichCo}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        [HttpPost]
        public async Task<IActionResult> EditKichCo([FromBody] KichCoDTO kichCoDTO)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/kichco/sua-kich-co", kichCoDTO);
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

        #endregion
    }
}
