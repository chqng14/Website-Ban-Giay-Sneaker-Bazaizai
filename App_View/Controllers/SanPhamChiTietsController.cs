using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Data.DbContextt;
using App_Data.Models;
using App_Data.IRepositories;
using App_View.IServices;
using DocumentFormat.OpenXml.Office.CustomUI;
using App_Data.ViewModels.SanPhamChiTietViewModel;

namespace App_View.Controllers
{
    public class SanPhamChiTietsController : Controller
    {
        private readonly BazaizaiContext _context;
        private readonly ISanPhamChiTietService _sanPhamChiTietService;
        public SanPhamChiTietsController(ISanPhamChiTietService sanPhamChiTietService)
        {
            _context = new BazaizaiContext();
            _sanPhamChiTietService = sanPhamChiTietService;
        }

        // GET: SanPhamChiTiets
        public async Task<IActionResult> Index()
        {
            return View(await _sanPhamChiTietService.GetListItemShopViewModelAynsc());
        }

        // GET: SanPhamChiTiets/Details/5
        public async Task<IActionResult> Details(string id)
        {
            return View(await _sanPhamChiTietService.GetItemDetailViewModelAynsc(id));
        }

        public async Task<IActionResult> GetItemDetailViewModelWhenSelectColor([FromQuery]string id,[FromQuery]string mauSac)
        {
            return Ok(await _sanPhamChiTietService.GetItemDetailViewModelWhenSelectColorAynsc(id, mauSac));
        }    

        // GET: SanPhamChiTiets/Create
        public IActionResult Create()
        {
            ViewData["IdChatLieu"] = new SelectList(_context.ChatLieus, "IdChatLieu", "IdChatLieu");
            ViewData["IdKichCo"] = new SelectList(_context.kichCos, "IdKichCo", "IdKichCo");
            ViewData["IdKieuDeGiay"] = new SelectList(_context.kieuDeGiays, "IdKieuDeGiay", "IdKieuDeGiay");
            ViewData["IdLoaiGiay"] = new SelectList(_context.LoaiGiays, "IdLoaiGiay", "IdLoaiGiay");
            ViewData["IdMauSac"] = new SelectList(_context.mauSacs, "IdMauSac", "IdMauSac");
            ViewData["IdSanPham"] = new SelectList(_context.SanPhams, "IdSanPham", "IdSanPham");
            ViewData["IdThuongHieu"] = new SelectList(_context.thuongHieus, "IdThuongHieu", "IdThuongHieu");
            ViewData["IdXuatXu"] = new SelectList(_context.xuatXus, "IdXuatXu", "IdXuatXu");
            return View();
        }

        // POST: SanPhamChiTiets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdChiTietSp,Ma,Day,MoTa,SoLuongTon,SoLuongDaBan,NgayTao,NoiBat,GiaBan,GiaNhap,TrangThai,TrangThaiSale,IdSanPham,IdKieuDeGiay,IdXuatXu,IdChatLieu,IdMauSac,IdKichCo,IdLoaiGiay,IdThuongHieu")] SanPhamChiTiet sanPhamChiTiet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanPhamChiTiet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdChatLieu"] = new SelectList(_context.ChatLieus, "IdChatLieu", "IdChatLieu", sanPhamChiTiet.IdChatLieu);
            ViewData["IdKichCo"] = new SelectList(_context.kichCos, "IdKichCo", "IdKichCo", sanPhamChiTiet.IdKichCo);
            ViewData["IdKieuDeGiay"] = new SelectList(_context.kieuDeGiays, "IdKieuDeGiay", "IdKieuDeGiay", sanPhamChiTiet.IdKieuDeGiay);
            ViewData["IdLoaiGiay"] = new SelectList(_context.LoaiGiays, "IdLoaiGiay", "IdLoaiGiay", sanPhamChiTiet.IdLoaiGiay);
            ViewData["IdMauSac"] = new SelectList(_context.mauSacs, "IdMauSac", "IdMauSac", sanPhamChiTiet.IdMauSac);
            ViewData["IdSanPham"] = new SelectList(_context.SanPhams, "IdSanPham", "IdSanPham", sanPhamChiTiet.IdSanPham);
            ViewData["IdThuongHieu"] = new SelectList(_context.thuongHieus, "IdThuongHieu", "IdThuongHieu", sanPhamChiTiet.IdThuongHieu);
            ViewData["IdXuatXu"] = new SelectList(_context.xuatXus, "IdXuatXu", "IdXuatXu", sanPhamChiTiet.IdXuatXu);
            return View(sanPhamChiTiet);
        }

        // GET: SanPhamChiTiets/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.sanPhamChiTiets == null)
            {
                return NotFound();
            }

