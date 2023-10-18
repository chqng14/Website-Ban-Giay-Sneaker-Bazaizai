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
using App_Data.ViewModels.FilterViewModel;

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


        public async Task<IActionResult> Index(string brand, string search)
        {
            var lstSanPhamItemShop = await _sanPhamChiTietService.GetListItemShopViewModelAynsc();
            if (!string.IsNullOrEmpty(brand))
            {
                lstSanPhamItemShop = lstSanPhamItemShop!.Where(sp => sp.ThuongHieu!.ToLower() == brand.ToLower()).ToList();
            }
            if (!string.IsNullOrEmpty(search))
            {
                lstSanPhamItemShop = lstSanPhamItemShop!.Where(sp => sp.TenSanPham!.ToLower().Contains(search.ToLower())).ToList();
            }

            return View(new FilterDataVM()
            {
                Items = lstSanPhamItemShop!.Take(12).ToList(),
                PagingInfo = new PagingInfo()
                {
                    SoItemTrenMotTrang = 12,
                    TongSoItem = lstSanPhamItemShop!.Count(),
                    TrangHienTai = 1
                }

            });
        }

        [HttpPost]
        public async Task<IActionResult> LoadPartialViewDanhSachSanPhamNguoiDung([FromBody]FilterData filterData)
        {
            return PartialView("_DanhSachSanPhamPartialView", new FilterDataVM());
        }

        public async Task<IActionResult> LoadPartialViewSanPhamChiTiet(string idSanPhamChiTiet)
        {
            var model = await _sanPhamChiTietService.GetItemDetailViewModelAynsc(idSanPhamChiTiet);
            return PartialView("_ModalSanPhamChiTietPartialView", model);
        }

        public async Task<IActionResult> Details(string id)
        {
            return View(await _sanPhamChiTietService.GetItemDetailViewModelAynsc(id));
        }

        public async Task<IActionResult> GetItemDetailViewModelWhenSelectColor([FromQuery] string id, [FromQuery] string mauSac)
        {
            return Ok(await _sanPhamChiTietService.GetItemDetailViewModelWhenSelectColorAynsc(id, mauSac));
        }

        public async Task<IActionResult> GetItemDetailViewModelWhenSelectSize([FromQuery] string id, [FromQuery] int size)
        {
            return Ok(await _sanPhamChiTietService.GetItemDetailViewModelWhenSelectSizeAynsc(id, size));
        }

    }
}
