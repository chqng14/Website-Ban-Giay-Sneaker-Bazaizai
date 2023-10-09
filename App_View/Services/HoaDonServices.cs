using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_View.IServices;
using System.Net.Http;

namespace App_View.Services
{
    public class HoaDonServices : IHoaDonServices
    {
        private readonly HttpClient _httpClient;
        public HoaDonServices()
        {
            _httpClient = new HttpClient();
        }
        public async Task<bool> CreateHoaDon(HoaDon HoaDon)
        {
            try
            {
                var res = await _httpClient.PostAsync($"https://localhost:7038/api/HoaDon/Create?IdVoucher={HoaDon.IdVoucher}&IdNguoiDung={HoaDon.IdNguoiDung}&IdKhachHang={HoaDon.IdKhachHang}&IdThongTinGH={HoaDon.IdThongTinGH}&NgayTao={HoaDon.NgayTao}&NgayThanhToan={HoaDon.NgayThanhToan}&NgayShip={HoaDon.NgayShip}&NgayNhan={HoaDon.NgayNhan}&TienShip={HoaDon.TienShip}&TienGiam={HoaDon.TienGiam}&TongTien={HoaDon.TongTien}&MoTa={HoaDon.MoTa}&TrangThai={HoaDon.TrangThai}&TrangThaiThanhToan={HoaDon.TrangThaiThanhToan}", null);
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

        public Task<bool> UpdateHoaDon(HoaDon HoaDon)
        {
            throw new NotImplementedException();
        }
    }
}
