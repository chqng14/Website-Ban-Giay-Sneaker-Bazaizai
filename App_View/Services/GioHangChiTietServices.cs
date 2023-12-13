using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_View.IServices;
using System.Net.Http;

namespace App_View.Services
{
    public class GioHangChiTietServices : IGioHangChiTietServices
    {
        private readonly HttpClient _httpClient;
        public GioHangChiTietServices()
        {
            _httpClient = new HttpClient();
        }
        public async Task<bool> CreateCartDetailDTO(GioHangChiTietDTOCUD gioHangChiTietDTOCUD)
        {

            try
            {
                var res = await _httpClient.PostAsJsonAsync("https://bazaizaiapi-v2.azurewebsites.net/api/GioHangChiTiet/Create", gioHangChiTietDTOCUD);
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

        public async Task<bool> DeleteGioHang(string id)
        {
            var httpResponse = await _httpClient.DeleteAsync($"https://bazaizaiapi-v2.azurewebsites.net/api/GioHangChiTiet/Delete?id={id}");
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<List<GioHangChiTietDTO>> GetAllGioHang()
        {
            return await _httpClient.GetFromJsonAsync<List<GioHangChiTietDTO>>("https://bazaizaiapi-v2.azurewebsites.net/api/GioHangChiTiet/Get-List-GioHangChiTietDTO");

        }

        public async Task<bool> UpdateGioHang(string IdSanPhamChiTiet, int SoLuong, string IdNguoiDung)
        {
            try
            {
                var httpResponse = await _httpClient.PutAsync($"https://bazaizaiapi-v2.azurewebsites.net/api/GioHangChiTiet/Edit?IdSanPhamChiTiet={IdSanPhamChiTiet}&SoLuong={SoLuong}&IdNguoiDung={IdNguoiDung}", null);
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
