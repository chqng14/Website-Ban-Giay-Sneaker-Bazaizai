using App_Data.Models;

namespace App_View.IServices
{
    public interface IPTThanhToanServices
    {
        Task<bool> CreatePTThanhToanAsync(string ten, string mota, int trangthai);
        Task<bool> UpdatePTThanhToanAsync(string idPhuongThucThanhToan, string ten, string mota, int trangthai);
        Task<bool> DeletePTThanhToanAsync(string idPhuongThucThanhToan);
        Task<List<PhuongThucThanhToan>> GetAllPTThanhToanAsync();
        Task<PhuongThucThanhToan> GetPTThanhToanByIDAsync(string idPhuongThucThanhToan);
        Task<string> GetPTThanhToanByNameAsync(string ten);
    }
}
