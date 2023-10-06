using App_View.IServices;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Models.Components
{
    public class ElectronicsTab: ViewComponent
    {
        private readonly ISanPhamChiTietService _sanPhamChiTietService;

        public ElectronicsTab(ISanPhamChiTietService sanPhamChiTietService)
        {
            _sanPhamChiTietService = sanPhamChiTietService;
        }

        public IViewComponentResult Invoke()
        {
            return View(_sanPhamChiTietService.GetDanhSachGiayViewModelAynsc().Result);
        }
    }
}
