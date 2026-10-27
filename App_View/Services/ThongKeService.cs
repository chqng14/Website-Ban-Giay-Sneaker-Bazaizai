using App_Data.Models;
using App_View.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App_View.Services
{
    public class ThongKeService : IThongKeService
    {
        private readonly HttpClient _httpClient;
        public ThongKeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<double> DoanhThuTheoThang(int month)
        {
            string apiUrl = $"DoanhThuTheoThang/{month}";

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
            var data = await _httpClient.GetFromJsonAsync<System.Text.Json.JsonElement>(
                "DoanhThuTrong7ngay");
            return new JsonResult(data);
        }

        public async Task<List<HoaDon>> DonHangTheoThang(int month, int year)
        {
            string apiUrl = $"DonHangTheoThang/{month}/{year}";

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
            string apiUrl = $"DonHangTaiQuayTheoThang/{month}";

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
            string apiUrl = "DonDatHnagGanDay";

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
            string apiUrl = "ThongKeBanHang";

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
            string apiUrl = "DonHangTaiQuayGanDay";

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
