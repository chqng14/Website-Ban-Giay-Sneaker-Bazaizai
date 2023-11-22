using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.DanhGia;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin, NhanVien")]
    public class DanhGiaController : Controller
    {
        private IDanhGiaService _danhGiaService;
        public HttpClient httpClient { get; set; }

        public DanhGiaController(IDanhGiaService danhGiaService)
        {
            httpClient = new HttpClient();
            _danhGiaService = danhGiaService;
        }

        public async Task<IActionResult> Index()
        {
            var lst = await _danhGiaService.GetAllDanhGia();
            return View(lst);
        }
        //public async Task<IActionResult> _GetAllDanhGia()
        //{
        //    var lst = JsonConvert.DeserializeObject<List<DanhGia>>(await (await httpClient.GetAsync("https://localhost:7038/api/DanhGia")).Content.ReadAsStringAsync()).ToList();
        //    return PartialView("_GetAllDanhGia", lst);
        //}
        //public async Task<IActionResult> GetAllDanhGiaView()
        //{
        //    var lst = await GetAllDanhGia();
        //    ViewBag.DanhGias = lst;
        //    return View();
        //}
        //public async Task<IActionResult> _ChildComment(string id)
        //{
        //    var lst = await GetAllDanhGia();
        //    var data = lst.FirstOrDefault(s => s.ParentId == id);
        //    return PartialView("_ChildComment", data);
        //}
      








        //public async Task<IActionResult> TongSoDanhGiaCuaMoiSpChuaDuyet()
        //{
        //    var result = await _danhGiaService.TongSoDanhGiaCuaMoiSpChuaDuyet();

        //    // Chuyển đổi từ Tuple sang DanhGiaResult
        //    var danhGiaResults = result.Select(tuple => new DanhGiaResult
        //    {
        //        SanPham = tuple.Item1,
        //        SoLuongDanhGiaChuaDuyet = tuple.Item2,
        //        IdSanPham = tuple.Item3,
        //    }).ToList();
        //    return View(result);

        //} 
        public async Task<IActionResult> TongSoDanhGiaCuaMoiSpChuaDuyet()
        {
            //string apiUrl = "https://localhost:7038/api/DanhGia/GetTongSoDanhGiaCuaMoiSpChuaDuyet";
            //using (var httpClient = new HttpClient())
            //{
            //    var apiData = await httpClient.GetStringAsync(apiUrl);
            //    var result = JsonConvert.DeserializeObject<List<DanhGiaResult>>(apiData);

            //    var danhGiaResults = result.Select(item => new DanhGiaResult
            //    {
            //        SanPham = item.SanPham,
            //        SoLuongDanhGiaChuaDuyet = item.SoLuongDanhGiaChuaDuyet,
            //        IdSanPham = item.IdSanPham,
            //    }).ToList();

            //    return View(result);
            //}
            var result = await _danhGiaService.TongSoDanhGiaCuaMoiSpChuaDuyet();
            var danhGiaResults = result.Select(item => new DanhGiaResult
            {
                SanPham = item.SanPham,
                SoLuongDanhGiaChuaDuyet = item.SoLuongDanhGiaChuaDuyet,
                IdSanPham = item.IdSanPham,
                IdChiTietSp = item.IdChiTietSp,
            }).ToList();

            return View(result);
        }

        public async Task<IActionResult> LstChiTietDanhGiaCuaMoiSpChuaDuyet(string idSanPham)
        {
            var lst = await _danhGiaService.LstChiTietDanhGiaCuaMoiSpChuaDuyet(idSanPham);
            return View(lst);
        }

        public async Task<IActionResult> DuyetDanhGia(string id, string idSanPham)
        {

            var result = await _danhGiaService.DuyetDanhGia(id);

            if (result)
            {
                var lst = await _danhGiaService.LstChiTietDanhGiaCuaMoiSpChuaDuyet(idSanPham);
                if (lst.Count() == 0) return RedirectToAction("TongSoDanhGiaCuaMoiSpChuaDuyet");
                else
                    return RedirectToAction("LstChiTietDanhGiaCuaMoiSpChuaDuyet", new { idSanPham });
            }
            else
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> DuyetNhieuDanhGia(List<string> ids, string idSanPham)
        {
            foreach (var id in ids)
            {
                await _danhGiaService.DuyetDanhGia(id);
            }
            var lst = await _danhGiaService.LstChiTietDanhGiaCuaMoiSpChuaDuyet(idSanPham);
            if (lst.Count() == 0) return RedirectToAction("TongSoDanhGiaCuaMoiSpChuaDuyet");
            else
                return RedirectToAction("LstChiTietDanhGiaCuaMoiSpChuaDuyet", new { idSanPham });
        }

        public async Task<IActionResult> DuyetTatCaDanhGiaCuaMSp(string idSanPham)
        {
            var lst = await _danhGiaService.LstChiTietDanhGiaCuaMoiSpChuaDuyet(idSanPham);
            foreach (var item in lst)
            {
                await _danhGiaService.DuyetDanhGia(item.IdDanhGia);
            }
            return RedirectToAction("TongSoDanhGiaCuaMoiSpChuaDuyet");
        }
    }
}
