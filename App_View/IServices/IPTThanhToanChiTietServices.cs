using App_Data.Models;

namespace App_View.IServices
{
    public interface IPTThanhToanChiTietServices
    {
        public Task<string> CreatePTThanhToanChiTietAsync(string IdHoaDon, string IdThanhToan, double SoTien);
        public Task<bool> UpdatePTThanhToanChiTietAsync(string IdPhuongThucThanhToanChiTiet, int TrangThai);
        public Task<bool> DeletePTThanhToanChiTietAsync(string idPhuongThucThanhToan);
        public Task<List<PhuongThucThanhToanChiTiet>> GetAllPTThanhToanChiTietAsync();
        public Task<PhuongThucThanhToanChiTiet> GetPTThanhToanChiTietByIDAsync(string idPhuongThucThanhToan);
    }
}
