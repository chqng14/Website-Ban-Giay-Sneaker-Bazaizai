using App_Data.ViewModels.HoaDonChiTietDTO;

namespace App_View.IServices
{
    public interface IHoaDonChiTietServices
    {
        Task<List<HoaDonChiTietViewModel>> GetAllHoaDonChiTiet();
        Task<bool> CreateHoaDonChiTiet(HoaDonChiTietDTO hoaDonChiTietDTO);
        Task<bool> UpdateHoaDonChiTiet(HoaDonChiTietDTO hoaDonChiTietDTO);
        Task<bool> DeleteHoaDonChiTiet(string idHoaDonChiTiet);
    }
}
