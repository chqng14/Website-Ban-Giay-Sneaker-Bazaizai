using App_Data.Models;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Controllers
{
    public class PTThanhToanChiTietController : Controller
    {
        IPTThanhToanChiTietServices PTThanhToanChiTietServices;
        public PTThanhToanChiTietController()
        {
            PTThanhToanChiTietServices = new PTThanhToanChiTietServices();
        }
        // GET: PTThanhToanChiTietController
        public async Task<List<PhuongThucThanhToanChiTiet>> GetAll()
        {
            return await PTThanhToanChiTietServices.GetAllPTThanhToanChiTietAsync();
        }

        // GET: PTThanhToanChiTietController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PTThanhToanChiTietController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PTThanhToanChiTietController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> CreatePTThanhToanChiTiet(string IdHoaDon, string IdThanhToan, double SoTien)
        {
            return await PTThanhToanChiTietServices.CreatePTThanhToanChiTietAsync(IdHoaDon, IdThanhToan, SoTien);
        }

        // GET: PTThanhToanChiTietController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PTThanhToanChiTietController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string IdPhuongThucThanhToanChiTiet, int TrangThai)
        {
            await PTThanhToanChiTietServices.UpdatePTThanhToanChiTietAsync(IdPhuongThucThanhToanChiTiet, TrangThai);
            return Ok();
        }

        // GET: PTThanhToanChiTietController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PTThanhToanChiTietController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
