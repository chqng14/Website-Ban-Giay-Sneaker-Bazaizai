using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class SanPhamYeuThich
    {
        public string? IdSanPhamYeuThich { get; set; }
        public string? IdNguoiDung { get; set; }
        public string? IdSanPhamChiTiet { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
        public virtual SanPhamChiTiet SanPhamChiTiet { get; set; }
    }
}
