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
            var response = _httpClient.GetFromJsonAsync<FiltersVM>("/api/SanPhamChiTiet/get-ItemFilterVM").Result;
            if (response != null)
            {
                return View(response);
            }
            else
            {
                return View(new FiltersVM());
            }
        }
    }
}
