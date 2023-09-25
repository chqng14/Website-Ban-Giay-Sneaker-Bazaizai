using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class KieuDeGiay
    {
        public string? IdKieuDeGiay { get; set; }
        public string?  MaKieuDeGiay { get; set; }
        public string? TenKieuDeGiay { get; set; }
        public int? Trangthai { get; set; }
        public virtual List<SanPhamChiTiet> SanPhamChiTiets { get; set; }
    }
}
