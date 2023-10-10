using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class GioHangChiTiet
    {
        public string? IdGioHangChiTiet { get; set; }
        public string? IdNguoiDung { get; set; }
        public string? IdSanPhamCT { get; set; }
        public int? Soluong { get; set; }
        public double? GiaGoc { get; set; }
        public double? GiaBan { get; set; }
        public int? TrangThai { get; set; }
        public virtual GioHang GioHang { get; set; }
        public virtual SanPhamChiTiet SanPhamChiTiet { get; set; }
    }
}