            var sanPhamChiTiet = await _context.sanPhamChiTiets.FindAsync(id);
            if (sanPhamChiTiet == null)
            {
                return NotFound();
            }
            ViewData["IdChatLieu"] = new SelectList(_context.ChatLieus, "IdChatLieu", "IdChatLieu", sanPhamChiTiet.IdChatLieu);
            ViewData["IdKichCo"] = new SelectList(_context.kichCos, "IdKichCo", "IdKichCo", sanPhamChiTiet.IdKichCo);
            ViewData["IdKieuDeGiay"] = new SelectList(_context.kieuDeGiays, "IdKieuDeGiay", "IdKieuDeGiay", sanPhamChiTiet.IdKieuDeGiay);
            ViewData["IdLoaiGiay"] = new SelectList(_context.LoaiGiays, "IdLoaiGiay", "IdLoaiGiay", sanPhamChiTiet.IdLoaiGiay);
            ViewData["IdMauSac"] = new SelectList(_context.mauSacs, "IdMauSac", "IdMauSac", sanPhamChiTiet.IdMauSac);
            ViewData["IdSanPham"] = new SelectList(_context.SanPhams, "IdSanPham", "IdSanPham", sanPhamChiTiet.IdSanPham);
            ViewData["IdThuongHieu"] = new SelectList(_context.thuongHieus, "IdThuongHieu", "IdThuongHieu", sanPhamChiTiet.IdThuongHieu);
            ViewData["IdXuatXu"] = new SelectList(_context.xuatXus, "IdXuatXu", "IdXuatXu", sanPhamChiTiet.IdXuatXu);
            return View(sanPhamChiTiet);
        }

        // POST: SanPhamChiTiets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdChiTietSp,Ma,Day,MoTa,SoLuongTon,SoLuongDaBan,NgayTao,NoiBat,GiaBan,GiaNhap,TrangThai,TrangThaiSale,IdSanPham,IdKieuDeGiay,IdXuatXu,IdChatLieu,IdMauSac,IdKichCo,IdLoaiGiay,IdThuongHieu")] SanPhamChiTiet sanPhamChiTiet)
        {
            if (id != sanPhamChiTiet.IdChiTietSp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPhamChiTiet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamChiTietExists(sanPhamChiTiet.IdChiTietSp))
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
            ViewData["IdChatLieu"] = new SelectList(_context.ChatLieus, "IdChatLieu", "IdChatLieu", sanPhamChiTiet.IdChatLieu);
            ViewData["IdKichCo"] = new SelectList(_context.kichCos, "IdKichCo", "IdKichCo", sanPhamChiTiet.IdKichCo);
            ViewData["IdKieuDeGiay"] = new SelectList(_context.kieuDeGiays, "IdKieuDeGiay", "IdKieuDeGiay", sanPhamChiTiet.IdKieuDeGiay);
            ViewData["IdLoaiGiay"] = new SelectList(_context.LoaiGiays, "IdLoaiGiay", "IdLoaiGiay", sanPhamChiTiet.IdLoaiGiay);
            ViewData["IdMauSac"] = new SelectList(_context.mauSacs, "IdMauSac", "IdMauSac", sanPhamChiTiet.IdMauSac);
            ViewData["IdSanPham"] = new SelectList(_context.SanPhams, "IdSanPham", "IdSanPham", sanPhamChiTiet.IdSanPham);
            ViewData["IdThuongHieu"] = new SelectList(_context.thuongHieus, "IdThuongHieu", "IdThuongHieu", sanPhamChiTiet.IdThuongHieu);
            ViewData["IdXuatXu"] = new SelectList(_context.xuatXus, "IdXuatXu", "IdXuatXu", sanPhamChiTiet.IdXuatXu);
            return View(sanPhamChiTiet);
        }

        // GET: SanPhamChiTiets/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.sanPhamChiTiets == null)
            {
                return NotFound();
            }

            var sanPhamChiTiet = await _context.sanPhamChiTiets
                .Include(s => s.ChatLieu)
                .Include(s => s.KichCo)
                .Include(s => s.KieuDeGiay)
                .Include(s => s.LoaiGiay)
                .Include(s => s.MauSac)
                .Include(s => s.SanPham)
                .Include(s => s.ThuongHieu)
                .Include(s => s.XuatXu)
                .FirstOrDefaultAsync(m => m.IdChiTietSp == id);
            if (sanPhamChiTiet == null)
            {
                return NotFound();
            }

            return View(sanPhamChiTiet);
        }

        // POST: SanPhamChiTiets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.sanPhamChiTiets == null)
            {
                return Problem("Entity set 'BazaizaiContext.sanPhamChiTiets'  is null.");
            }
            var sanPhamChiTiet = await _context.sanPhamChiTiets.FindAsync(id);
            if (sanPhamChiTiet != null)
            {
                _context.sanPhamChiTiets.Remove(sanPhamChiTiet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamChiTietExists(string id)
        {
          return (_context.sanPhamChiTiets?.Any(e => e.IdChiTietSp == id)).GetValueOrDefault();
        }
    }
}
