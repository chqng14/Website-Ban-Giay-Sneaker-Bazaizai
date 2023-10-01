using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.Voucher;

namespace App_View.IServices
{
    public interface IGioHangChiTietServices
    {
        Task<List<GioHangChiTietDTO>> GetAllGioHang();
        Task<bool> CreateGioHang(GioHangChiTietDTO GioHangChiTietDTO);
        Task<bool> UpdateGioHang(GioHangChiTietDTO GioHangChiTietDTO);
        Task<bool> DeleteGioHang(string id);
    }
}
