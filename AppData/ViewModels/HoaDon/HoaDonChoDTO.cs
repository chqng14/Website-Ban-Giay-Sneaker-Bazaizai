using App_Data.Models;
using App_Data.ViewModels.HoaDonChiTietDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.HoaDon
{
    public class HoaDonChoDTO
    {
        public string Id { get; set; }
        public string? IdNguoiDung { get; set; }
        public string? IdVoucher { get; set; }
        public string? IdKhachHang { get; set; }
        public string? MaHoaDon { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? TrangThaiGiaoHang { get; set; }
        public int? TrangThaiThanhToan { get; set; }
        public List<HoaDonChiTietTaiQuay>? hoaDonChiTietDTOs { get; set; }
    }
}
