using App_Data.DbContextt;
using App_View.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Controllers
{
    public class VoucherNguoiDungController : Controller
    {

        private readonly BazaizaiContext _context;
        private readonly IVoucherNguoiDungServices _voucherND;
        public VoucherNguoiDungController(IVoucherNguoiDungServices voucherNDServices)
        {
            _voucherND = voucherNDServices;
            _context = new BazaizaiContext();
        }
        // GET: VoucherNguoiDungController
        public ActionResult Voucher_wallet()
        {
            return View();
        }

        // GET: VoucherNguoiDungController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VoucherNguoiDungController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VoucherNguoiDungController/Create
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

        // GET: VoucherNguoiDungController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VoucherNguoiDungController/Edit/5
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

        // GET: VoucherNguoiDungController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VoucherNguoiDungController/Delete/5
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
