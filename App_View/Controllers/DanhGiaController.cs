using App_Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App_View.Controllers
{
    public class DanhGiaController : Controller
    {
        public HttpClient httpClient { get; set; }
        public DanhGiaController()
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> GetAllDanhGia()
        {
             await httpClient.GetFromJsonAsync<List<DanhGia>>("https://localhost:7038/api/DanhGia");
            var lst = JsonConvert.DeserializeObject<List<DanhGia>>(await (await httpClient.GetAsync("https://localhost:7038/api/DanhGia")).Content.ReadAsStringAsync());
            return View(lst);
        }
        public async Task<IActionResult> DetailDanhGia(string id)
        {

            var kichCo = (JsonConvert.DeserializeObject<List<ThuongHieu>>(await (await httpClient.GetAsync("https://localhost:7038/api/ThuongHieu")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdThuongHieu == id);
            return View(kichCo);

        }

        public ActionResult CreateDanhGia()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDanhGia(ThuongHieu q)
        {
            await httpClient.PostAsync($"https://localhost:7038/api/ThuongHieu?TrangThai={q.TrangThai}&Ten={q.TenThuongHieu}", null);
            return RedirectToAction("GetAllThuongHieu");
        }


        public async Task<IActionResult> DeleteDanhGia(string id)
        {
            await httpClient.DeleteAsync($"https://localhost:7038/api/ThuongHieu/{id}");
            return RedirectToAction("GetAllThuongHieu");
        }

        public async Task<IActionResult> EditDanhGia(string Id)
        {
            var kichCo = (JsonConvert.DeserializeObject<List<ThuongHieu>>(await (await httpClient.GetAsync("https://localhost:7038/api/ThuongHieu")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdThuongHieu == Id);

            if (kichCo == null)
            {
                return NotFound();
            }

            return View(kichCo);
        }
        [HttpPost]

        public async Task<IActionResult> EditDanhGia(ThuongHieu a)
        {
            var apiUrl = $"https://localhost:7038/api/ThuongHieu/{a.IdThuongHieu}?TrangThai={a.TrangThai}&Ten={a.TenThuongHieu}";

            var response = await httpClient.PutAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllThuongHieu");
            }
            else
            {
                return BadRequest();
            }
        }

        //public async Task<IActionResult> AddLike(string Id)
        //{
        //    var kichCo = (JsonConvert.DeserializeObject<List<DanhGia>>(await (await httpClient.GetAsync("https://localhost:7038/api/ThuongHieu")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdThuongHieu == Id);

        //    if (kichCo == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(kichCo);
        //}
        //[HttpPost]

        //public async Task<IActionResult> AddLike()
        //{
        //    var apiUrl = $"https://localhost:7038/api/ThuongHieu/{a.IdThuongHieu}?TrangThai={a.TrangThai}&Ten={a.TenThuongHieu}";

        //    var response = await httpClient.PutAsync(apiUrl, null);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("GetAllThuongHieu");
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

    }
}
