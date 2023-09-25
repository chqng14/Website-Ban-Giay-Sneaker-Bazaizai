using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class LoaiGiay
    {
        public string? IdLoaiGiay { get; set; }
        public string? MaLoaiGiay { get; set; }
        public string? TenLoaiGiay { get; set; }
        public int? TrangThai { get; set; }
        public virtual List<SanPhamChiTiet> SanPhamChiTiets { get; set; }
    }
}
