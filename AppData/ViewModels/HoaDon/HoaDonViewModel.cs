using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.HoaDon
{
    public class HoaDonViewModel
    {
        public string? IdHoaDon { get; set; }
        public string? IdNguoiDung { get; set; }
        public string? IdKhachHang { get; set; }
        public string? IdVoucher { get; set; }
        public string? IdThongTinGH { get; set; }
        public string? MaHoaDon { get; set; }
        public string? MaVoucher { get; set; }
        public string? TenNguoiNhan { get; set; }
        public string? DiaChi { get; set; }
        public string? SDT { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public DateTime? NgayShip { get; set; }
        public DateTime? NgayNhan { get; set; }
        public DateTime? NgayGiaoDuKien { get; set; }
        public double? TienShip { get; set; }
        public double? TienGiam { get; set; }
        public double? TongTien { get; set; }
        public string? MoTa { get; set; }
        public string? LiDoHuy { get; set; }
        public int? TrangThaiGiaoHang { get; set; }
        public int? TrangThaiThanhToan { get; set; }
        public string? LoaiThanhToan { get; set; }
    }
}
