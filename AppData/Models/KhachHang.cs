using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class KhachHang
    {
        public string? IdKhachHang { get; set; }
        public string? IdNguoiDung { get; set; }
        public string? TenKhachHang { get; set; } 
        public string? SDT { get; set; }
        public int? TrangThai { get; set; }
        public virtual NguoiDung? NguoiDung { get; set; }
        public virtual List<HoaDon>? HoaDons { get; set; }
    }
}
