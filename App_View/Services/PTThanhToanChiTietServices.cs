using App_Data.Models;
using App_Data.Repositories;
using App_View.IServices;
using System.Net.Http;

namespace App_View.Services
{
    public class PTThanhToanChiTietServices : IPTThanhToanChiTietServices
    {
        private readonly HttpClient _httpClient;
        public PTThanhToanChiTietServices()
        {
            _httpClient = new HttpClient();
        }
        public async Task<string> CreatePTThanhToanChiTietAsync(string IdHoaDon, string IdThanhToan, double SoTien)
        {
            try
            {
                var res = await _httpClient.PostAsync($"https://localhost:7038/api/PTThanhToanChiTiet?IdHoaDon={IdHoaDon}&IdThanhToan={IdThanhToan}&SoTien={SoTien}", null);
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

        public Task<bool> DeletePTThanhToanChiTietAsync(string idPhuongThucThanhToan)
        {
            throw new NotImplementedException();
        }

        public Task<List<PhuongThucThanhToanChiTiet>> GetAllPTThanhToanChiTietAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PhuongThucThanhToanChiTiet> GetPTThanhToanChiTietByIDAsync(string idPhuongThucThanhToan)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePTThanhToanChiTietAsync(string IdPhuongThucThanhToanChiTiet, int TrangThai)
        {
            try
            {
                var res = await _httpClient.PutAsync($"https://localhost:7038/api/PTThanhToanChiTiet/SuaTrangThaiPTThanhToanChiTiet?IdPhuongThucThanhToanChiTiet={IdPhuongThucThanhToanChiTiet}&TrangThai={TrangThai}", null);
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
