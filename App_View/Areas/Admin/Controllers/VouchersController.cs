using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Data.DbContextt;
using App_Data.Models;
using App_View.IServices;
using App_View.Services;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VouchersController : Controller
    {
        private readonly BazaizaiContext _context;
        private readonly IVoucherServices _voucherSV;

        public VouchersController(IVoucherServices voucherServices)
        {
            _voucherSV = voucherServices;
            _context = new BazaizaiContext();
        }

        // GET: Admin/Vouchers
        public async Task<IActionResult> Index()
        {
            return _context.vouchers != null ?
                        View(await _context.vouchers.ToListAsync()) :
                        Problem("Entity set 'BazaizaiContext.vouchers'  is null.");
        }

        // GET: Admin/Vouchers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.vouchers
                .FirstOrDefaultAsync(m => m.IdVoucher == id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // GET: Admin/Vouchers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Vouchers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVoucher,MaVoucher,TenVoucher,DieuKien,LoaiHinhUuDai,SoLuong,MucUuDai,NgayBatDau,NgayKetThuc,TrangThai")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(voucher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voucher);
        }

        // GET: Admin/Vouchers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.vouchers.FindAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }
            return View(voucher);
        }

        // POST: Admin/Vouchers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdVoucher,MaVoucher,TenVoucher,DieuKien,LoaiHinhUuDai,SoLuong,MucUuDai,NgayBatDau,NgayKetThuc,TrangThai")] Voucher voucher)
        {
            if (id != voucher.IdVoucher)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voucher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherExists(voucher.IdVoucher))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(voucher);
        }

        // GET: Admin/Vouchers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.vouchers
                .FirstOrDefaultAsync(m => m.IdVoucher == id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // POST: Admin/Vouchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.vouchers == null)
            {
                return Problem("Entity set 'BazaizaiContext.vouchers'  is null.");
            }
            var voucher = await _context.vouchers.FindAsync(id);
            if (voucher != null)
            {
                _context.vouchers.Remove(voucher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoucherExists(string id)
        {
            return (_context.vouchers?.Any(e => e.IdVoucher == id)).GetValueOrDefault();
        }
    }
}
