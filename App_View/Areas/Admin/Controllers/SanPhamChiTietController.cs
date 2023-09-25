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
using App_Data.ViewModels.SanPhamChiTiet;
using App_Data.ViewModels.Anh;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamChiTietController : Controller
    {
        private readonly BazaizaiContext _context;
        private readonly ISanPhamChiTietService _sanPhamChiTietService;

        public SanPhamChiTietController(ISanPhamChiTietService sanPhamChiTietService)
        {
            _context = new BazaizaiContext();
            _sanPhamChiTietService = sanPhamChiTietService;
        }
        [HttpGet]
        // GET: Admin/SanPhamChiTiet/DanhSachSanPham
        public async Task<IActionResult> DanhSachSanPham(int draw, int start, int length, string searchValue)
        {
            return View(await _sanPhamChiTietService.GetListSanPhamChiTietViewModelAsync());
        }

        public async Task<IActionResult> GetDanhSachSanPham(int draw, int start, int length, string searchValue)
        {
            var query = (await _sanPhamChiTietService.GetListSanPhamChiTietViewModelAsync())
                .Skip(start)
                .Take(length)
                .ToList();

            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchValueLower = searchValue.ToLower();
                query = (await _sanPhamChiTietService.GetListSanPhamChiTietViewModelAsync()).Where(x => x.SanPham!.ToLower().Contains(searchValueLower) || x.LoaiGiay!.ToLower().Contains(searchValueLower) || x.ChatLieu!.ToLower().Contains(searchValueLower) || x.MauSac!.ToLower().Contains(searchValueLower))
                .Skip(start)
                .Take(length)
                .ToList();

            }

            var totalRecords = (await _sanPhamChiTietService.GetListSanPhamChiTietViewModelAsync()).Count;

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = query
            });
        }


        // GET: Admin/SanPhamChiTiet/Details/5
        public async Task<IActionResult> Details(string id)
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

        // GET: Admin/SanPhamChiTiet/ManageSanPham
        public IActionResult ManageSanPham()
        {
            ViewData["IdChatLieu"] = new SelectList(_context.ChatLieus, "IdChatLieu", "TenChatLieu");
            ViewData["IdKichCo"] = new SelectList(_context.kichCos, "IdKichCo", "SoKichCo");
            ViewData["IdKieuDeGiay"] = new SelectList(_context.kieuDeGiays, "IdKieuDeGiay", "TenKieuDeGiay");
            ViewData["IdLoaiGiay"] = new SelectList(_context.LoaiGiays, "IdLoaiGiay", "TenLoaiGiay");
            ViewData["IdMauSac"] = new SelectList(_context.mauSacs, "IdMauSac", "TenMauSac");
            ViewData["IdSanPham"] = new SelectList(_context.SanPhams, "IdSanPham", "TenSanPham");
            ViewData["IdThuongHieu"] = new SelectList(_context.thuongHieus, "IdThuongHieu", "TenThuongHieu");
            ViewData["IdXuatXu"] = new SelectList(_context.xuatXus, "IdXuatXu", "Ten");
            return View();
        }

        public async Task<IActionResult> LoadPartialView(string idSanPhamChiTiet)
        {
            var model = (await _sanPhamChiTietService.GetListSanPhamChiTietViewModelAsync()).FirstOrDefault(x => x.IdChiTietSp == idSanPhamChiTiet);
            return PartialView("_DetailPartialView", model);
        }

        [HttpPost]
        public async Task<IActionResult> CheckSanPhamAddOrUpdate([FromBody]SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            return Json(await _sanPhamChiTietService.CheckSanPhamAddOrUpdate(sanPhamChiTietDTO));
        }

        // POST: Admin/SanPhamChiTiet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            if (ModelState.IsValid)
            {
                return Json(await _sanPhamChiTietService.AddAysnc(sanPhamChiTietDTO));
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task CreateAnh([FromForm]string IdChiTietSp, [FromForm] List<IFormFile> lstIFormFile)
        {
            await _sanPhamChiTietService.CreateAnhAysnc(IdChiTietSp, lstIFormFile);
        }

        [HttpPost]
        public async Task<bool> UpdateSanPham([FromBody]SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            return await _sanPhamChiTietService.UpdateAynsc(sanPhamChiTietDTO);
        }

        [HttpPost]
        public async Task DeleteAnh([FromForm]ImageDTO responseImageDeleteVM)
        {
             await _sanPhamChiTietService.DeleteAnhAysnc(responseImageDeleteVM);
        }

        // GET: Admin/SanPhamChiTiet/Edit/5
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

        // POST: Admin/SanPhamChiTiet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdChiTietSp,Ma,Day,MoTa,SoLuongTon,GiaBan,GiaNhap,TrangThai,TrangThaiSale,IdSanPham,IdKieuDeGiay,IdXuatXu,IdChatLieu,IdMauSac,IdKichCo,IdLoaiGiay,IdThuongHieu")] SanPhamChiTiet sanPhamChiTiet)
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

        // GET: Admin/SanPhamChiTiet/Delete/5
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

        // POST: Admin/SanPhamChiTiet/Delete/5
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
