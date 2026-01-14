using App_Data.DbContext;

namespace App_View.Services
{
    public class CapNhatThoiGianService
    {
        BazaizaiContext _dbContext = new BazaizaiContext();
        private readonly HttpClient _httpClient;
        bool loading = false;
        public CapNhatThoiGianService() : this(HttpClientFactory.CreateClient()) { }
        public CapNhatThoiGianService(HttpClient httpClient)
        {
            _dbContext = new BazaizaiContext();
            _httpClient = httpClient;
        }
        public async Task<bool> CapNhatThongTinKhuyenMai()
        {
            try
            {
                var response = await _httpClient.PutAsync("api/AutoUpdate/CapNhatThongTinKhuyenMai", null);
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
                var response = await _httpClient.PutAsync("api/AutoUpdate/CapNhatThoiGianVoucher", null);
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
