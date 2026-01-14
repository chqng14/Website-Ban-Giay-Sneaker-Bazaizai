using App_Data.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
                    try
                    {
                        var builder = new SqlConnectionStringBuilder(connectionString);
                        databaseName = builder.InitialCatalog ?? "Unknown";
                    }
                    catch
                    {
                        // Nếu không thể parse connection string, giữ giá trị mặc định
                        databaseName = "Unknown";
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
                    try
                    {
                        var builder = new SqlConnectionStringBuilder(connectionString);
                        builder.Password = "";
                        safeConnectionString = builder.ToString();
                    }
                    catch
                    {
                        // Nếu không thể parse, dùng phương pháp đơn giản hơn
                        var parts = connectionString.Split(';')
                            .Where(x => !x.Contains("Password", StringComparison.OrdinalIgnoreCase) 
                                     && !x.Contains("Pwd", StringComparison.OrdinalIgnoreCase));
                        safeConnectionString = string.Join(";", parts);
                    }
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
