namespace App_View.Services
{
    public class CapNhatThoiGianService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CapNhatThoiGianService> _logger;

        public CapNhatThoiGianService(
            HttpClient httpClient,
            ILogger<CapNhatThoiGianService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> CapNhatThongTinKhuyenMai(
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var response = await _httpClient.PutAsync(
                    "api/AutoUpdate/CapNhatThongTinKhuyenMai",
                    null,
                    cancellationToken);

                return response.IsSuccessStatusCode
                    && await response.Content.ReadAsAsync<bool>(cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Không thể cập nhật trạng thái khuyến mãi.");
                return false;
            }
        }

        public async Task<bool> CapNhatThoiGianVoucher(
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var response = await _httpClient.PutAsync(
                    "api/AutoUpdate/CapNhatThoiGianVoucher",
                    null,
                    cancellationToken);

                return response.IsSuccessStatusCode
                    && await response.Content.ReadAsAsync<bool>(cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Không thể cập nhật trạng thái voucher.");
                return false;
            }
        }
    }
}
