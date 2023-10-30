using App_Data.Models;
using App_Data.ViewModels.HoaDonChiTietDTO;
using App_View.IServices;
using System.Net.Http;

namespace App_View.Services
{
    public class HoaDonChiTietServices : IHoaDonChiTietServices
    {
        private readonly HttpClient httpClient;
        public HoaDonChiTietServices()
        {
            httpClient = new HttpClient();
        }
        public async Task<bool> CreateHoaDonChiTiet(HoaDonChiTietDTO hoaDonChiTietDTO)
        {
            try
            {
                var res = await httpClient.PostAsJsonAsync("https://localhost:7038/api/HoaDonChiTiet/Create", hoaDonChiTietDTO);
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

        public Task<bool> DeleteHoaDonChiTiet(string idHoaDonChiTiet)
        {
            throw new NotImplementedException();
        }

        public Task<List<HoaDonChiTietViewModel>> GetAllHoaDonChiTiet()
        {
            throw new NotImplementedException();
        }

        public async Task<HoaDonChiTiet> ThemSanPhamVaoHoaDon(HoaDonChiTiet hoaDonChiTiet)
        {
            try
            {
                var res = await httpClient.PostAsJsonAsync("https://localhost:7038/api/HoaDonChiTiet/ThemSanPhamVaoHoaDon", hoaDonChiTiet);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<HoaDonChiTiet>();
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

        public Task<bool> UpdateHoaDonChiTiet(HoaDonChiTietDTO hoaDonChiTietDTO)
        {
            throw new NotImplementedException();
        }
    }
}
