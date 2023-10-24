using App_Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App_View.Controllers
{
    public class KhuyenMaiController : Controller
    {
        public HttpClient httpClient { get; set; }
        public KhuyenMaiController()
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> GetAllKhuyenMai()
        {
            var KhuyenMais = JsonConvert.DeserializeObject<List<KhuyenMai>>(await (await httpClient.GetAsync("https://localhost:7038/api/KhuyenMai")).Content.ReadAsStringAsync());
            return View(KhuyenMais);
        }
        public async Task<IActionResult> DetailKhuyenMai(string id)
        {

            var kichCo = (JsonConvert.DeserializeObject<List<KhuyenMai>>(await (await httpClient.GetAsync("https://localhost:7038/api/KhuyenMai")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdKhuyenMai == id);
            return View(kichCo);

        }

        public ActionResult CreateKhuyenMai()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateKhuyenMai(KhuyenMai q)
        {
            await httpClient.PostAsync($"https://localhost:7038/api/KhuyenMai?Ten={q.TenKhuyenMai}&ngayBD={q.NgayBatDau}&ngayKT={q.NgayKetThuc}&trangThai={q.TrangThai}&mucGiam={q.MucGiam}&loaiHinh={q.LoaiHinhKM}", null);
            return RedirectToAction("GetAllKhuyenMai");
        }


        public async Task<IActionResult> DeleteKhuyenMai(string id)
        {
            await httpClient.DeleteAsync($"https://localhost:7038/api/KhuyenMai/{id}");
            return RedirectToAction("GetAllKhuyenMai");
        }

        public async Task<IActionResult> EditKhuyenMai(string IdKhuyenMai)
        {
            var kichCo = (JsonConvert.DeserializeObject<List<KhuyenMai>>(await (await httpClient.GetAsync("https://localhost:7038/api/KhuyenMai")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdKhuyenMai == IdKhuyenMai);

            if (kichCo == null)
            {
                return NotFound();
            }

            return View(kichCo);
        }
        [HttpPost]

        public async Task<IActionResult> EditKhuyenMai(KhuyenMai q)
        {
            var apiUrl = $"https://localhost:7038/api/KhuyenMai/{q.IdKhuyenMai}?Ten={q.TenKhuyenMai}&ngayBD={q.NgayBatDau}&ngayKT={q.NgayKetThuc}&trangThai={q.TrangThai}&mucGiam={q.MucGiam}&loaiHinh={q.LoaiHinhKM}";

            var response = await httpClient.PutAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllKhuyenMai");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
