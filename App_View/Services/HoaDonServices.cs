using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDon;
using App_View.IServices;
using System.Net.Http;
using System.Net.Http.Json;

namespace App_View.Services
{
    public class HoaDonServices : IHoaDonServices
    {
        private readonly HttpClient _httpClient;
        public HoaDonServices()
        {
            _httpClient = new HttpClient();
        }
        public async Task<bool> CreateHoaDon(HoaDonDTO hoaDonDTO)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync("https://localhost:7038/api/HoaDon/TaoHoaDonOnlineDTO", hoaDonDTO);
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

        public Task<bool> DeleteHoaDon(string idHoaDon)
        {
            throw new NotImplementedException();
        }

        public Task<List<HoaDon>> GetAllHoaDon()
        {
            throw new NotImplementedException();
        }

        public async Task<List<HoaDonChoDTO>> GetAllHoaDonCho()
        {
            return await _httpClient.GetFromJsonAsync<List<HoaDonChoDTO>>("https://localhost:7038/api/HoaDon/GetAllHoaDonCho");
        }

        public async Task<HoaDon> TaoHoaDonTaiQuay(HoaDon hoaDon)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync("https://localhost:7038/api/HoaDon/TaoHoaDonTaiQuay", hoaDon);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<HoaDon>();
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

        public Task<bool> UpdateHoaDon(HoaDon HoaDon)
        {
            throw new NotImplementedException();
        }
    }
}
