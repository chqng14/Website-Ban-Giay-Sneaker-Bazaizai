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
            var model = new App_Data.ViewModels.SanPhamChiTietViewModel.DanhSachGiayViewModel();
            try
            {
                model = _sanPhamChiTietService.GetDanhSachGiayViewModelAynsc().Result;
            }
            catch (Exception)
            {
                model = new App_Data.ViewModels.SanPhamChiTietViewModel.DanhSachGiayViewModel();
            }
                
            return View(model);
        }
    }
}
