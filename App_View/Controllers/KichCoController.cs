using App_Data.Models;
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
        public async Task<IActionResult> CreateKichCo(KichCo p)
        {
            JsonConvert.DeserializeObject<KichCo>(await (await httpClient.PostAsync($"https://localhost:7038/api/KichCo?TrangThai={p.TrangThai}&KichCo={p.SoKichCo}", null)).Content.ReadAsStringAsync());
            return RedirectToAction("GetAllKickCo");
        }



        //public async Task<IActionResult> DeleteSale(Guid id)
        //{
        //    if (await SaleService.DeleteSale(id))
        //    {
        //        return RedirectToAction("GetAllSale");
        //    }
        //    else return BadRequest();
        //}
        //public async Task<IActionResult> EditSale(Guid id)
        //{
        //    var sp = (await SaleService.GetAllSale()).FirstOrDefault(x => x.Id == id);

        //    if (sp == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(sp);
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditSale(Sale p)
        //{
        //    if (await SaleService.EditSale(p))
        //    {
        //        return RedirectToAction("GetAllSale");
        //    }
        //    else return BadRequest();
        //}
    }
}
