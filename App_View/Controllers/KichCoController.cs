using App_Data.Models;
using App_Data.Repositories;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenXmlPowerTools;
using System.Net.Http;

namespace App_View.Controllers
{
    public class KichCoController : Controller
    {
        public HttpClient httpClient { get; set; }
        public KichCoController()
        {
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> GetAllKickCo()
        {
            var KichCos = JsonConvert.DeserializeObject<List<KichCo>>(await (await httpClient.GetAsync("https://localhost:7038/api/KichCo")).Content.ReadAsStringAsync());
            return View(KichCos);
        }
        public async Task<IActionResult> DetailKickCo(string id)
        {

            var kichCo = (JsonConvert.DeserializeObject<List<KichCo>>(await (await httpClient.GetAsync("https://localhost:7038/api/KichCo")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdKichCo == id);
            return View(kichCo);

        }

        public ActionResult CreateKichCo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateKichCo(KichCo q)
        {
            await httpClient.PostAsync($"https://localhost:7038/api/KichCo?TrangThai={q.TrangThai}&KichCo={q.SoKichCo}", null);
            return RedirectToAction("GetAllKickCo");
        }


        public async Task<IActionResult> DeleteKichCo(string id)
        {
            await httpClient.DeleteAsync($"https://localhost:7038/api/KichCo/{id}");
            return RedirectToAction("GetAllKickCo");
        }

        public async Task<IActionResult> EditKichCo(string IdKichCo)
        {
            var kichCo = (JsonConvert.DeserializeObject<List<KichCo>>(await (await httpClient.GetAsync("https://localhost:7038/api/KichCo")).Content.ReadAsStringAsync())).FirstOrDefault(x => x.IdKichCo ==IdKichCo);

            if (kichCo == null)
            {
                return NotFound();
            }

            return View(kichCo);
        }
        [HttpPost]

        public async Task<IActionResult> EditKichCo(KichCo a)
        {
            var apiUrl = $"https://localhost:7038/api/KichCo/{a.IdKichCo}?TrangThai={a.TrangThai}&KichCo={a.SoKichCo}";

            var response = await httpClient.PutAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllKickCo"); 
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
