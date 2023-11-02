using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDon;

namespace App_View.IServices
{
    public interface IHoaDonServices
    {
        Task<List<HoaDon>> GetAllHoaDon();
        Task<string> CreateHoaDon(HoaDonDTO hoaDonDTO);
        Task<bool> UpdateHoaDon(string idHoaDon, int TrangThai);
        Task<bool> DeleteHoaDon(string idHoaDon);
        Task<List<HoaDonChoDTO>> GetAllHoaDonCho();
        Task<List<HoaDonViewModel>> GetHoaDon();
        Task<HoaDon> TaoHoaDonTaiQuay(HoaDon hoaDon);
    }
}
