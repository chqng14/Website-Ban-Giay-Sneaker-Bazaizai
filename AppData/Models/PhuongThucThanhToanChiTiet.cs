using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class PhuongThucThanhToanChiTiet
    {
        public string? IdPhuongThucThanhToanChiTiet { get; set; }
        public string? IdHoaDon { get; set; }
        public string? IdThanhToan { get; set; }
        public double? SoTien { get; set; }
        public int? TrangThai { get; set; }
        public virtual PhuongThucThanhToan PhuongThucThanhToan { get; set; }
        public virtual HoaDon HoaDons { get; set; }
    }
}
