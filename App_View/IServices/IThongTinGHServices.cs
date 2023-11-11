using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.ThongTinGHDTO;

namespace App_View.IServices
{
    public interface IThongTinGHServices
    {
        Task<List<ThongTinGHDTO>> GetAllThongTinDTO();
        Task<List<ThongTinGiaoHang>> GetAllThongTin();
        Task<List<ThongTinGiaoHang>> GetThongTinByIdUser(string idNguoiDung);
        Task<bool> CreateThongTin(ThongTinGHDTO thongTinGHDTO);
        Task<bool> UpdateThongTin(ThongTinGHDTO thongTinGHDTO);
        Task<bool> UpdateTrangThaiThongTin(string idThongTin);
        Task<bool> DeleteThongTin(string id);
    }
}
