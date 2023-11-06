using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDon;

namespace App_View.IServices
{
    public interface IHoaDonServices
    {
        Task<List<HoaDon>> GetAllHoaDon();
        Task<string> CreateHoaDon(HoaDonDTO hoaDonDTO);
        Task<bool> UpdateTrangThaiHoaDon(string idHoaDon, int TrangThai);
        Task<bool> UpdateNgayHoaDon(string idHoaDon, DateTime? NgayThanhToan, DateTime? NgayNhan, DateTime? NgayShip);
        Task<bool> DeleteHoaDon(string idHoaDon);
        Task<List<HoaDonChoDTO>> GetAllHoaDonCho();
        Task<List<HoaDonViewModel>> GetHoaDon();
        Task<List<HoaDonTest>> GetHoaDonOnline(string idNguoiDung);
        Task<HoaDon> TaoHoaDonTaiQuay(HoaDon hoaDon);
        Task<string> GetPayMent(string idHoaDon);
    }
}
