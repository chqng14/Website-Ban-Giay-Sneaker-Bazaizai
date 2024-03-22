using App_Data.Models;
using App_Data.ViewModels.HoaDonChiTietDTO;
using App_View.IServices;
using Google.Apis.PeopleService.v1.Data;
using System.Net.Http;
using System.Net.Http.Json;

namespace App_View.Services
{
    public class HoaDonChiTietservices : IHoaDonChiTietservices
    {
        private readonly HttpClient httpClient;
        public HoaDonChiTietservices()
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
        public async Task<bool> UpdateTrangThaiHoaDonChiTiet(string idHoaDon, int TrangThai)

        {
            try
            {
                var res = await httpClient.PutAsync($"https://localhost:7038/api/HoaDonChiTiet/SuaTrangThaiHoaDon?idHoaDon={idHoaDon}&TrangThai={TrangThai}", null);
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

        public async Task<string> UpdateSoLuong(string idHD, string idSanPham, int SoLuongMoi, string SoluongTon)
        {
            try
            {
                var res = await httpClient.PutAsync("https://localhost:7038/api/HoaDonChiTiet/SuaSoLuong?idHD="+idHD+"&idSanPham="+idSanPham+"&SoLuongMoi="+SoLuongMoi+"&SoluongTon="+SoluongTon,null);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsStringAsync();
                }
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<string> XoaSanPhamKhoiHoaDon(string idHD, string idSanPham)
        {
            try
            {
                var res = await httpClient.DeleteAsync("https://localhost:7038/api/HoaDonChiTiet/XoaSanPhamKhoiHoaDon?idHD=" + idHD + "&idSanPham=" + idSanPham);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsStringAsync();
                }
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }

        public async Task<List<HoaDonChiTiet>> HuyHoaDon(string maHD, string lyDoHuy, string idUser)
        {
            try
            {
                var res = await httpClient.PutAsync("https://localhost:7038/api/HoaDon/HuyHoaDon?maHD=" + maHD +"&lyDoHuy="+lyDoHuy+"&idUser="+idUser, null );
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<List<HoaDonChiTiet>>();
                }
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> ThanhToanHoaDonChiTiet(string maHD)
        {
            try
            {
                var res = await httpClient.PutAsync("https://localhost:7038/api/HoaDon/ThanhToanHoaDonChiTiet?maHD=" + maHD, null);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<bool>();
                }
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
