using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.Voucher;

namespace App_View.IServices
{
    public interface IGioHangChiTietServices
    {
        Task<List<GioHangChiTietDTO>> GetAllGioHang();
        Task<bool> CreateCartDetailDTO(GioHangChiTietDTOCUD gioHangChiTietDTOCUD);
        Task<bool> UpdateGioHang(string IdGioHangChiTiet, int SoLuong);
        Task<bool> DeleteGioHang(string id);
    }
}
