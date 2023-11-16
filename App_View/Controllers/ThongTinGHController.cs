using App_Data.Models;
using App_Data.ViewModels.ThongTinGHDTO;
using App_View.IServices;
using App_View.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static App_Data.Repositories.TrangThai;

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

        public async Task<IEnumerable<ThongTinGiaoHang>> GetThongTinGiaoHang()
        {
            var idNguoiDung = _userManager.GetUserId(User);
            var thongTinGH = await thongTinGHServices.GetThongTinByIdUser(idNguoiDung);
            return thongTinGH;
        }


        public async Task<ActionResult> ShowByIdUser()
        {
            var thongTinGH = await GetThongTinGiaoHang();
            return View(thongTinGH);
        }

        public async Task<ActionResult> Default()
        {
            var thongTinGH = await GetThongTinGiaoHang();
            return View(thongTinGH);
        }
        // GET: ThongTinGHController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ThongTinGHController/Create
        public ActionResult CreateThongTin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateThongTin(ThongTinGHDTO thongTinGHDTO)
        {
            thongTinGHDTO.IdThongTinGH = Guid.NewGuid().ToString();
            thongTinGHDTO.IdNguoiDung = _userManager.GetUserId(User);
            thongTinGHDTO.TrangThai = (int)TrangThaiThongTinGH.HoatDong;
            await thongTinGHServices.CreateThongTin(thongTinGHDTO);
            return View();
        }
        // POST: ThongTinGHController/Create
        public async Task<IActionResult> CreateThongTinBill(ThongTinGHDTO thongTinGHDTO)
        {
            await thongTinGHServices.CreateThongTin(thongTinGHDTO);
            return Ok();
        }

        //GET: ThongTinGHController/Edit/5
        public async Task<IActionResult> EditThongTin(string idThongTin)
        {
            var thongTinGH = (await thongTinGHServices.GetAllThongTinDTO()).FirstOrDefault(c => c.IdThongTinGH == idThongTin);
            return View(thongTinGH);
        }

        //POST: ThongTinGHController/Edit/5
        [HttpPost]
        public async Task<IActionResult> EditThongTin(ThongTinGHDTO thongTinGHDTO)
        {
            await thongTinGHServices.UpdateThongTin(thongTinGHDTO);
            return Ok(new { idThongTin = thongTinGHDTO.IdThongTinGH });
        }
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTrangThai(string idThongTin)
        {
            await thongTinGHServices.UpdateTrangThaiThongTin(idThongTin);
            return Ok();
        }

        // GET: ThongTinGHController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: ThongTinGHController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string idThongTin)
        {
            var thongtin = (await GetThongTinGiaoHang()).FirstOrDefault(c => c.IdThongTinGH == idThongTin);
            if (thongtin.TrangThai == 0)
            {
                return Json(new { status = 0 });
            }
            else
            {
                await thongTinGHServices.DeleteThongTin(idThongTin);
                return RedirectToAction("ShowByIdUser");

            }
        }
    }
}
