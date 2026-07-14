using App_Data.Models;
using App_Data.Repositories;
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
        public HoaDonServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> CreateHoaDon(HoaDonDTO hoaDonDTO)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync("api/HoaDon/TaoHoaDonOnlineDTO", hoaDonDTO);
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

        public async Task<bool> DeleteHoaDon(string idHoaDon)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/HoaDon/DeleteHoaDon?idHoaDon={Uri.EscapeDataString(idHoaDon)}");
            return response.IsSuccessStatusCode && await response.Content.ReadAsAsync<bool>();
        }

        public async Task<List<HoaDonTest>> GetAllHoaDon()
        {
            return await _httpClient.GetFromJsonAsync<List<HoaDonTest>>("api/HoaDon/GetHoaDonOnlineAdmin");

		}

        public async Task<List<HoaDonChoDTO>> GetAllHoaDonCho()
        {
            return await _httpClient.GetFromJsonAsync<List<HoaDonChoDTO>>("api/HoaDon/GetAllHoaDonCho");
        }

        public async Task<List<HoaDonViewModel>> GetHoaDon()
        {
            return await _httpClient.GetFromJsonAsync<List<HoaDonViewModel>>("api/HoaDon/GetHoaDonOnline");
        }

        public async Task<List<HoaDonTest>> GetHoaDonOnline(string idNguoiDung)
        {
            return await _httpClient.GetFromJsonAsync<List<HoaDonTest>>($"api/HoaDon/GetHoaDonOnlineTest?idNguoiDung={idNguoiDung}");
        }
        public async Task<HoaDonTest> GetHoaDonOnlineById(string idHoaDon, string idNguoiDung)
        {
            return await _httpClient.GetFromJsonAsync<HoaDonTest>($"api/HoaDon/GetHoaDonOnlineById?idHoadon={idHoaDon}&idNguoiDung={idNguoiDung}");
        }
        public async Task<HoaDonTest> GetHoaDonOnlineByMa(string Ma)
        {
            return await _httpClient.GetFromJsonAsync<HoaDonTest>($"api/HoaDon/GetHoaDonOnlineByMa?Ma={Ma}");
        }

        public async Task<List<KhachHang>> GetKhachHangs()
        {
            var lst = await _httpClient.GetFromJsonAsync<List<KhachHang>>("api/HoaDon/GetAllKhachHang");

            return lst;
        }

        public async Task<string> GetPayMent(string idHoaDon)
        {
            return await _httpClient.GetStringAsync($"api/HoaDon/GetPTThanhToan?idhoadon={idHoaDon}");
        }

        public async Task<HoaDon> TaoHoaDonTaiQuay(HoaDon hoaDon)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync("api/HoaDon/TaoHoaDonTaiQuay", hoaDon);
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

        public async Task<string> TaoKhachHang(KhachHang khachHang)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync("api/HoaDon/TaoKhachHang", khachHang);
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
        public async Task<bool> TaoPTTTChiTiet(string idHoaDon, string idPTTT, double soTien, int trangThai)
        {
            try
            {
                var res = await _httpClient.PostAsync($"api/PTThanhToanChiTiet/AddPhuongThucThanhToanChiTietTaiQuay?IdHoaDon={idHoaDon}&IdThanhToan={idPTTT}&SoTien={soTien}&TrangThai={trangThai}", null);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<bool>();
                }
                Console.WriteLine(await res.Content.ReadAsAsync<bool>());
                throw new Exception("Not IsSuccessStatusCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Not IsSuccessStatusCode");
            }
        }
        public async Task<string> GetPTTT(string ten)
        {
            return await _httpClient.GetStringAsync($"api/PTThanhToan/PhuongThucThanhToanByName?ten={ten}");
        }

        public async Task<bool> UpdateNgayHoaDon(string idHoaDon, DateTime? NgayThanhToan, DateTime? NgayNhan, DateTime? NgayShip)
        {
            try
            {
                var res = await _httpClient.PutAsync($"api/HoaDon/UpdateNgayHoaDonOnline?idHoaDon={idHoaDon}&NgayThanhToan={NgayThanhToan}&NgayNhan={NgayNhan}&NgayShip={NgayShip}", null);
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
        public async Task<bool> XoaPhuongThucThanhToanChiTietBangIdHoaDon(string idHoaDon)
        {
            try
            {
                var res = await _httpClient.DeleteAsync($"api/HoaDon/XoaPhuongThucThanhToanChiTietBangIdHoaDon?idHoaDon={idHoaDon}");
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

        public async Task<bool> UpdateTrangThaiGiaoHangHoaDon(string idHoaDon, string? idNguoiDung, int TrangThai, string? Lido, DateTime? ngayCapNhatGanNhat)
        {
            try
            {
                var res = await _httpClient.PutAsync($"api/HoaDon/UpdateTrangThaiGiaoHangHoaDon?idHoaDon={idHoaDon}&idNguoiDung={idNguoiDung}&trangThaiGiaoHang={TrangThai}&Lido={Lido}&ngayCapNhatGanNhat={ngayCapNhatGanNhat}", null);
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

        public async Task<bool> UpdateTrangThaiHoaDon(string idHoaDon, int TrangThai)
        {
            try
            {
                var res = await _httpClient.PutAsync($"api/HoaDon/UpdateTrangThaiHoaDonOnline?idHoaDon={idHoaDon}&TrangThaiThanhToan={TrangThai}", null);
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
        public async Task<bool> ThanhToanTaiQuay(HoaDon hoaDon)
        {
            try
            {
                var res = await _httpClient.PutAsJsonAsync("api/HoaDon/ThanhToanTaiQuay", hoaDon);
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

        public async Task<bool> UpdateDiaChi(string idHoaDon, string diaChi)
        {
            try
            {
                var res = await _httpClient.PutAsync($"api/HoaDon/UpdateDiaChi?idHoaDon={idHoaDon}&diaChi={diaChi}", null);
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
