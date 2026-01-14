using App_Data.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly BazaizaiContext _dbContext;

        public HealthController(BazaizaiContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Kiểm tra kết nối database
        /// </summary>
        /// <returns>Trạng thái kết nối database</returns>
        [HttpGet]
        public async Task<IActionResult> CheckHealth()
        {
            try
            {
                // Thử kết nối đến database bằng cách thực hiện một query đơn giản
                await _dbContext.Database.CanConnectAsync();
                
                // Trích xuất tên database từ connection string một cách an toàn
                var connectionString = _dbContext.Database.GetConnectionString();
                var databaseName = "Unknown";
                
                if (!string.IsNullOrEmpty(connectionString))
                {
                    var parts = connectionString.Split(';');
                    var dbPart = parts.FirstOrDefault(x => x.Contains("Database", StringComparison.OrdinalIgnoreCase));
                    if (dbPart != null)
                    {
                        var dbValue = dbPart.Split('=');
                        if (dbValue.Length > 1)
                        {
                            databaseName = dbValue[1].Trim();
                        }
                    }
                }
                
                return Ok(new
                {
                    status = "Healthy",
                    message = "Kết nối database thành công",
                    timestamp = DateTime.UtcNow,
                    database = databaseName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "Unhealthy",
                    message = "Kết nối database thất bại",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Kiểm tra chi tiết kết nối database
        /// </summary>
        /// <returns>Thông tin chi tiết về kết nối database</returns>
        [HttpGet("detailed")]
        public async Task<IActionResult> CheckDetailedHealth()
        {
            var timestamp = DateTime.UtcNow;
            
            try
            {
                var canConnect = await _dbContext.Database.CanConnectAsync();
                var connectionString = _dbContext.Database.GetConnectionString();
                
                // Ẩn mật khẩu trong connection string khi trả về
                var safeConnectionString = "";
                if (!string.IsNullOrEmpty(connectionString))
                {
                    var parts = connectionString.Split(';')
                        .Where(x => !x.Contains("Password", StringComparison.OrdinalIgnoreCase));
                    safeConnectionString = string.Join(";", parts);
                }

                return Ok(new
                {
                    database = new
                    {
                        canConnect = canConnect,
                        connectionString = safeConnectionString,
                        errorMessage = ""
                    },
                    status = canConnect ? "Healthy" : "Unhealthy",
                    timestamp = timestamp
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    database = new
                    {
                        canConnect = false,
                        connectionString = "",
                        errorMessage = ex.Message
                    },
                    status = "Unhealthy",
                    timestamp = timestamp
                });
            }
        }
    }
}
