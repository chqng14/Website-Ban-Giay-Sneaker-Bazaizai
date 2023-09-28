using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Data.DbContextt;
using App_Data.Models;

namespace App_View.Controllers
{
    public class GioHangChiTietsController : Controller
    {
        private readonly BazaizaiContext _context;

        public GioHangChiTietsController(BazaizaiContext context)
        {
            _context = context;
        }

        // GET: GioHangChiTiets
        public async Task<IActionResult> Index()
        {
            var bazaizaiContext = _context.gioHangChiTiets.Include(g => g.GioHang).Include(g => g.SanPhamChiTiet);
            return View(await bazaizaiContext.ToListAsync());
        }

        // GET: GioHangChiTiets/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.gioHangChiTiets == null)
            {
                return NotFound();
            }

            var gioHangChiTiet = await _context.gioHangChiTiets
                .Include(g => g.GioHang)
                .Include(g => g.SanPhamChiTiet)
                .FirstOrDefaultAsync(m => m.IdGioHangChiTiet == id);
            if (gioHangChiTiet == null)
            {
                return NotFound();
            }

            return View(gioHangChiTiet);
        }

        // GET: GioHangChiTiets/Create
        public IActionResult Create()
        {
            ViewData["IdNguoiDung"] = new SelectList(_context.gioHangs, "IdNguoiDung", "IdNguoiDung");
            ViewData["IdSanPhamCT"] = new SelectList(_context.sanPhamChiTiets, "IdChiTietSp", "IdChiTietSp");
            return View();
        }

        // POST: GioHangChiTiets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGioHangChiTiet,IdNguoiDung,IdSanPhamCT,Soluong,GiaGoc,TrangThai")] GioHangChiTiet gioHangChiTiet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gioHangChiTiet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdNguoiDung"] = new SelectList(_context.gioHangs, "IdNguoiDung", "IdNguoiDung", gioHangChiTiet.IdNguoiDung);
            ViewData["IdSanPhamCT"] = new SelectList(_context.sanPhamChiTiets, "IdChiTietSp", "IdChiTietSp", gioHangChiTiet.IdSanPhamCT);
            return View(gioHangChiTiet);
        }

        // GET: GioHangChiTiets/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.gioHangChiTiets == null)
            {
                return NotFound();
            }

            var gioHangChiTiet = await _context.gioHangChiTiets.FindAsync(id);
            if (gioHangChiTiet == null)
            {
                return NotFound();
            }
            ViewData["IdNguoiDung"] = new SelectList(_context.gioHangs, "IdNguoiDung", "IdNguoiDung", gioHangChiTiet.IdNguoiDung);
            ViewData["IdSanPhamCT"] = new SelectList(_context.sanPhamChiTiets, "IdChiTietSp", "IdChiTietSp", gioHangChiTiet.IdSanPhamCT);
            return View(gioHangChiTiet);
        }

        // POST: GioHangChiTiets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdGioHangChiTiet,IdNguoiDung,IdSanPhamCT,Soluong,GiaGoc,TrangThai")] GioHangChiTiet gioHangChiTiet)
        {
            if (id != gioHangChiTiet.IdGioHangChiTiet)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gioHangChiTiet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GioHangChiTietExists(gioHangChiTiet.IdGioHangChiTiet))
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
            ViewData["IdNguoiDung"] = new SelectList(_context.gioHangs, "IdNguoiDung", "IdNguoiDung", gioHangChiTiet.IdNguoiDung);
            ViewData["IdSanPhamCT"] = new SelectList(_context.sanPhamChiTiets, "IdChiTietSp", "IdChiTietSp", gioHangChiTiet.IdSanPhamCT);
            return View(gioHangChiTiet);
        }

        // GET: GioHangChiTiets/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.gioHangChiTiets == null)
            {
                return NotFound();
            }

            var gioHangChiTiet = await _context.gioHangChiTiets
                .Include(g => g.GioHang)
                .Include(g => g.SanPhamChiTiet)
                .FirstOrDefaultAsync(m => m.IdGioHangChiTiet == id);
            if (gioHangChiTiet == null)
            {
                return NotFound();
            }

            return View(gioHangChiTiet);
        }

        // POST: GioHangChiTiets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.gioHangChiTiets == null)
            {
                return Problem("Entity set 'BazaizaiContext.gioHangChiTiets'  is null.");
            }
            var gioHangChiTiet = await _context.gioHangChiTiets.FindAsync(id);
            if (gioHangChiTiet != null)
            {
                _context.gioHangChiTiets.Remove(gioHangChiTiet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GioHangChiTietExists(string id)
        {
          return (_context.gioHangChiTiets?.Any(e => e.IdGioHangChiTiet == id)).GetValueOrDefault();
        }
    }
}
