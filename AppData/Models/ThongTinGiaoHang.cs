using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class ThongTinGiaoHang
    {
        public string? IdThongTinGH { get; set; }
        public string? IdNguoiDung { get; set; }
        public string? TenNguoiNhan { get; set; }
        public string? SDT { get; set; }
        public string? DiaChi { get; set; }
        public int? TrangThai { get; set; }
        public virtual NguoiDung NguoiDungs { get; set; }
        public virtual List<HoaDon> HoaDon { get; set; }
    }
}
