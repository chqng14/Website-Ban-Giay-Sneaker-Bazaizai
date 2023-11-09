using App_Data.ViewModels.FilterViewModel;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Models.Components
{
    public class SideFilters : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public SideFilters(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IViewComponentResult Invoke()
        {
            var model = new FiltersVM();
            try
            {
                model = _httpClient.GetFromJsonAsync<FiltersVM>("/api/SanPhamChiTiet/get-ItemFilterVM").Result;
            }
            catch (Exception)
            {
                model = new FiltersVM();
            }

            return View(model);
        }
    }
}
