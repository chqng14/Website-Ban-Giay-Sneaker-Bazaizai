using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_View.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace App_View.Models.Components
{
    public class Cart : ViewComponent
    {
        private readonly HttpClient _httpClient;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;

        public Cart(SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager, HttpClient httpClient)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _httpClient = httpClient;
        }


        public IViewComponentResult Invoke()
        {
            var idNguoiDung = _userManager.GetUserId(HttpContext.User);
            var data = new List<SanPhamGioHangViewModel>();
            if (idNguoiDung != null)
            {
                data = _httpClient.GetFromJsonAsync<List<SanPhamGioHangViewModel>>($"/api/GioHangChiTiet/Get-List-SanPhamGioHangVM/{idNguoiDung}").Result;
            }
            else
            {
                var GioHangsession = SessionServices.GetObjFomSession(HttpContext.Session, "Cart");
                data = GioHangsession.Select(gh => new SanPhamGioHangViewModel()
                {
                    Anh = gh.LinkAnh.OrderBy(item=>item).ToList().FirstOrDefault(),
                    GiaSanPham = Convert.ToDouble(gh.GiaBan),
                    IdSanPhamChiTiet = gh.IdSanPhamCT.ToString(),
                    SoLuong = Convert.ToInt32(gh.SoLuong),
                    TenSanPham = gh.TenSanPham + " " + gh.TenMauSac + " " + gh.TenKichCo,
 
                }).ToList();
            }

            var model = new GioHangMiniViewModel()
            {
                SanPhamGioHangViewModels = data,
            };

            return View(model);
        }
    }
}
