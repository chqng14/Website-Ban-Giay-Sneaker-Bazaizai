using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.ThongTinGHDTO;
using App_View.IServices;

namespace App_View.Services
{
    public class ThongTinGHServices : IThongTinGHServices
    {
        private readonly HttpClient _httpClient;
        public ThongTinGHServices()
        {
            _httpClient = new HttpClient();
        }
        public async Task<bool> CreateThongTin(ThongTinGHDTO thongTinGHDTO)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync("https://localhost:7038/api/ThongTinGiaoHang/Create", thongTinGHDTO);
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

        public async Task<bool> DeleteThongTin(string id)
        {
            var res = await _httpClient.DeleteAsync($"https://localhost:7038/api/ThongTinGiaoHang/Delete?id={id}");
            return res.IsSuccessStatusCode;
        }

        public async Task<List<ThongTinGiaoHang>> GetAllThongTin()
        {
            return await _httpClient.GetFromJsonAsync<List<ThongTinGiaoHang>>("https://localhost:7038/api/ThongTinGiaoHang/GetAll");
        }

        public async Task<List<ThongTinGHDTO>> GetAllThongTinDTO()
        {
            return await _httpClient.GetFromJsonAsync<List<ThongTinGHDTO>>("https://localhost:7038/api/ThongTinGiaoHang/GetAllDTO");
        }

        public async Task<List<ThongTinGiaoHang>> GetThongTinByIdUser(string idNguoiDung)
        {

            return await _httpClient.GetFromJsonAsync<List<ThongTinGiaoHang>>($"https://localhost:7038/api/ThongTinGiaoHang/GetByIdUser?idNguoiDung={idNguoiDung}");
        }

        public async Task<bool> UpdateThongTin(ThongTinGHDTO thongTinGHDTO)
        {
            try
            {
                var res = await _httpClient.PutAsJsonAsync("https://localhost:7038/api/ThongTinGiaoHang/Edit", thongTinGHDTO);
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

        public async Task<bool> UpdateTrangThaiThongTin(string idThongTin)
        {
            try
            {
                var res = await _httpClient.PutAsync($"https://localhost:7038/api/ThongTinGiaoHang/UpdateTrangThai?idThongTin={idThongTin}", null);
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
