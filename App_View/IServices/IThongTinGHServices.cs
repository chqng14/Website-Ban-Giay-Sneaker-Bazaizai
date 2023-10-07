using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;

namespace App_View.IServices
{
    public interface IThongTinGHServices
    {
        Task<List<ThongTinGiaoHang>> GetAllThongTin();
        Task<List<ThongTinGiaoHang>> GetThongTinByIdUser(string idNguoiDung);
        Task<bool> CreateThongTin(ThongTinGiaoHang thongTinGiaoHang);
        Task<bool> UpdateThongTin(ThongTinGiaoHang thongTinGiaoHang);
        Task<bool> DeleteThongTin(string id);
    }
}
