using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class SanPhamYeuThich
    {
        public Guid IDSanPhamYeuThich { get; set; }
        public Guid IDNguoiDung { get; set; }
        public Guid IDSanPhamChiTiet { get; set; }
        public int TrangThai { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
    }
}
