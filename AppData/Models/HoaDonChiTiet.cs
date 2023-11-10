using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class HoaDonChiTiet
    {
        public string? IdHoaDonChiTiet { get; set; }
        public string? IdHoaDon { get; set; }
        public string? IdSanPhamChiTiet { get; set; }
        public int? SoLuong { get; set; }
        public double? GiaGoc { get; set; }
        public double? GiaBan { get; set; }
        public int? TrangThai { get; set; }
        public virtual HoaDon? HoaDon { get; set; }
        public virtual SanPhamChiTiet? SanPhamChiTiet { get; set; }
    }
}
