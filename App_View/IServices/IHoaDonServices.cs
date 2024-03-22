using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.HoaDon;

namespace App_View.IServices
{
    public interface IHoaDonServices
    {
        Task<List<HoaDonTest>> GetAllHoaDon();
        Task<string> CreateHoaDon(HoaDonDTO hoaDonDTO);
        Task<bool> UpdateTrangThaiHoaDon(string idHoaDon, int TrangThai);
        Task<bool> UpdateNgayHoaDon(string idHoaDon, DateTime? NgayThanhToan, DateTime? NgayNhan, DateTime? NgayShip);
        Task<bool> UpdateDiaChi(string idHoaDon, string diaChi);
        Task<bool> UpdateTrangThaiGiaoHangHoaDon(string idHoaDon, string? idNguoiDung, int TrangThai, string? Lido, DateTime? ngayCapNhatGanNhat);
        Task<bool> DeleteHoaDon(string idHoaDon);
        Task<List<HoaDonChoDTO>> GetAllHoaDonCho();
        Task<List<HoaDonViewModel>> GetHoaDon();
        Task<List<HoaDonTest>> GetHoaDonOnline(string idNguoiDung);
        Task<HoaDonTest> GetHoaDonOnlineById(string idHoaDon, string idNguoiDung);
        Task<HoaDonTest> GetHoaDonOnlineByMa(string Ma);
        Task<HoaDon> TaoHoaDonTaiQuay(HoaDon hoaDon);
        Task<string> TaoKhachHang(KhachHang khachHang);
        Task<List<KhachHang>> GetKhachHangs();
        Task<string> GetPayMent(string idHoaDon);
        Task<bool> TaoPTTTChiTiet(string idHoaDon, string idPTTT, double soTien, int trangThai);
        Task<string> GetPTTT(string ten);
        Task<bool> XoaPhuongThucThanhToanChiTietBangIdHoaDon(string idHoaDon);
        Task<bool> ThanhToanTaiQuay(HoaDon hoaDon);
    }
}
