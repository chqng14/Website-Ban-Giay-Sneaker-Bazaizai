using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.HoaDonChiTietDTO
{
    public class HoaDonChiTietDTO
    {
        public string? IdHoaDonChiTiet { get; set; }
        public string? IdHoaDon { get; set; }
        public string? IdSanPhamChiTiet { get; set; }
        public string? IdVoucher { get; set; }
        public string? IdThongTinGH { get; set; }
        public int? SoLuong { get; set; }
        public double? GiaGoc { get; set; }
        public double? GiaBan { get; set; }
        public double? TongTien { get; set; }
        public double? TienShip { get; set; }
        public double? TienGiam { get; set; }
        public string? MoTa { get; set; }
        public int? TrangThai { get; set; }
    }
}
