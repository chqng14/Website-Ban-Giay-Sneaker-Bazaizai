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

        }

        public async Task<bool> DeleteGioHang(string id)
        {
            var httpResponse = await _httpClient.DeleteAsync($"https://localhost:7038/api/GioHangChiTiet/Delete?id={id}");
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<List<GioHangChiTietDTO>> GetAllGioHang()
        {
            return await _httpClient.GetFromJsonAsync<List<GioHangChiTietDTO>>("https://localhost:7038/api/GioHangChiTiet/Get-List-GioHangChiTietDTO");

        }

        public async Task<bool> UpdateGioHang(string IdGioHangChiTiet, int SoLuong)
        {
            try
            {
                var httpResponse = await _httpClient.PutAsync($"https://localhost:7038/api/GioHangChiTiet/Edit?IdGioHangChiTiet={IdGioHangChiTiet}&SoLuong={SoLuong}", null);
                if (httpResponse.IsSuccessStatusCode)
                {
                    return await httpResponse.Content.ReadAsAsync<bool>();
                }
                return false;
            }
            catch (Exception e)
            {

                return false;
            }
        }
    }
}
