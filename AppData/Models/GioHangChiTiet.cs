using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class GioHangChiTiet
    {
        public Guid IdGioHangChiTiet { get; set; }
        public Guid? IDNguoiDung { get; set; }
        public Guid? IDSanPhamCT { get; set; }
        public int? Soluong { get; set; }
        public double? GiaGoc { get; set; }
        public int TrangThai { get; set; }
        public virtual GioHang GioHang { get; set; }
        public virtual SanPhamChiTiet SanPhamChiTiet { get; set; }
    }
}
