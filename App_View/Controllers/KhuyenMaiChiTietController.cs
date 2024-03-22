using App_Data.Models;
using App_View.IServices;
using App_View.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static App_Data.Repositories.TrangThai;

namespace App_View.Controllers
{
    public class KhuyenMaiChiTietController : Controller
    {
        public HttpClient httpClient { get; set; }
        private readonly IKhuyenMaiChiTietservices KhuyenMaiChiTietservices;
        private readonly IKhuyenMaiservices KhuyenMaiservices;
        private readonly ISanPhamChiTietservice SanPhamChiTietservice;
        public KhuyenMaiChiTietController(IKhuyenMaiChiTietservices KhuyenMaiChiTietservices, IKhuyenMaiservices KhuyenMaiservices, ISanPhamChiTietservice SanPhamChiTietservice)
        {
            httpClient = new HttpClient();
            this.KhuyenMaiChiTietservices = KhuyenMaiChiTietservices;
            this.KhuyenMaiservices = KhuyenMaiservices;
            this.SanPhamChiTietservice = SanPhamChiTietservice;
        }

        public async Task<IActionResult> GetAllKhuyenMaiChiTiet()
        {
            var KhuyenMaiChiTiet = JsonConvert.DeserializeObject<List<KhuyenMaiChiTiet>>(await (await httpClient.GetAsync("https://localhost:7038/api/KhuyenMaiChiTiet")).Content.ReadAsStringAsync());
            return View(KhuyenMaiChiTiet);
        }
        public async Task<IActionResult> DetailKhuyenMaiChiTiet(string id)
        {

            var kichCo = (JsonConvert.DeserializeObject<List<KhuyenMaiChiTiet>>(await (await httpClient.GetAsync("https://localhost:7038/api/KhuyenMaiChiTiet")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdKhuyenMaiChiTiet == id);
            return View(kichCo);

        }

        public ActionResult CreateKhuyenMaiChiTiet()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateKhuyenMaiChiTiet(KhuyenMaiChiTiet q)
        {
            await httpClient.PostAsync($"https://localhost:7038/api/KhuyenMaiChiTiet?mota={q.MoTa}&trangThai={q.TrangThai}&IDKm={q.IdKhuyenMai}&IDSpCt={q.IdSanPhamChiTiet}", null);
            return RedirectToAction("GetAllKhuyenMaiChiTiet");
        }


        public async Task<IActionResult> DeleteKhuyenMaiChiTiet(string id)
        {
            await httpClient.DeleteAsync($"https://localhost:7038/api/KhuyenMaiChiTiet/{id}");
            return RedirectToAction("GetAllKhuyenMaiChiTiet");
        }

        public async Task<IActionResult> EditKhuyenMaiChiTiet(string IdKhuyenMaiChiTiet)
        {
            var kichCo = (JsonConvert.DeserializeObject<List<KhuyenMaiChiTiet>>(await (await httpClient.GetAsync("https://localhost:7038/api/KhuyenMaiChiTiet")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdKhuyenMaiChiTiet == IdKhuyenMaiChiTiet);

            if (kichCo == null)
            {
                return NotFound();
            }

            return View(kichCo);
        }
        [HttpPost]

        public async Task<IActionResult> EditKhuyenMaiChiTiet(KhuyenMaiChiTiet a)
        {
            var apiUrl = $"https://localhost:7038/api/KhuyenMaiChiTiet/{a.IdKhuyenMaiChiTiet}?mota={a.MoTa}&trangThai={a.TrangThai}&IDKm={a.IdKhuyenMai}&IDSpCt={a.IdSanPhamChiTiet}";

            var response = await httpClient.PutAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllKhuyenMaiChiTiet");
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult ViewKhuyenMai()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> KhuyenMaiDongGiaAsync(string idKhuyenMai)
        {
            var kmct = (await KhuyenMaiChiTietservices.GetAllKhuyenMaiChiTiet()).Where(x => x.TrangThai == (int)TrangThaiSaleDetail.DangKhuyenMai&&x.IdKhuyenMai== idKhuyenMai).GroupBy(x=>x.SanPham).Select(x=>x.First()).ToList();
            var model = new App_Data.ViewModels.SanPhamChiTietViewModel.DanhSachGiayViewModel();
            model = SanPhamChiTietservice.GetDanhSachGiayViewModelAynsc().Result;
            var lstSpDuocApDungKhuyenMai = model.LstAllSanPham.Where(x => x.GiaThucTe < x.GiaGoc).ToList();
            ViewBag.lstSpct = lstSpDuocApDungKhuyenMai.ToList();
            var khuyenMai = (await KhuyenMaiservices.GetAllKhuyenMai()).Where(x => x.TrangThai == (int)TrangThaiSale.DangBatDau);
            ViewBag.KhuyemMai = khuyenMai;
            return PartialView(kmct);
        }
    }
}
