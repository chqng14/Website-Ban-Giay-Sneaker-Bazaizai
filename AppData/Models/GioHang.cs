using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class GioHang
    {
        public string? IdNguoiDung { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? TrangThai { get; set; }
        public virtual IEnumerable<GioHangChiTiet> GioHangChiTiet { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
    }
}
