using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Data.DbContextt;
using App_Data.Models;

using App_Data.ViewModels.GioHangChiTiet;
using App_View.Services;
using App_View.IServices;

namespace App_View.Controllers
{
    public class GioHangChiTietsController : Controller
    {
        private readonly HttpClient httpClient;
        IGioHangChiTietServices GioHangChiTietServices;
        public GioHangChiTietsController()
        {
            httpClient = new HttpClient();
            GioHangChiTietServices = new GioHangChiTietServices();
        }

        // GET: GioHangChiTiets
        public async Task<IActionResult> ShowCartUser()
        {
            //var acc = SessionServices.GetObjFromSession(HttpContext.Session, "acc").TaiKhoan;
            //var idCart = (await userServices.GetAllUser()).FirstOrDefault(c => c.TaiKhoan == acc).Id;
            var giohang = (await GioHangChiTietServices.GetAllGioHang()).Where(c => c.IdNguoiDung == "43037").ToList();
            return View(giohang);
        }

        // GET: GioHangChiTiets/Details/5
        public async Task<IActionResult> Details(string id)
        {


            return View();
        }

        // GET: GioHangChiTiets/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: GioHangChiTiets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGioHangChiTiet,IdNguoiDung,IdSanPhamCT,Soluong,GiaGoc,TrangThai")] GioHangChiTiet gioHangChiTiet)
        {

            return View();
        }

        // GET: GioHangChiTiets/Edit/5
        public async Task<IActionResult> CapNhatSoLuongGioHang(string IdGioHangChiTiet, int SoLuong, string IdSanPhamChiTiet)
        {
            var jsonupdate = await GioHangChiTietServices.UpdateGioHang(IdGioHangChiTiet, SoLuong);
            return Ok(jsonupdate);
        }

        // POST: GioHangChiTiets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdGioHangChiTiet,IdNguoiDung,IdSanPhamCT,Soluong,GiaGoc,TrangThai")] GioHangChiTiet gioHangChiTiet)
        {

            return View();
        }

        // GET: GioHangChiTiets/Delete/5
        public async Task<IActionResult> Delete(string id)
        {


            return View();
        }

        // POST: GioHangChiTiets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            return RedirectToAction(nameof(Index));
        }

        //private bool GioHangChiTietExists(string id)
        //{
        //    return View();
        //}
    }
}
