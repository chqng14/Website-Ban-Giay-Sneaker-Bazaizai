using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.HoaDonChiTietDTO
{
    public class HoaDonChiTietTaiQuay
    {
        public string? IdHoaDonChiTiet { get; set; }
        public string? IdHoaDon { get; set; }
        public string? IdSanPhamChiTiet { get; set; }
        public string? MaSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public int? SoLuong { get; set; }
        public double? GiaGoc { get; set; }
        public double? GiaBan { get; set; }
        public int? TrangThai { get; set; }
    }
}
