using App_Data.Models;
using App_Data.ViewModels.KhuyenMaiChiTietDTO;
using App_View.IServices;
using System.Net.Http;

namespace App_View.Services
{
    public class KhuyenMaiChiTietServices : IKhuyenMaiChiTietServices
    {
        HttpClient _httpClient;
        public KhuyenMaiChiTietServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<KhuyenMaiChiTietDTO>> GetAllKhuyenMaiChiTiet()
        {
            var lstKMCT = (await _httpClient.GetFromJsonAsync<List<KhuyenMaiChiTietDTO>>("https://bazaizaiapi.azurewebsites.net/api/KhuyenMaiChiTiet"));
            return lstKMCT;
        }
    }
}
