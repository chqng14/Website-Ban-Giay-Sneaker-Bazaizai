using App_Data.DbContext;
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
    public class VoucherNguoiDungservices : IVoucherNguoiDungservices
    {
        private readonly HttpClient _httpClient;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        DbSet<VoucherNguoiDung> voucherNguoiDung;
        DbSet<Voucher> voucher;
        public VoucherNguoiDungservices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddVoucherNguoiDung(string MaVoucher, string idNguoiDung)
        {
            try
            {
                var httpClient = new HttpClient();
                string apiUrl = $"https://localhost:7038/api/VoucherNguoiDung/AddVoucherNguoiDung?MaVoucher={MaVoucher}&idNguoiDung={idNguoiDung}";
                var response = await httpClient.PostAsync(apiUrl, null);
                var check = await response.Content.ReadAsStringAsync();
                if (check == "true")
                {
                    return true;
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
        public bool CheckVoucherInUser(string ma, string idUser)
        {
            string voucherKhaDung = DbContextModel.Vouchers
                .FirstOrDefault(c => c.MaVoucher == ma && c.TrangThai == (int)TrangThaiVoucher.HoatDong).IdVoucher;

            if (voucherKhaDung == null)
            {
                return false;
            }

            var existsInVoucherNguoiDung = DbContextModel.VoucherNguoiDungs
                .Any(vnd => vnd.IdVouCher == voucherKhaDung && vnd.IdNguoiDung == idUser);

            return !existsInVoucherNguoiDung;
        }


        public Task<List<VoucherNguoiDungDTO>> GetAllVouCherNguoiDung()
        {
            ///api/VoucherNguoiDung/GetAllVoucherNguoiDung
            return _httpClient.GetFromJsonAsync<List<VoucherNguoiDungDTO>>("/api/VoucherNguoiDung/GetAllVoucherNguoiDung");
        }
        public async Task<VoucherTaiQuayDto> GetVocherTaiQuay(string? id)
        {
            ///api/VoucherNguoiDung/GetAllVoucherNguoiDung
            try
            {

                if (id == null)
                {
                    await _httpClient.GetFromJsonAsync<VoucherTaiQuayDto?>("/api/VoucherNguoiDung/GetVocherTaiQuay");
                }
                return await _httpClient.GetFromJsonAsync<VoucherTaiQuayDto?>("/api/VoucherNguoiDung/GetVocherTaiQuay?id=" + id);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<List<VoucherNguoiDungDTO>> GetAllVoucherNguoiDungByID(string id)
        {

            return await _httpClient.GetFromJsonAsync<List<VoucherNguoiDungDTO>>($"/api/VoucherNguoiDung/GetAllVoucherNguoiDungByID{id}");
        }

        public async Task<VoucherNguoiDung> GetVoucherNguoiDungById(string id)
        {
            return await _httpClient.GetFromJsonAsync<VoucherNguoiDung>($"/api/VoucherNguoiDung/GetChiTietVoucherNguoiDungByID{id}");
        }

        public async Task<bool> TangVoucherNguoiDungMoi(string ma)
        {
            ///api/VoucherNguoiDung/TangVoucherChoNguoiDungMoi
            try
            {
                var httpClient = new HttpClient();
                string apiUrl = $"https://localhost:7038/api/VoucherNguoiDung/TangVoucherChoNguoiDungMoi?ma={ma}";
                var response = await httpClient.PostAsync(apiUrl, null);
                var check = await response.Content.ReadAsStringAsync();
                if (check == "true")
                {
                    return true;
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

        public async Task<bool> UpdateVoucherNguoiDungsauKhiDung(VoucherNguoiDungDTO VcDTO)
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
