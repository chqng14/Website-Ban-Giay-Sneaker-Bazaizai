using App_Data.Models;
using App_Data.Repositories;
using App_View.IServices;
using System.Net.Http;

namespace App_View.Services
{
    public class PTThanhToanChiTietServices : IPTThanhToanChiTietServices
    {
        private readonly HttpClient _httpClient;
        public PTThanhToanChiTietServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> CreatePTThanhToanChiTietAsync(string IdHoaDon, string IdThanhToan, double SoTien)
        {
            try
            {
                var res = await _httpClient.PostAsync($"api/PTThanhToanChiTiet?IdHoaDon={IdHoaDon}&IdThanhToan={IdThanhToan}&SoTien={SoTien}", null);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsStringAsync();
                }
                Console.WriteLine(await res.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<bool> DeletePTThanhToanChiTietAsync(string idPhuongThucThanhToan)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/PTThanhToanChiTiet/XoaPhuongThucThanhToanChiTiet={Uri.EscapeDataString(idPhuongThucThanhToan)}");
            return response.IsSuccessStatusCode && await response.Content.ReadAsAsync<bool>();
        }

        public async Task<List<PhuongThucThanhToanChiTiet>> GetAllPTThanhToanChiTietAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PhuongThucThanhToanChiTiet>>(
                "api/PTThanhToanChiTiet") ?? new();
        }

        public async Task<PhuongThucThanhToanChiTiet> GetPTThanhToanChiTietByIDAsync(string idPhuongThucThanhToan)
        {
            return await _httpClient.GetFromJsonAsync<PhuongThucThanhToanChiTiet>(
                $"api/PTThanhToanChiTiet/TimPhuongThucThanhToanChiTiet={Uri.EscapeDataString(idPhuongThucThanhToan)}");
        }

        public async Task<bool> UpdatePTThanhToanChiTietAsync(string IdPhuongThucThanhToanChiTiet, int TrangThai)
        {
            try
            {
                var res = await _httpClient.PutAsync($"api/PTThanhToanChiTiet/SuaTrangThaiPTThanhToanChiTiet?IdPhuongThucThanhToanChiTiet={IdPhuongThucThanhToanChiTiet}&TrangThai={TrangThai}", null);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<bool>();
                }
                Console.WriteLine(await res.Content.ReadAsStringAsync());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }
    }
}
