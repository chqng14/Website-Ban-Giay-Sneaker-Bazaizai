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
                var response = await _httpClient.PostAsJsonAsync("/api/Voucher/CreateVoucher", voucherDTO);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<bool>();
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

        public async Task<bool> DeleteVoucher(string id)
        {
            try
            {
                var response = await _httpClient.PutAsync($"/api/Voucher/DeleteVoucher/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<bool>();
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi xảy ra: {e}");
                return false;
            }

        }
        public async Task<bool> DeleteVoucherWithList(List<string> Id)
        {
            try
            {
                foreach (string item in Id)
                {
                    var response = await _httpClient.PutAsync($"/api/Voucher/DeleteVoucher/{item}", null);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi xảy ra: {e}");
                return false;
            }
        }
        public async Task<bool> RestoreVoucherWithList(List<string> Id)
        {
            try
            {
                foreach (string item in Id)
                {
                    var response = await _httpClient.PutAsync($"/api/Voucher/RestoreVoucher/{item}", null);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi xảy ra: {e}");
                return false;
            }
        }
        public Task<List<Voucher>> GetAllVoucher()
        {
            return _httpClient.GetFromJsonAsync<List<Voucher>>("/api/Voucher/GetVoucher");
        }

        public async Task<Voucher> GetVoucherByMa(string ma)
        {
            return await _httpClient.GetFromJsonAsync<Voucher>($"/api/Voucher/GetVoucherByMa/{ma}");
        }

        public async Task<VoucherDTO> GetVoucherDTOById(string id)
        {
            return await _httpClient.GetFromJsonAsync<VoucherDTO>($"/api/Voucher/GetVoucherDTOByMa/{id}");
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

        public async Task<bool> UpdateVoucherAfterUseIt(string ma)
        {
            ///api/Voucher/UpdateVoucherAfterUseIt/{ma
            try
            {
                var reponse = await _httpClient.PutAsync($"api/Voucher/UpdateVoucherAfterUseIt/{ma}", null);
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
