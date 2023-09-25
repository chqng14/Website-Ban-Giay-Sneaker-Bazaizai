using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class SanPham
    {
        public string? IdSanPham { get; set; }
        public string? MaSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public int? Trangthai { get; set; }
        public virtual List<SanPhamChiTiet> SanPhamChiTiets { get; set; }
    }
}
