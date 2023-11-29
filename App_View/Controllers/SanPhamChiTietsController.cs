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
using Microsoft.AspNetCore.Identity;
using App_Data.ViewModels.SanPhamYeuThichDTO;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace App_View.Controllers
{
    public class SanPhamChiTietsController : Controller
    {
        private readonly ISanPhamChiTietService _sanPhamChiTietService;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly HttpClient _httpClient;
        public SanPhamChiTietsController(ISanPhamChiTietService sanPhamChiTietService, UserManager<NguoiDung> userManager, SignInManager<NguoiDung> signInManager, HttpClient httpClient)
        {
            _sanPhamChiTietService = sanPhamChiTietService;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpClient = httpClient;
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
        public IActionResult LoadComponentDanhSachSanPhamUuDai(int page)
        {
            return ViewComponent("DanhSachSanPhamUuDai", new {page});
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

            if (filterData.LstKichCo!.Any() || filterData.LstMauSac!.Any() || (filterData.GiaMin != 0 && filterData.GiaMax != 0) || !string.IsNullOrEmpty(filterData.Sort))
            {
                data = await _sanPhamChiTietService.GetDanhSachBienTheItemShopViewModelAsync();

                if (filterData.GiaMin != 0 && filterData.GiaMax != 0)
                {
                    data = data!.Where(sp => sp.GiaThucTe >= filterData.GiaMin && sp.GiaThucTe <= filterData.GiaMax).ToList();
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

                if (filterData.LstRating!.Any())
                {
                    data = data!
                        .Where(sp =>
                            filterData.LstRating!.Any(item =>
                                sp.SoSao >= item && sp.SoSao <= item + 1
                            )
                        )
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

                if (!string.IsNullOrEmpty(filterData.Sort))
                {
                    if (filterData.Sort == "price_asc")
                    {
                        data = data!.OrderBy(it => it.GiaThucTe).ToList();
                    }else
                    {
                        data = data!.OrderByDescending(it => it.GiaThucTe).ToList();
                    }
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

            if (filterData.LstRating!.Any())
            {
                data = data!
                    .Where(sp =>
                        filterData.LstRating!.Any(item =>
                            sp.SoSao >= item && sp.SoSao <= item + 1
                        )
                    )
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
            var data = await _sanPhamChiTietService.GetItemDetailViewModelAynsc(id);
            var checkLogin = _userManager.GetUserId(User);

            if (checkLogin == null)
            {
                data!.IsYeuThich = false;
            }
            else
            {
                var danhSachYeuThich = await _httpClient.GetFromJsonAsync<List<SanPhamYeuThichViewModel>>($"/api/SanPhamYeuThich/Get-Danh-Sach-SanPhamYeuThich?idNguoiDung={checkLogin}");
                if (danhSachYeuThich!.FirstOrDefault(x => x.IdSanPhamChiTiet == data!.IdChiTietSp) != null)
                {
                    data!.IsYeuThich = true;
                }
                else
                {
                    data!.IsYeuThich = false;
                }
            }

            return View(data);
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
