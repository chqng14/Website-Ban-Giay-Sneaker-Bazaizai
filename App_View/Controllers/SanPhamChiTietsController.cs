using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Data.DbContext;
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
        private readonly ISanPhamChiTietservice _SanPhamChiTietservice;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly HttpClient _httpClient;
        public SanPhamChiTietsController(ISanPhamChiTietservice SanPhamChiTietservice, UserManager<NguoiDung> userManager, SignInManager<NguoiDung> signInManager, HttpClient httpClient)
        {
            _SanPhamChiTietservice = SanPhamChiTietservice;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpClient = httpClient;
        }


        public async Task<IActionResult> Index(string brand, string search)
        {
            return View(await _httpClient.GetFromJsonAsync<FilterDataVM>($"/api/SanPhamChiTiet/get-danh-sach-san-pham-shop-khoi-tao?brand={brand}&search={search}"));
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

            if (!string.IsNullOrEmpty(brand))
            {
                filterData.Brand = brand;
            }
            var response = await _httpClient.PostAsJsonAsync<FilterData>("/api/SanPhamChiTiet/get-danh-sach-san-pham-shop", filterData);
            var model = new FilterDataVM();
            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadAsAsync<FilterDataVM>();
            }else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"HTTP request failed with status code {response.StatusCode}. Error message: {errorMessage}");
            }
            return PartialView("_DanhSachSanPhamPartialView", model);
        }

        public async Task<IActionResult> LoadPartialViewSanPhamChiTiet(string idSanPhamChiTiet)
        {
            var model = await _SanPhamChiTietservice.GetItemDetailViewModelAynsc(idSanPhamChiTiet);
            return PartialView("_ModalSanPhamChiTietPartialView", model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var data = await _SanPhamChiTietservice.GetItemDetailViewModelAynsc(id);

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

        public async Task<IActionResult> LoadPartialDetailProduct(string id)
        {
            var data = await _SanPhamChiTietservice.GetItemDetailViewModelAynsc(id);

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var checkLogin = _userManager.GetUserId(User);

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            // Log thời gian phản hồi
            Console.WriteLine($"Thời gian phản hồi của hàm là: {elapsedTime.TotalMilliseconds} ms");

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
            return PartialView("_SanPhamDetailPatialView", data);
        }

        public async Task<IActionResult> GetItemDetailViewModelWhenSelectColor([FromQuery] string id, [FromQuery] string mauSac)
        {
            return Ok(await _SanPhamChiTietservice.GetItemDetailViewModelWhenSelectColorAynsc(id, mauSac));
        }

        public async Task<IActionResult> GetItemDetailViewModelWhenSelectSize([FromQuery] string id, [FromQuery] int size)
        {
            return Ok(await _SanPhamChiTietservice.GetItemDetailViewModelWhenSelectSizeAynsc(id, size));
        }
        public IActionResult test()
        {
            return View();
        }
    }
}
