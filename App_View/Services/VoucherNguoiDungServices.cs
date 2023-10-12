using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.Voucher;
using App_Data.ViewModels.VoucherNguoiDung;
using App_View.IServices;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Net.Http;

namespace App_View.Services
{
    public class VoucherNguoiDungServices : IVoucherNguoiDungServices
    {
        private readonly HttpClient _httpClient;

        public VoucherNguoiDungServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddVoucherNguoiDung(string MaVoucher, string idNguoiDung)
        {
            var httpClient = new HttpClient();
            string apiUrl = $"https://localhost:7038/api/VoucherNguoiDung/AddVoucherNguoiDung?MaVoucher={MaVoucher}&idNguoiDung={idNguoiDung}";
            var response = await httpClient.PostAsync(apiUrl, null);
            return true;
        }

        public Task<List<VoucherNguoiDungDTO>> GetAllVouCherNguoiDung()
        {
            ///api/VoucherNguoiDung/GetAllVoucherNguoiDung
            return _httpClient.GetFromJsonAsync<List<VoucherNguoiDungDTO>>("/api/VoucherNguoiDung/GetAllVoucherNguoiDung");
        }

        public async Task<List<VoucherNguoiDungDTO>> GetAllVoucherNguoiDungByID(string id)
        {

            return await _httpClient.GetFromJsonAsync<List<VoucherNguoiDungDTO>>($"/api/VoucherNguoiDung/GetAllVoucherNguoiDungByID{id}");
        }

        public async Task<VoucherNguoiDung> GetVoucherNguoiDungById(string id)
        {
            return await _httpClient.GetFromJsonAsync<VoucherNguoiDung>($"/api/VoucherNguoiDung/GetChiTietVoucherNguoiDungByID{id}");
        }

        public async Task<bool> UpdateVoucherNguoiDungSauKhiDung(VoucherNguoiDungDTO VcDTO)
        {
            try
            {
                var reponse = await _httpClient.PutAsJsonAsync($"/api/VoucherNguoiDung/UpdateVoucherNguoiDung{VcDTO.IdVouCher}", VcDTO);
                if (reponse.IsSuccessStatusCode)
                {
                    return await reponse.Content.ReadAsAsync<bool>();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi xảy ra: {e}");
                return false;
            }
        }
    }
}
