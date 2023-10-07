using App_Data.Models;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Controllers
{
    public class ThongTinGHController : Controller
    {
        IThongTinGHServices thongTinGHServices;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        public ThongTinGHController(SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager)
        {
            thongTinGHServices = new ThongTinGHServices();
            _signInManager = signInManager;
            _userManager = userManager;
        }
        // GET: ThongTinGHController
        public ActionResult ShowByIdUser()
        {
            var idNguoiDung = _userManager.GetUserId(User);
            var thongTinGH = thongTinGHServices.GetThongTinByIdUser(idNguoiDung);
            ViewData["ThongTinGH"] = thongTinGH;
            return Ok();
        }

        // GET: ThongTinGHController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ThongTinGHController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ThongTinGHController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ThongTinGHController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ThongTinGHController/Edit/5
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

        // GET: ThongTinGHController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ThongTinGHController/Delete/5
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
