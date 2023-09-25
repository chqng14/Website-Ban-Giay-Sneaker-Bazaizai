using App_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class KichCo
    {
        public string? IdKichCo { get; set; }
        public string? MaKichCo { get; set; }
        public int? SoKichCo { get; set; }
        public int? TrangThai { get; set; }
        public virtual List<SanPhamChiTiet> SanPhamChiTiets { get; set; }
    }
}
