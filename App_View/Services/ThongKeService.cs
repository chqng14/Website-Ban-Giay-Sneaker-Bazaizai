using App_Data.Models;
using App_View.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App_View.Services
{
    public class ThongKeService : IThongKeService
    {
        private readonly HttpClient _httpClient;
        public ThongKeService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<double> DoanhThuTheoThang(int month)
        {
            string apiUrl = $"https://bazaizaiapi-v2.azurewebsites.net/DoanhThuTheoThang/{month}";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<double>(apiData);
            }
            catch (HttpRequestException)
            {
                return 0;
            }
        }

        public async Task<JsonResult> DoanhThuTrong7ngay()
        {
            throw new NotImplementedException();
        }

        public async Task<List<HoaDon>> DonHangTheoThang(int month, int year)
        {
            string apiUrl = $"https://bazaizaiapi-v2.azurewebsites.net/DonHangTheoThang/{month}/{year}";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<List<HoaDon>>(apiData);
            }
            catch (HttpRequestException)
            {
                return new List<HoaDon>();
            }
        }
        public async Task<int> DonHangTaiQuayTheoThang(int month)
        {
            string apiUrl = $"https://bazaizaiapi-v2.azurewebsites.net/DonHangTaiQuayTheoThang/{month}";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<int>(apiData);
            }
            catch (HttpRequestException)
            {
                return 0;
            }
        }

        public async Task<List<HoaDon>> DonDatHnagGanDay()
        {
            string apiUrl = "https://bazaizaiapi-v2.azurewebsites.net/DonDatHnagGanDay";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<List<HoaDon>>(apiData);
            }
            catch (HttpRequestException)
            {
                return new List<HoaDon>();
            }
        }

        public async Task<string> ThongKeBanHang()
        {
            string apiUrl = "https://bazaizaiapi-v2.azurewebsites.net/ThongKeBanHang";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return apiData;
            }
            catch (HttpRequestException)
            {
                // Xử lý khi gọi API thất bại
                return "Failed to call the API.";
            }
        }
        public async Task<List<HoaDon>> DonHangTaiQuayGanDay()
        {
            string apiUrl = "https://bazaizaiapi-v2.azurewebsites.net/DonHangTaiQuayGanDay";

            try
            {
                var apiData = await _httpClient.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<List<HoaDon>>(apiData);
            }
            catch (HttpRequestException)
            {
                return new List<HoaDon>();
            }
        }
    }
}
