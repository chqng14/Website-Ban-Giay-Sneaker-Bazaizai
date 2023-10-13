using App_Data.Models;
using App_Data.ViewModels.KhuyenMaiChiTietDTO;
using App_View.IServices;

namespace App_View.Services
{
    public class KhuyenMaiServices: IKhuyenMaiServices
    {
        HttpClient _httpClient;
        public KhuyenMaiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<KhuyenMai>> GetAllKhuyenMai()
        {
            var lstKM = (await _httpClient.GetFromJsonAsync<List<KhuyenMai>>("https://localhost:7038/api/GetAllKhuyenMai"));
            return lstKM;
        }
    }
}
