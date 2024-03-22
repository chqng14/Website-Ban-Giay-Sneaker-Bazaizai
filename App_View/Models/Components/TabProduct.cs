using App_View.IServices;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Models.Components
{
    public class TabProduct: ViewComponent
    {
        private readonly ISanPhamChiTietservice _SanPhamChiTietservice;

        public TabProduct(ISanPhamChiTietservice SanPhamChiTietservice)
        {
            _SanPhamChiTietservice = SanPhamChiTietservice;
        }

        public IViewComponentResult Invoke()
        {
            var model = new App_Data.ViewModels.SanPhamChiTietViewModel.DanhSachGiayViewModel();
            try
            {
                model = _SanPhamChiTietservice.GetDanhSachGiayViewModelAynsc().Result;
            }
            catch (Exception)
            {
                model = new App_Data.ViewModels.SanPhamChiTietViewModel.DanhSachGiayViewModel();
            }
                
            return View(model);
        }
    }
}
