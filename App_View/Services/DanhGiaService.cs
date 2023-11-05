//using App_Data.Models;
//using App_Data.Repositories;
//using App_Data.ViewModels.HoaDon;
//using App_View.IServices;
//using Newtonsoft.Json;
//using System.Net.Http;

//namespace App_View.Services
//{
//    public class DanhGiaService:IDanhGiaService
//    {
//        private readonly HttpClient _httpClient;
//        public DanhGiaService()
//        {
//            _httpClient = new HttpClient();
//        }
//        public async Task<string> CreateDanhGia(DanhGia DanhGia)
//        {
//            var httpClient = new HttpClient();
//            string apiUrl = $"https://localhost:7280/api/Role/Create-Role?ten={ten}&trangthai={trangthai}";
//            var response = await httpClient.PostAsync(apiUrl, null);
//            return true;


//            var response = await _httpClient.PostAsJsonAsync("https://localhost:7038/api/HoaDon/TaoHoaDonOnlineDTO", DanhGia);
//                if (res.IsSuccessStatusCode)
//                {
//                    return await res.Content.ReadAsStringAsync();
//                }
               
               
          
//        }

//        public Task<bool> DeleteHoaDon(string idHoaDon)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<List<HoaDon>> GetAllHoaDon()
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<List<HoaDonChoDTO>> GetAllHoaDonThanhCong()
//        {
//            return await _httpClient.GetFromJsonAsync<List<HoaDonChoDTO>>("https://localhost:7038/api/HoaDon/GetAllHoaDonCho");
//        }

//        public async Task<List<HoaDonViewModel>> GetHoaDon()
//        {
//            return await _httpClient.GetFromJsonAsync<List<HoaDonViewModel>>("https://localhost:7038/api/HoaDon/GetHoaDonOnline");
//        }

//        public async Task<HoaDon> TaoHoaDonTaiQuay(HoaDon hoaDon)
//        {
//            try
//            {
//                var res = await _httpClient.PostAsJsonAsync("https://localhost:7038/api/HoaDon/TaoHoaDonTaiQuay", hoaDon);
//                if (res.IsSuccessStatusCode)
//                {
//                    return await res.Content.ReadAsAsync<HoaDon>();
//                }
//                Console.WriteLine(await res.Content.ReadAsStringAsync());
//                throw new Exception("Not IsSuccessStatusCode");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                throw new Exception("Not IsSuccessStatusCode");
//            }
//        }

//        public async Task<bool> UpdateDanhGia(string iddanhGia, int TrangThai)
//        {
//            try
//            {
//                var res = await _httpClient.PutAsync($"", null);
//                if (res.IsSuccessStatusCode)
//                {
//                    return await res.Content.ReadAsAsync<bool>();
//                }
//                Console.WriteLine(await res.Content.ReadAsStringAsync());
//                throw new Exception("Not IsSuccessStatusCode");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                throw new Exception("Not IsSuccessStatusCode");
//            }
//        }
//    }
//}
