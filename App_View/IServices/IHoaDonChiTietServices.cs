using App_Data.Models;
using App_Data.ViewModels.HoaDonChiTietDTO;

namespace App_View.IServices
{
    public interface IHoaDonChiTietServices
    {
        Task<List<HoaDonChiTietViewModel>> GetAllHoaDonChiTiet();
        Task<bool> CreateHoaDonChiTiet(HoaDonChiTietDTO hoaDonChiTietDTO);
        Task<bool> UpdateHoaDonChiTiet(HoaDonChiTietDTO hoaDonChiTietDTO);
        Task<bool> DeleteHoaDonChiTiet(string idHoaDonChiTiet);
        Task<HoaDonChiTiet> ThemSanPhamVaoHoaDon(HoaDonChiTiet hoaDonChiTiet);
        Task<string> UpdateSoLuong(string idHD, string idSanPham,int SoLuongMoi, string SoluongTon);
        Task<string> XoaSanPhamKhoiHoaDon(string idHD, string idSanPham);
    }
}
