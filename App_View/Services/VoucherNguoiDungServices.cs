using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.Voucher;
using App_Data.ViewModels.VoucherNguoiDung;
using App_View.IServices;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System;
using System.Net.Http;
using static App_Data.Repositories.TrangThai;

namespace App_View.Services
{
    public class VoucherNguoiDungServices : IVoucherNguoiDungServices
    {
        private readonly HttpClient _httpClient;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        DbSet<VoucherNguoiDung> voucherNguoiDung;
        DbSet<Voucher> voucher;
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

        public async Task<string> AddVoucherNguoiDungTuAdmin(AddVoucherRequestDTO addVoucherRequestDTO)
        {
            // / api / VoucherNguoiDung / AddVoucherNguoiDungTuAdmin
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/VoucherNguoiDung/AddVoucherNguoiDungTuAdmin", addVoucherRequestDTO);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    return content;
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi xảy ra: {e}");
                return "false";
            }
        }



        //hàm này để check xem voucher đó đã có trong id người dùng chưa
        public bool CheckVoucherInUser(string ma)
        {
            string voucherKhaDung = DbContextModel.vouchers
                .FirstOrDefault(c => c.MaVoucher == ma && c.TrangThai == (int)TrangThaiVoucher.HoatDong).IdVoucher;

            if (voucherKhaDung == null)
            {
                return false;
            }

            var existsInVoucherNguoiDung = DbContextModel.voucherNguoiDungs
                .Any(vnd => vnd.IdVouCher == voucherKhaDung);

            return !existsInVoucherNguoiDung;
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

        public async Task<string> TangVoucherNguoiDungMoi(string ma)
        {
            ///api/VoucherNguoiDung/TangVoucherChoNguoiDungMoi
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/VoucherNguoiDung/TangVoucherChoNguoiDungMoi", ma);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    return content;
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi xảy ra: {e}");
                return "false";
            }
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
