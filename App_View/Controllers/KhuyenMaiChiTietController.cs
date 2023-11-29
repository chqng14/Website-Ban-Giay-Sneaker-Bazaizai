using App_Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App_View.Controllers
{
    public class KhuyenMaiChiTietController : Controller
    {
        public HttpClient httpClient { get; set; }
        public KhuyenMaiChiTietController()
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> GetAllKhuyenMaiChiTiet()
        {
            var KhuyenMaiChiTiet = JsonConvert.DeserializeObject<List<KhuyenMaiChiTiet>>(await (await httpClient.GetAsync("https://bazaizaiapi.azurewebsites.net/api/KhuyenMaiChiTiet")).Content.ReadAsStringAsync());
            return View(KhuyenMaiChiTiet);
        }
        public async Task<IActionResult> DetailKhuyenMaiChiTiet(string id)
        {

            var kichCo = (JsonConvert.DeserializeObject<List<KhuyenMaiChiTiet>>(await (await httpClient.GetAsync("https://bazaizaiapi.azurewebsites.net/api/KhuyenMaiChiTiet")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdKhuyenMaiChiTiet == id);
            return View(kichCo);

        }

        public ActionResult CreateKhuyenMaiChiTiet()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateKhuyenMaiChiTiet(KhuyenMaiChiTiet q)
        {
            await httpClient.PostAsync($"https://bazaizaiapi.azurewebsites.net/api/KhuyenMaiChiTiet?mota={q.MoTa}&trangThai={q.TrangThai}&IDKm={q.IdKhuyenMai}&IDSpCt={q.IdSanPhamChiTiet}", null);
            return RedirectToAction("GetAllKhuyenMaiChiTiet");
        }


        public async Task<IActionResult> DeleteKhuyenMaiChiTiet(string id)
        {
            await httpClient.DeleteAsync($"https://bazaizaiapi.azurewebsites.net/api/KhuyenMaiChiTiet/{id}");
            return RedirectToAction("GetAllKhuyenMaiChiTiet");
        }

        public async Task<IActionResult> EditKhuyenMaiChiTiet(string IdKhuyenMaiChiTiet)
        {
            var kichCo = (JsonConvert.DeserializeObject<List<KhuyenMaiChiTiet>>(await (await httpClient.GetAsync("https://bazaizaiapi.azurewebsites.net/api/KhuyenMaiChiTiet")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdKhuyenMaiChiTiet == IdKhuyenMaiChiTiet);

            if (kichCo == null)
            {
                return NotFound();
            }

            return View(kichCo);
        }
        [HttpPost]

        public async Task<IActionResult> EditKhuyenMaiChiTiet(KhuyenMaiChiTiet a)
        {
            var apiUrl = $"https://bazaizaiapi.azurewebsites.net/api/KhuyenMaiChiTiet/{a.IdKhuyenMaiChiTiet}?mota={a.MoTa}&trangThai={a.TrangThai}&IDKm={a.IdKhuyenMai}&IDSpCt={a.IdSanPhamChiTiet}";

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
    }
}
