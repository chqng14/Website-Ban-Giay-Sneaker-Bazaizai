using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDon;
using App_View.IServices;
using System.Net.Http;

namespace App_View.Services
{
    public class PTThanhToanServices : IPTThanhToanServices
    {
        private readonly HttpClient _httpClient;
        public PTThanhToanServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CreatePTThanhToanAsync(string ten, string mota, int trangthai)
        {
            try
            {
                var res = await _httpClient.PostAsync($"api/PTThanhToan?ten={ten}&mota={mota}&trangthai={trangthai}", null);
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

        public async Task<bool> DeletePTThanhToanAsync(string idPhuongThucThanhToan)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/PTThanhToan/XoaPhuongThucThanhToan={Uri.EscapeDataString(idPhuongThucThanhToan)}");
            return response.IsSuccessStatusCode && await response.Content.ReadAsAsync<bool>();
        }

        public async Task<List<PhuongThucThanhToan>> GetAllPTThanhToanAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PhuongThucThanhToan>>("api/PTThanhToan") ?? new();
        }

        public async Task<PhuongThucThanhToan> GetPTThanhToanByIDAsync(string idPhuongThucThanhToan)
        {
            return await _httpClient.GetFromJsonAsync<PhuongThucThanhToan>(
                $"api/PTThanhToan/PhuongThucThanhToan={Uri.EscapeDataString(idPhuongThucThanhToan)}");
        }

        public async Task<string> GetPTThanhToanByNameAsync(string ten)
        {
            return await _httpClient.GetStringAsync($"api/PTThanhToan/PhuongThucThanhToanByName?ten={ten}");
        }

        public async Task<bool> UpdatePTThanhToanAsync(string idPhuongThucThanhToan, string ten, string mota, int trangthai)
        {
            var id = Uri.EscapeDataString(idPhuongThucThanhToan);
            var name = Uri.EscapeDataString(ten);
            var description = Uri.EscapeDataString(mota);
            var response = await _httpClient.PutAsync(
                $"api/PTThanhToan/SuaPhuongThucThanhToan={id}?ma=&ten={name}&mota={description}&trangthai={trangthai}",
                null);
            return response.IsSuccessStatusCode && await response.Content.ReadAsAsync<bool>();
        }
    }
}
