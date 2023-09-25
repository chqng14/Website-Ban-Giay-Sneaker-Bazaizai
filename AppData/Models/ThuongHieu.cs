using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class ThuongHieu
    {
        public string? IdThuongHieu { get; set; }
        public string? MaThuongHieu { get; set; }
        public string? TenThuongHieu { get; set; }
        public int? TrangThai { get; set; }
        public virtual List<SanPhamChiTiet> SanPhamChiTiets { get; set; }
    }
}
