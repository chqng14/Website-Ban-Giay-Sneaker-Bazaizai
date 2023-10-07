using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
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
        public async Task<bool> CreateThongTin(ThongTinGiaoHang thongTinGiaoHang)
        {
            try
            {
                var res = await _httpClient.PostAsync($"https://localhost:7038/api/ThongTinGiaoHang/Create?idNguoiDung={thongTinGiaoHang.IdNguoiDung}&TenNguoiNhan={thongTinGiaoHang.TenNguoiNhan}&S%C4%90T={thongTinGiaoHang.SDT}&DiaChi={thongTinGiaoHang.DiaChi}&TrangThai={thongTinGiaoHang.TrangThai}", null);
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

        public async Task<List<ThongTinGiaoHang>> GetThongTinByIdUser(string idNguoiDung)
        {

            return await _httpClient.GetFromJsonAsync<List<ThongTinGiaoHang>>($"https://localhost:7038/api/ThongTinGiaoHang/GetByIdUser?idNguoiDung={idNguoiDung}");
        }

        public async Task<bool> UpdateThongTin(ThongTinGiaoHang thongTinGiaoHang)
        {
            try
            {
                var res = await _httpClient.PostAsync($"https://localhost:7038/api/ThongTinGiaoHang/Edit?idThongTinGH={thongTinGiaoHang.IdThongTinGH}&idNguoiDung={thongTinGiaoHang.IdNguoiDung}&TenNguoiNhan={thongTinGiaoHang.TenNguoiNhan}&S%C4%90T={thongTinGiaoHang.SDT}&DiaChi={thongTinGiaoHang.DiaChi}&TrangThai={thongTinGiaoHang.TrangThai}", null);
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
