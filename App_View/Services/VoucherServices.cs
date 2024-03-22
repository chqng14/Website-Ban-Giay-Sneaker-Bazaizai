using App_Data.Models;
using App_Data.ViewModels.Voucher;
using App_View.IServices;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;

namespace App_View.Services
{
    public class Voucherservices : IVoucherservices
    {
        private readonly HttpClient _httpClient;

        public Voucherservices(HttpClient httpClient)
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

        public async Task<bool> UpdateVoucherAfterUseIt(string idVoucher, string IdNguoiDung)
        {
            ///api/Voucher/UpdateVoucherAfterUseIt/{ma
            try
            {
                var reponse = await _httpClient.PutAsync($"api/Voucher/UpdateVoucherAfterUseIt/{idVoucher}/{IdNguoiDung}", null);
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

        public async Task<bool> CreateTaiQuay(VoucherDTO voucherDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Voucher/CreateVoucherTaiQuay", voucherDTO);
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

        public async Task<bool> DeleteTaiQuay(string id)
        {
            try
            {
                var response = await _httpClient.PutAsync($"/api/Voucher/DeleteVoucherTaiQuay/{id}", null);
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

        public async Task<bool> DeleteVoucherWithListTaiQuay(List<string> Id)
        {
            try
            {
                foreach (string item in Id)
                {
                    var response = await _httpClient.PutAsync($"/api/Voucher/DeleteVoucherTaiQuay/{item}", null);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi xảy ra: {e}");
                return false;
            }
        }

        public async Task<bool> RestoreVoucherWithListTaiQuay(List<string> Id)
        {
            try
            {
                foreach (string item in Id)
                {
                    var response = await _httpClient.PutAsync($"/api/Voucher/RestoreVoucherTaiQuay/{item}", null);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi xảy ra: {e}");
                return false;
            }
        }
        public async Task<bool> UpdateTaiQuay(VoucherDTO voucherDTO)
        {
            try
            {
                var reponse = await _httpClient.PutAsJsonAsync($"/api/Voucher/UpdateVoucherTaiQuay", voucherDTO);
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
        public async Task<bool> UpdateVoucherAfterUseItTaiQuay(string idVoucherNguoiDung)
        {
            try
            {
                var reponse = await _httpClient.PutAsync($"/api/Voucher/UpdateVoucherAfterUseItTaiQuay?idVoucherNguoiDung={idVoucherNguoiDung}", null);
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
        public async Task<bool> UpdateVouchersoluong(string idVoucher)
        {
            try
            {
                var reponse = await _httpClient.PutAsync($"https://localhost:7038/api/Voucher/UpdateVoucher/{idVoucher}", null);
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
        public async Task<bool> AddVoucherCungBanTaiQuay(string idVoucher, string idUser, int soluong)
        {
            try
            {
                var reponse = await _httpClient.PostAsync($"/api/Voucher/AddVoucherCungBanTaiQuay/{idVoucher}/{idUser}/{soluong}", null);
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

        public async Task<bool> UpdateTrangThaiKhiXuat(List<string> idVoucherNguoiDung)
        {
            try
            {
                var reponse = await _httpClient.PutAsJsonAsync($"/api/Voucher/UpdateTrangThaiKhiXuat", idVoucherNguoiDung);
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
