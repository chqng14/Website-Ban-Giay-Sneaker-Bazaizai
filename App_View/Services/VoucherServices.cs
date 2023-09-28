using App_Data.Models;
using App_Data.ViewModels.Voucher;
using App_View.IServices;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;

namespace App_View.Services
{
    public class VoucherServices : IVoucherServices
    {
        private readonly HttpClient _httpClient;

        public VoucherServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateVoucher(VoucherDTO voucherDTO)
        {
            try
            {
                var reponse = await _httpClient.PostAsJsonAsync("/api/Voucher/CreateVoucher", voucherDTO);
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

        public Task<bool> DeleteVoucher(string id)
        {
            return _httpClient.GetFromJsonAsync<bool>($"/api/Voucher/DeleteVoucher/{id}");
        }

        public Task<List<Voucher>> GetAllVoucher()
        {
            return _httpClient.GetFromJsonAsync<List<Voucher>>("/api/Voucher/GetVoucher");
        }

        public Task<Voucher> GetVoucherById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateVoucher(VoucherDTO voucherDTO)
        {
            try
            {
                var reponse = await _httpClient.PutAsJsonAsync($"/api/Voucher/UpdateVoucher", voucherDTO);
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
