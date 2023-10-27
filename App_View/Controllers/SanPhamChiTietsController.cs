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
                lstSanPhamItemShop = lstSanPhamItemShop!
                    .Where(sp => sp.ThuongHieu!.ToLower() == brand.ToLower())
                    .ToList();
            }

            if (!string.IsNullOrEmpty(search))
            {
                lstSanPhamItemShop = lstSanPhamItemShop!
                    .Where(sp => sp.TenSanPham!.ToLower()
                    .Contains(search.ToLower()))
                    .ToList();
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
        public async Task<IActionResult> LoadPartialViewDanhSachSanPhamNguoiDung([FromBody] FilterData filterData)
        {
            var brand = HttpContext.Request.Query["brand"].ToString();
            var data = await _sanPhamChiTietService.GetListItemShopViewModelAynsc();

            if (!string.IsNullOrEmpty(brand))
            {
                data = data!.Where(sp => sp.ThuongHieu!.ToLower() == brand.ToLower()).ToList();
            }

            if (filterData.LstKichCo!.Any() || filterData.LstMauSac!.Any() || (filterData.GiaMin != 0 && filterData.GiaMax != 0))
            {
                data = await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync();

                if (filterData.GiaMin != 0 && filterData.GiaMax != 0)
                {
                    data = data!.Where(sp => sp.GiaBan >= filterData.GiaMin && sp.GiaBan <= filterData.GiaMax).ToList();
                }

                if (!string.IsNullOrEmpty(brand))
                {
                    data = data!.Where(sp => sp.ThuongHieu!.ToLower() == brand.ToLower()).ToList();
                }

                if (filterData.LstMauSac!.Any())
                {
                    data = data!
                        .Where(sp => filterData.LstMauSac!.Contains(sp.MauSac!))
                        .ToList();
                }

                if (filterData.LstKichCo!.Any())
                {
                    data = data!
                        .Where(sp => filterData.LstKichCo!.Contains(sp.KichCo!))
                        .ToList();
                }

                if (filterData.LstTheLoai!.Any())
                {
                    data = data!
                        .Where(sp => filterData.LstTheLoai!.Contains(sp.TheLoai!))
                        .ToList();
                }

                var dataBienThe = new FilterDataVM()
                {
                    Items = data!.Skip((filterData.TrangHienTai - 1) * 12).Take(12).ToList(),
                    PagingInfo = new PagingInfo()
                    {
                        SoItemTrenMotTrang = 12,
                        TongSoItem = data!.Count(),
                        TrangHienTai = filterData.TrangHienTai
                    }
                };

                return PartialView("_DanhSachSanPhamBienThePartialView", dataBienThe);
            }

            if (filterData.LstTheLoai!.Any())
            {
                data = data!
                    .Where(sp => filterData.LstTheLoai!.Contains(sp.TheLoai!))
                    .ToList();
            }

            var model = new FilterDataVM()
            {
                Items = data!.Skip((filterData.TrangHienTai - 1) * 12).Take(12).ToList(),
                PagingInfo = new PagingInfo()
                {
                    SoItemTrenMotTrang = 12,
                    TongSoItem = data!.Count(),
                    TrangHienTai = filterData.TrangHienTai
                }
            };

            return PartialView("_DanhSachSanPhamPartialView", model);
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
