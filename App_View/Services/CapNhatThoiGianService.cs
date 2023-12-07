using App_Data.DbContextt;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.Voucher;
using App_View.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using static App_Data.Repositories.TrangThai;

namespace App_View.Services
{
    public class CapNhatThoiGianService
    {
        BazaizaiContext _dbContext = new BazaizaiContext();
        private readonly HttpClient _httpClient;
        bool loading = false;
        public CapNhatThoiGianService()
        {         
            _dbContext = new BazaizaiContext();
            _httpClient =new HttpClient();
        }
        public async Task<bool> CapNhatThongTinKhuyenMai()
        {
            try
            {
                var response = await _httpClient.PutAsync("https://localhost:7038/api/AutoUpdate/CapNhatThongTinKhuyenMai", null);
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
        public async Task<bool> CapNhatThoiGianVoucher()
        {
            try
            {
                var response = await _httpClient.PutAsync("https://localhost:7038/api/AutoUpdate/CapNhatThoiGianVoucher", null);
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


    }
}
