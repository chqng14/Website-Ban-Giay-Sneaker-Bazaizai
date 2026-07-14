namespace App_View.Services
{
    public sealed class PromotionUpdateBackgroundService : BackgroundService
    {
        private static readonly TimeSpan InitialDelay = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan UpdateInterval = TimeSpan.FromMinutes(1);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PromotionUpdateBackgroundService> _logger;

        public PromotionUpdateBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<PromotionUpdateBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await Task.Delay(InitialDelay, stoppingToken);
                using var timer = new PeriodicTimer(UpdateInterval);

                do
                {
                    await UpdatePromotionsAsync(stoppingToken);
                }
                while (await timer.WaitForNextTickAsync(stoppingToken));
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Normal application shutdown.
            }
        }

        private async Task UpdatePromotionsAsync(CancellationToken stoppingToken)
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var service = scope.ServiceProvider.GetRequiredService<CapNhatThoiGianService>();

                var promotionUpdated = await service.CapNhatThongTinKhuyenMai(stoppingToken);
                var voucherUpdated = await service.CapNhatThoiGianVoucher(stoppingToken);

                if (!promotionUpdated || !voucherUpdated)
                {
                    _logger.LogWarning(
                        "Tác vụ cập nhật định kỳ chưa hoàn tất. Khuyến mãi: {PromotionUpdated}; voucher: {VoucherUpdated}.",
                        promotionUpdated,
                        voucherUpdated);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Normal application shutdown.
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Tác vụ cập nhật khuyến mãi định kỳ bị lỗi.");
            }
        }
    }
}
