using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_View.IServices;

namespace App_View.Services
{
    public class GioHangChiTietServices : IGioHangChiTietServices
    {
        private readonly HttpClient _httpClient;
        public GioHangChiTietServices()
        {
            _httpClient = new HttpClient();
        }
        public Task<bool> CreateGioHang(GioHangChiTietDTO GioHangChiTietDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteGioHang(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<GioHangChiTietDTO>> GetAllGioHang()
        {
            return _httpClient.GetFromJsonAsync<List<GioHangChiTietDTO>>("https://localhost:7038/api/GioHangChiTiet/Get-List-GioHangChiTietDTO");
        }

        public Task<bool> UpdateGioHang(GioHangChiTietDTO GioHangChiTietDTO)
        {
            throw new NotImplementedException();
        }
    }
}
