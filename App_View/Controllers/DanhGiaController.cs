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
        public async Task<List<DanhGia>> GetAllDanhGia()
        {
            var lst = JsonConvert.DeserializeObject<List<DanhGia>>(await (await httpClient.GetAsync("https://localhost:7038/api/DanhGia/GetAllDanhGia")).Content.ReadAsStringAsync());
          
            return lst;
        }
        //public async Task<IActionResult> _GetAllDanhGia()
        //{
        //    var lst = JsonConvert.DeserializeObject<List<DanhGia>>(await (await httpClient.GetAsync("https://localhost:7038/api/DanhGia")).Content.ReadAsStringAsync()).ToList();
        //    return PartialView("_GetAllDanhGia", lst);
        //}
        public async Task<IActionResult> GetAllDanhGiaView()
        {
            var lst= await GetAllDanhGia();
            ViewBag.DanhGias = lst;
            return View();
        }
        public async Task<IActionResult> _ChildComment(string id)
        {
            var lst = await GetAllDanhGia();
            var data = lst.FirstOrDefault(s => s.ParentId == id);
            return PartialView("_ChildComment", data);
        }
        public async Task<IActionResult> DetailThuongHieu(string id)
        {

            var kichCo = (JsonConvert.DeserializeObject<List<ThuongHieu>>(await (await httpClient.GetAsync("https://localhost:7038/api/ThuongHieu")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdThuongHieu == id);
            return View(kichCo);

        }

        public ActionResult CreateThuongHieu()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateThuongHieu(ThuongHieu q)
        {
            await httpClient.PostAsync($"https://localhost:7038/api/ThuongHieu?TrangThai={q.TrangThai}&Ten={q.TenThuongHieu}", null);
            return RedirectToAction("GetAllThuongHieu");
        }


        public async Task<IActionResult> DeleteThuongHieu(string id)
        {
            await httpClient.DeleteAsync($"https://localhost:7038/api/ThuongHieu/{id}");
            return RedirectToAction("GetAllThuongHieu");
        }

        public async Task<IActionResult> EditThuongHieu(string Id)
        {
            var kichCo = (JsonConvert.DeserializeObject<List<ThuongHieu>>(await (await httpClient.GetAsync("https://localhost:7038/api/ThuongHieu")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdThuongHieu == Id);

            if (kichCo == null)
            {
                return NotFound();
            }

            return View(kichCo);
        }
        [HttpPost]

        public async Task<IActionResult> EditThuongHieu(ThuongHieu a)
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

    }
}
