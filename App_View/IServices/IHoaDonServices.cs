using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;

namespace App_View.IServices
{
    public interface IHoaDonServices
    {
        Task<List<HoaDon>> GetAllHoaDon();
        Task<bool> CreateHoaDon(HoaDon HoaDon);
        Task<bool> UpdateHoaDon(HoaDon HoaDon);
        Task<bool> DeleteHoaDon(string idHoaDon);
    }
}
