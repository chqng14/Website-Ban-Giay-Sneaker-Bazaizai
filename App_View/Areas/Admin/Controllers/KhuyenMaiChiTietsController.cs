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
using DocumentFormat.OpenXml.Drawing.Diagrams;
using AutoMapper;
using App_Data.ViewModels.SanPhamChiTiet.SanPhamDTO;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using static App_Data.Repositories.TrangThai;
using App_View.Models.ViewModels;
using System.Security.Policy;
using Org.BouncyCastle.Crypto;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.Metrics;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,NhanVien")]
    public class KhuyenMaiChiTietsController : Controller
    {
        private readonly BazaizaiContext _context;

        private readonly IKhuyenMaiChiTietServices khuyenMaiChiTietServices;
        private readonly IKhuyenMaiServices _khuyenMaiServices;
        private readonly ISanPhamChiTietService sanPhamChiTietService;
        private readonly IAllRepo<KhuyenMaiChiTiet> allRepo;
        private readonly IMapper _mapper;
        HttpClient httpClient;

        public KhuyenMaiChiTietsController(IKhuyenMaiChiTietServices khuyenMaiChiTietServices, ISanPhamChiTietService sanPhamChiTietService, IMapper mapper, IKhuyenMaiServices khuyenMaiServices)
        {
            _context = new BazaizaiContext();

            httpClient = new HttpClient();
            allRepo = new AllRepo<KhuyenMaiChiTiet>();
            this.khuyenMaiChiTietServices = khuyenMaiChiTietServices;
            this.sanPhamChiTietService = sanPhamChiTietService;
            _mapper = mapper;
            _khuyenMaiServices = khuyenMaiServices;
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
            await httpClient.PostAsync($"https://bazaizaiapi-v2.azurewebsites.net/api/KhuyenMaiChiTiet?mota={khuyenMaiChiTiet.MoTa}&trangThai={khuyenMaiChiTiet.TrangThai}&IDKm={khuyenMaiChiTiet.IdKhuyenMai}&IDSpCt={khuyenMaiChiTiet.IdSanPhamChiTiet}", null);
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

        public SanPhamDanhSachViewModel CreateSanPhamDanhSachViewModel(SanPhamChiTiet sanPham)
        {
            return new SanPhamDanhSachViewModel()
            {
                IdChiTietSp = sanPham.IdChiTietSp,
                ChatLieu = _context.ChatLieus.ToList().FirstOrDefault(x => x.IdChatLieu == sanPham.IdChatLieu)?.TenChatLieu,
                SanPham = _context.thuongHieus.ToList().FirstOrDefault(x => x.IdThuongHieu == sanPham.IdThuongHieu)?.TenThuongHieu + " " + _context.SanPhams.ToList().FirstOrDefault(x => x.IdSanPham == sanPham.IdSanPham)?.TenSanPham,
                GiaBan = sanPham.GiaBan,
                GiaNhap = sanPham.GiaNhap,
                KichCo = _context.kichCos.ToList().FirstOrDefault(x => x.IdKichCo == sanPham.IdKichCo)?.SoKichCo,
                Anh = _context.Anh.ToList().Where(x => x.IdSanPhamChiTiet == sanPham.IdChiTietSp && x.TrangThai == 0).FirstOrDefault()?.Url,
                KieuDeGiay = _context.kieuDeGiays.ToList().FirstOrDefault(x => x.IdKieuDeGiay == sanPham.IdKieuDeGiay)?.TenKieuDeGiay,
                LoaiGiay = _context.LoaiGiays.ToList().FirstOrDefault(x => x.IdLoaiGiay == sanPham.IdLoaiGiay)?.TenLoaiGiay,
                SoLuongTon = sanPham.SoLuongTon
            };
        }
        [HttpGet]
        public async Task<IActionResult> ApllySale(string id,string? idThuongHieu,string? idLoaiGiay,string? idMauSac)
        {
            try
            {
                var sale = _context.khuyenMais.FirstOrDefault(x => x.IdKhuyenMai == id);
               
                if (id == null)
                {
                    ViewData["IdSale"] = new SelectList(_context.khuyenMais.Where(x => x.TrangThai == (int)TrangThaiSale.DangBatDau), "IdKhuyenMai", "TenKhuyenMai");
                }
                else
                {
                    if (sale.TrangThai == (int)TrangThaiSale.HetHan)
                    {
                        TempData["ThongBao"] = "Đã hết chương trình khuyến mại không áp dụng được";
                        return RedirectToAction("Index", "KhuyenMais");
                    }
                    if (sale.TrangThai == (int)TrangThaiSale.BuocDung)
                    {
                        TempData["ThongBao"] = "Chương trình khuyến mại không hoạt động";
                        return RedirectToAction("Index", "KhuyenMais");
                    }

                    ViewData["IdSale"] = new SelectList(_context.khuyenMais.Where(x => x.IdKhuyenMai == id), "IdKhuyenMai", "TenKhuyenMai");
                }
                ViewData["IdLoaiGiay"] = new SelectList(await sanPhamChiTietService.GetListModelLoaiGiayAsync(), "IdLoaiGiay", "TenLoaiGiay");
                ViewData["IdMauSac"] = new SelectList(await sanPhamChiTietService.GetListModelMauSacAsync(), "IdMauSac", "TenMauSac");
                ViewData["IdThuongHieu"] = new SelectList(await sanPhamChiTietService.GetListModelThuongHieuAsync(), "IdThuongHieu", "TenThuongHieu");

                //.Where(x => x.TrangThaiSale == (int)TrangThaiSaleInProductDetail.DuocApDungSale|| x.TrangThaiSale == (int)TrangThaiSaleInProductDetail.DaApDungSale)
                var getallProductDT = (await sanPhamChiTietService.GetListSanPhamChiTietAsync()).Where(x => (x.TrangThaiSale == (int)TrangThaiSaleInProductDetail.DuocApDungSale || x.TrangThaiSale == (int)TrangThaiSaleInProductDetail.DaApDungSale) && x.TrangThai == (int)TrangThaiCoBan.HoatDong).Select(item => CreateSanPhamDanhSachViewModel(item));
                var model = new DanhSachGiayViewModel();
                model = sanPhamChiTietService.GetDanhSachGiayViewModelAynsc().Result;
                var lstSpDuocApDungKhuyenMai = model.LstAllSanPham.GroupBy(x => x.TenSanPham).Select(x => x.First()).ToList();



                if (idThuongHieu!=null && idThuongHieu!="")
                {
                    var tenThuongHieu = _context.thuongHieus.Find(idThuongHieu).TenThuongHieu;
                    lstSpDuocApDungKhuyenMai = (List<ItemShopViewModel>?)lstSpDuocApDungKhuyenMai.Where(x => x.ThuongHieu.ToUpper() == tenThuongHieu.ToUpper()).ToList();           
                }
                if (idLoaiGiay != null && idLoaiGiay != "")
                {
                    var tenLoaiGiay = _context.LoaiGiays.Find(idLoaiGiay).TenLoaiGiay;
                    lstSpDuocApDungKhuyenMai = (List<ItemShopViewModel>?)lstSpDuocApDungKhuyenMai.Where(x => x.TheLoai.ToUpper() == tenLoaiGiay.ToUpper()).ToList();
                }
                if (idMauSac != null && idMauSac != "")
                {
                    var tenMauSac = _context.mauSacs.Find(idMauSac).TenMauSac;
                    lstSpDuocApDungKhuyenMai = (List<ItemShopViewModel>?)lstSpDuocApDungKhuyenMai.Where(x => x.MauSac.ToUpper() == tenMauSac.ToUpper()).ToList();
                }
                return View(lstSpDuocApDungKhuyenMai);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> ApllySale(string idSale, List<string> selectedProducts)
        {
            ViewData["IdSale"] = new SelectList(_context.khuyenMais.Where(x => x.TrangThai == (int)TrangThaiSale.DangBatDau), "IdKhuyenMai", "TenKhuyenMai");
            List<string> DataMessage = new List<string>();
            var successApllySale = "";
            var saledetailVM = khuyenMaiChiTietServices.GetAllKhuyenMaiChiTiet();
            var nameSale = _context.khuyenMais.FirstOrDefault(x => x.IdKhuyenMai == idSale);
            var model = new DanhSachGiayViewModel();
            model = sanPhamChiTietService.GetDanhSachGiayViewModelAynsc().Result;
            var lstSpct = model.LstAllSanPham;
            try
            {

                int temp = 0;
                if (idSale != null && idSale != "" && selectedProducts != null && selectedProducts.Count > 0)
                {
                    foreach (var idspct in selectedProducts) 
                    {
                        var tenSp = lstSpct.FirstOrDefault(x => x.IdChiTietSp == idspct).TenSanPham;
                        var lstSp = lstSpct.Where(x => x.TenSanPham == tenSp).ToList();
                        foreach (var IdProduct in lstSp)
                        {
                            var idChiTietSanPham = _context.sanPhamChiTiets.Find(IdProduct.IdChiTietSp);
                            var saledetail = allRepo.GetAll().Where(x => x.IdSanPhamChiTiet == IdProduct.IdChiTietSp);
                            var name = _context.SanPhams.FirstOrDefault(x => x.IdSanPham == _context.sanPhamChiTiets.FirstOrDefault(x => x.IdChiTietSp == IdProduct.IdChiTietSp).IdSanPham).TenSanPham;

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

                                    DataMessage.Add($"Sản phẩm {name} đang áp dụng chương trình {nameSale.TenKhuyenMai}");
                                }
                                else
                                {
                                    var addSale = new KhuyenMaiChiTiet()
                                    {
                                        IdKhuyenMaiChiTiet = Guid.NewGuid().ToString(),
                                        IdKhuyenMai = idSale,
                                        IdSanPhamChiTiet = IdProduct.IdChiTietSp,
                                        MoTa = "Kaisan",
                                        TrangThai = (int)TrangThaiSaleDetail.DangKhuyenMai
                                    };
                                    idChiTietSanPham.TrangThaiSale = (int)TrangThaiSaleInProductDetail.DaApDungSale;
                                    _context.sanPhamChiTiets.Update(idChiTietSanPham);
                                    _context.SaveChanges();
                                    await httpClient.PostAsync($"https://bazaizaiapi-v2.azurewebsites.net/api/KhuyenMaiChiTiet?mota={addSale.MoTa}&trangThai={addSale.TrangThai}&IDKm={addSale.IdKhuyenMai}&IDSpCt={addSale.IdSanPhamChiTiet}", null);
                                    DataMessage.Add($"Áp dụng thành công chương trình giảm giá {nameSale.TenKhuyenMai} với sản phẩm {name}");
                                }
                            }
                            else
                            {
                                var addSale = new KhuyenMaiChiTiet()
                                {
                                    IdKhuyenMaiChiTiet = Guid.NewGuid().ToString(),
                                    IdKhuyenMai = idSale,
                                    IdSanPhamChiTiet = IdProduct.IdChiTietSp,
                                    MoTa = "Kaisan",
                                    TrangThai = (int)TrangThaiSaleDetail.DangKhuyenMai
                                };
                                idChiTietSanPham.TrangThaiSale = (int)TrangThaiSaleInProductDetail.DaApDungSale;
                                _context.sanPhamChiTiets.Update(idChiTietSanPham);
                                _context.SaveChanges();
                                await httpClient.PostAsync($"https://bazaizaiapi-v2.azurewebsites.net/api/KhuyenMaiChiTiet?mota={addSale.MoTa}&trangThai={addSale.TrangThai}&IDKm={addSale.IdKhuyenMai}&IDSpCt={addSale.IdSanPhamChiTiet}", null);
                                successApllySale = $"Ap dụng thành công chương trình {nameSale.TenKhuyenMai} với sản phẩm đã chọn";
                            }
                            temp++;
                        }
                        ViewBag.Sales = DataMessage;
                        
                    }
                    return Ok(new { err = DataMessage, add = successApllySale });

                }
                else
                {
                    successApllySale = "Vui lòng chọn sản phẩm để add sale";
                    return Ok(new { add = successApllySale });
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        public async Task<IActionResult> viewSaleAsync(string id)
        {
            var km = (await _khuyenMaiServices.GetAllKhuyenMai()).FirstOrDefault(x => x.IdKhuyenMai == id);
            return PartialView("viewSale", km);
        }

        [HttpGet]
        public async Task<IActionResult> viewSanPhamFilter(string id, string? idThuongHieu, string? idLoaiGiay, string? idMauSac)
        {
            var km = (await _khuyenMaiServices.GetAllKhuyenMai()).FirstOrDefault(x => x.IdKhuyenMai == id);
            var getallProductDT = (await sanPhamChiTietService.GetListSanPhamChiTietAsync()).Where(x => (x.TrangThaiSale == (int)TrangThaiSaleInProductDetail.DuocApDungSale || x.TrangThaiSale == (int)TrangThaiSaleInProductDetail.DaApDungSale) && x.TrangThai == (int)TrangThaiCoBan.HoatDong).Select(item => CreateSanPhamDanhSachViewModel(item));
            var model = new DanhSachGiayViewModel();
            model = sanPhamChiTietService.GetDanhSachGiayViewModelAynsc().Result;
            var lstSpDuocApDungKhuyenMai = model.LstAllSanPham.GroupBy(x=>x.TenSanPham).Select(x=>x.First()).ToList();
           
           
            
            if (idThuongHieu != null && idThuongHieu != "")
            {
                var tenThuongHieu = _context.thuongHieus.Find(idThuongHieu).TenThuongHieu;
                lstSpDuocApDungKhuyenMai = (List<ItemShopViewModel>?)lstSpDuocApDungKhuyenMai.Where(x => x.ThuongHieu.ToUpper() == tenThuongHieu.ToUpper()).ToList();
            }
            if (idLoaiGiay != null && idLoaiGiay != "")
            {
                var tenLoaiGiay = _context.LoaiGiays.Find(idLoaiGiay).TenLoaiGiay;
                lstSpDuocApDungKhuyenMai = (List<ItemShopViewModel>?)lstSpDuocApDungKhuyenMai.Where(x => x.TheLoai.ToUpper() == tenLoaiGiay.ToUpper()).ToList();
            }
            if (idMauSac != null && idMauSac != "")
            {
                var tenMauSac = _context.mauSacs.Find(idMauSac).TenMauSac;
                lstSpDuocApDungKhuyenMai = (List<ItemShopViewModel>?)lstSpDuocApDungKhuyenMai.Where(x => x.MauSac.ToUpper() == tenMauSac.ToUpper()).ToList();
            }
           
            if (km.LoaiHinhKM==0)
            {
                var kmDongGia = lstSpDuocApDungKhuyenMai.Where(x => x.GiaGoc > Convert.ToDouble(km.MucGiam)).ToList();
                var a = kmDongGia;
                return PartialView("viewSanPhamFilter", (List<ItemShopViewModel>?)kmDongGia);
            }
            return PartialView("viewSanPhamFilter",lstSpDuocApDungKhuyenMai);
        }
    }
}
