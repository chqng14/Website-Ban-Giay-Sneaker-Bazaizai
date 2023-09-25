using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class PhuongThucThanhToan
    {
        public string? IdPhuongThucThanhToan { get; set; }
        public string? MaPhuongThucThanhToan { get; set; }
        public string? TenPhuongThucThanhToan { get; set; }
        public string? MoTa { get; set; }
        public int? TrangThai { get; set; }
        public virtual List<PhuongThucThanhToanChiTiet> PhuongThucThanhToanChiTiets { get; set; }
    }
}
