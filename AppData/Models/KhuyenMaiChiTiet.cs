using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class KhuyenMaiChiTiet
    {
        public string? IdKhuyenMaiChiTiet { get; set; }
        public string? IdKhuyenMai { get; set; }
        public string? IdSanPhamChiTiet { get; set; }
        public string? MoTa { get; set; }
        public int? TrangThai { get; set; }
        public virtual KhuyenMai KhuyenMai { get; set; }
        public virtual SanPhamChiTiet SanPhamChiTiet { get; set; }
    }
}
