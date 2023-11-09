using App_Data.Models;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Controllers
{
    public class PTThanhToanController : Controller
    {
        IPTThanhToanServices PTThanhToanServices;
        public PTThanhToanController()
        {
            PTThanhToanServices = new PTThanhToanServices();
        }
        // GET: PTThanhToanController
        public async Task<List<PhuongThucThanhToan>> GetAll()
        {
            return await PTThanhToanServices.GetAllPTThanhToanAsync();
        }

        // GET: PTThanhToanController/Details/5
        public async Task<string> GetPTThanhToanByName(string ten)
        {
            string ptThanhToan = await PTThanhToanServices.GetPTThanhToanByNameAsync(ten);
            return ptThanhToan;
        }

        // GET: PTThanhToanController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PTThanhToanController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePTThanhToan(string ten, string mota, int trangthai)
        {
            await PTThanhToanServices.CreatePTThanhToanAsync(ten, mota, trangthai);
            return Ok();
        }

        // GET: PTThanhToanController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PTThanhToanController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: PTThanhToanController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PTThanhToanController/Delete/5
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
