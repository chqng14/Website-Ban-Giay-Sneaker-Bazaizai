using App_Data.DbContext;

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
            _httpClient = new HttpClient();
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
