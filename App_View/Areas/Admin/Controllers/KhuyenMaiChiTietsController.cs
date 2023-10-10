using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Data.DbContextt;
using App_Data.Models;
using Newtonsoft.Json;
using System.Net.Http;
using App_View.IServices;
using App_View.Services;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.InkML;
using App_Data.IRepositories;
using App_Data.Repositories;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhuyenMaiChiTietsController : Controller
    {
        private readonly BazaizaiContext _context;
        private readonly IKhuyenMaiChiTietServices khuyenMaiChiTietServices;
        private readonly ISanPhamChiTietService sanPhamChiTietService;
        private readonly IAllRepo<KhuyenMaiChiTiet> allRepo;
        HttpClient httpClient;

        public KhuyenMaiChiTietsController(IKhuyenMaiChiTietServices khuyenMaiChiTietServices, ISanPhamChiTietService sanPhamChiTietService)
        {
            _context = new BazaizaiContext();

            httpClient = new HttpClient();
            allRepo = new AllRepo<KhuyenMaiChiTiet>();
            this.khuyenMaiChiTietServices = khuyenMaiChiTietServices;
            this.sanPhamChiTietService = sanPhamChiTietService;
        }

        // GET: Admin/KhuyenMaiChiTiets
        public async Task<IActionResult> Index()
        {
            
            return View(await khuyenMaiChiTietServices.GetAllKhuyenMaiChiTiet());
        }

        // GET: Admin/KhuyenMaiChiTiets/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.khuyenMaiChiTiets == null)
            {
                return NotFound();
            }

            var khuyenMaiChiTiet = await _context.khuyenMaiChiTiets
                .Include(k => k.KhuyenMai)
                .Include(k => k.SanPhamChiTiet)
                .FirstOrDefaultAsync(m => m.IdKhuyenMaiChiTiet == id);
            if (khuyenMaiChiTiet == null)
            {
                return NotFound();
            }

            return View(khuyenMaiChiTiet);
        }

        // GET: Admin/KhuyenMaiChiTiets/Create
        public IActionResult Create()
        {
            ViewData["IdKhuyenMai"] = new SelectList(_context.khuyenMais, "IdKhuyenMai", "IdKhuyenMai");
            ViewData["IdSanPhamChiTiet"] = new SelectList(_context.sanPhamChiTiets, "IdChiTietSp", "IdChiTietSp");
            return View();
        }

        // POST: Admin/KhuyenMaiChiTiets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKhuyenMaiChiTiet,IdKhuyenMai,IdSanPhamChiTiet,MoTa,TrangThai")] KhuyenMaiChiTiet khuyenMaiChiTiet)
        {
            khuyenMaiChiTiet.IdKhuyenMaiChiTiet = Guid.NewGuid().ToString();
            khuyenMaiChiTiet.TrangThai = 1;
            await httpClient.PostAsync($"https://localhost:7038/api/KhuyenMaiChiTiet?mota={khuyenMaiChiTiet.MoTa}&trangThai={khuyenMaiChiTiet.TrangThai}&IDKm={khuyenMaiChiTiet.IdKhuyenMai}&IDSpCt={khuyenMaiChiTiet.IdSanPhamChiTiet}", null);
            ViewData["IdKhuyenMai"] = new SelectList(_context.khuyenMais, "IdKhuyenMai", "IdKhuyenMai", khuyenMaiChiTiet.IdKhuyenMai);
            ViewData["IdSanPhamChiTiet"] = new SelectList(_context.sanPhamChiTiets, "IdChiTietSp", "IdChiTietSp", khuyenMaiChiTiet.IdSanPhamChiTiet);
            return RedirectToAction("Index");
        }

        // GET: Admin/KhuyenMaiChiTiets/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.khuyenMaiChiTiets == null)
            {
                return NotFound();
            }

            var khuyenMaiChiTiet = await _context.khuyenMaiChiTiets.FindAsync(id);
            if (khuyenMaiChiTiet == null)
            {
                return NotFound();
            }
            ViewData["IdKhuyenMai"] = new SelectList(_context.khuyenMais, "IdKhuyenMai", "IdKhuyenMai", khuyenMaiChiTiet.IdKhuyenMai);
            ViewData["IdSanPhamChiTiet"] = new SelectList(_context.sanPhamChiTiets, "IdChiTietSp", "IdChiTietSp", khuyenMaiChiTiet.IdSanPhamChiTiet);
            return View(khuyenMaiChiTiet);
        }

        // POST: Admin/KhuyenMaiChiTiets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdKhuyenMaiChiTiet,IdKhuyenMai,IdSanPhamChiTiet,MoTa,TrangThai")] KhuyenMaiChiTiet khuyenMaiChiTiet)
        {
            if (id != khuyenMaiChiTiet.IdKhuyenMaiChiTiet)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khuyenMaiChiTiet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhuyenMaiChiTietExists(khuyenMaiChiTiet.IdKhuyenMaiChiTiet))
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
            ViewData["IdKhuyenMai"] = new SelectList(_context.khuyenMais, "IdKhuyenMai", "IdKhuyenMai", khuyenMaiChiTiet.IdKhuyenMai);
            ViewData["IdSanPhamChiTiet"] = new SelectList(_context.sanPhamChiTiets, "IdChiTietSp", "IdChiTietSp", khuyenMaiChiTiet.IdSanPhamChiTiet);
            return View(khuyenMaiChiTiet);
        }

        // GET: Admin/KhuyenMaiChiTiets/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.khuyenMaiChiTiets == null)
            {
                return NotFound();
            }

            var khuyenMaiChiTiet = await _context.khuyenMaiChiTiets
                .Include(k => k.KhuyenMai)
                .Include(k => k.SanPhamChiTiet)
                .FirstOrDefaultAsync(m => m.IdKhuyenMaiChiTiet == id);
            if (khuyenMaiChiTiet == null)
            {
                return NotFound();
            }

            return View(khuyenMaiChiTiet);
        }

        // POST: Admin/KhuyenMaiChiTiets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.khuyenMaiChiTiets == null)
            {
                return Problem("Entity set 'BazaizaiContext.khuyenMaiChiTiets'  is null.");
            }
            var khuyenMaiChiTiet = await _context.khuyenMaiChiTiets.FindAsync(id);
            if (khuyenMaiChiTiet != null)
            {
                _context.khuyenMaiChiTiets.Remove(khuyenMaiChiTiet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhuyenMaiChiTietExists(string id)
        {
          return (_context.khuyenMaiChiTiets?.Any(e => e.IdKhuyenMaiChiTiet == id)).GetValueOrDefault();
        }
        [HttpGet]
        public async Task<IActionResult> ApllySale()
        {
            var saledetail = await sanPhamChiTietService.GetListSanPhamChiTietViewModelAsync();
            ViewData["IdSale"] = new SelectList(_context.khuyenMais, "IdKhuyenMai", "TenKhuyenMai");

            return View(saledetail);
        }
        [HttpPost]
        public async Task<IActionResult> ApllySale(string idSale, List<string> selectedProducts)
        {
            ViewData["IdSale"] = new SelectList(_context.khuyenMais, "IdKhuyenMai", "TenKhuyenMai");
            List<string> DataMessage = new List<string>();
            var successApllySale = "";
            var saledetailVM = khuyenMaiChiTietServices.GetAllKhuyenMaiChiTiet();
            try
            {
                int temp = 0;
                foreach (var IdProduct in selectedProducts)
                {
                    var saledetail = allRepo.GetAll().Where(x => x.IdSanPhamChiTiet == IdProduct);
                    var name = _context.SanPhams.FirstOrDefault(x => x.IdSanPham == _context.sanPhamChiTiets.FirstOrDefault(x => x.IdChiTietSp == IdProduct).IdSanPham).TenSanPham;
                    var nameSale = _context.khuyenMais.FirstOrDefault(x => x.IdKhuyenMai == idSale).TenKhuyenMai;
                    if (saledetail != null && saledetail.Count() > 0)
                    {
                        int i = 0;

                        foreach (var checkSale in saledetail)
                        {
                            if (checkSale.IdKhuyenMai == idSale)
                            {
                                i++;
                                break;
                            }
                        }
                        if (i != 0)
                        {

                            DataMessage.Add($"Sản phẩm {name} đang áp dụng chương trình {nameSale}");
                        }
                        else
                        {
                            var addSale = new KhuyenMaiChiTiet()
                            {
                                IdKhuyenMaiChiTiet = Guid.NewGuid().ToString(),
                                IdKhuyenMai = idSale,
                                IdSanPhamChiTiet = IdProduct,
                                MoTa = "Kaisan",
                                TrangThai = 0
                            };
                            await httpClient.PostAsync($"https://localhost:7038/api/KhuyenMaiChiTiet?mota={addSale.MoTa}&trangThai={addSale.TrangThai}&IDKm={addSale.IdKhuyenMai}&IDSpCt={addSale.IdSanPhamChiTiet}", null);
                            DataMessage.Add($"Áp dụng thành công chương trình giảm giá {nameSale} với sản phẩm {name}");
                        }
                    }
                    else
                    {
                        var addSale = new KhuyenMaiChiTiet()
                        {
                            IdKhuyenMaiChiTiet = Guid.NewGuid().ToString(),
                            IdKhuyenMai = idSale,
                            IdSanPhamChiTiet = IdProduct,
                            MoTa = "Kaisan",
                            TrangThai = 0
                        };
                        await httpClient.PostAsync($"https://localhost:7038/api/KhuyenMaiChiTiet?mota={addSale.MoTa}&trangThai={addSale.TrangThai}&IDKm={addSale.IdKhuyenMai}&IDSpCt={addSale.IdSanPhamChiTiet}", null);
                        successApllySale = $"Ap dụng thành công chương trình {nameSale} với sản phẩm đã chọn";
                    }
                    temp++;
                }
                ViewBag.Sales = DataMessage;
                return Ok(new { err = DataMessage, add = successApllySale });
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
