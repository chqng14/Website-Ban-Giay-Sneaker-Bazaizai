using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDon;

namespace App_View.IServices
{
    public interface IHoaDonServices
    {
        Task<List<HoaDon>> GetAllHoaDon();
        Task<bool> CreateHoaDon(HoaDonDTO hoaDonDTO);
        Task<bool> UpdateHoaDon(HoaDon HoaDon);
        Task<bool> DeleteHoaDon(string idHoaDon);
    }
}
