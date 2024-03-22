using App_Data.Models;
using App_Data.ViewModels.KhuyenMaiChiTietDTO;
using App_View.IServices;
using System.Net.Http;

namespace App_View.Services
{
    public class KhuyenMaiChiTietservices : IKhuyenMaiChiTietservices
    {
        HttpClient _httpClient;
        public KhuyenMaiChiTietservices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<KhuyenMaiChiTietDTO>> GetAllKhuyenMaiChiTiet()
        {
            var lstKMCT = (await _httpClient.GetFromJsonAsync<List<KhuyenMaiChiTietDTO>>("https://localhost:7038/api/KhuyenMaiChiTiet"));
            return lstKMCT;
        }
    }
}
