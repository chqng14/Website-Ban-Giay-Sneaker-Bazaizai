using App_Data.ViewModels.FilterViewModel;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Models.Components
{
    public class DanhSachSanPhamUuDai : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public DanhSachSanPhamUuDai(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IViewComponentResult Invoke(int page = 1)
        {
            var lstItemShop = _httpClient.GetFromJsonAsync<List<ItemShopViewModel>>("/api/SanPhamChiTiet/Get-List-ItemBienTheShopViewModelSale").Result;
            var data = new FilterDataVM()
            {
                Items = lstItemShop!.Skip((page-1)*9).Take(9).ToList(),
                PagingInfo = new PagingInfo()
                {
                    SoItemTrenMotTrang = 9,
                    TongSoItem = lstItemShop!.Count,
                    TrangHienTai = page
                }
            };
            return View(data);
        }
    }
}
