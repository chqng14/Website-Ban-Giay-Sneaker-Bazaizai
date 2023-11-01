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
        public PTThanhToanServices()
        {
            _httpClient = new HttpClient();
        }
        public async Task<bool> CreatePTThanhToanAsync(string ten, string mota, int trangthai)
        {
            try
            {
                var res = await _httpClient.PostAsync($"https://localhost:7038/api/PTThanhToan?ten={ten}&mota={mota}&trangthai={trangthai}", null);
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

        public Task<bool> DeletePTThanhToanAsync(string idPhuongThucThanhToan)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PhuongThucThanhToan>> GetAllPTThanhToanAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PhuongThucThanhToan>>("https://localhost:7038/api/PTThanhToan");
        }

        public Task<PhuongThucThanhToan> GetPTThanhToanByIDAsync(string idPhuongThucThanhToan)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetPTThanhToanByNameAsync(string ten)
        {
            return await _httpClient.GetStringAsync($"https://localhost:7038/api/PTThanhToan/PhuongThucThanhToanByName?ten={ten}");
        }

        public Task<bool> UpdatePTThanhToanAsync(string idPhuongThucThanhToan, string ten, string mota, int trangthai)
        {
            throw new NotImplementedException();
        }
    }
}
