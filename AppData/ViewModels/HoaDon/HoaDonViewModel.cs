using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.HoaDon
{
    public class HoaDonViewModel
    {
        public string? IdHoaDon { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayGiaoDuKien { get; set; }
        public double? TienShip { get; set; }
        public double? TienGiam { get; set; }
        public double? TongTien { get; set; }
        public string? MoTa { get; set; }
        public int? TrangThaiGiaoHang { get; set; }
        public int? TrangThaiThanhToan { get; set; }
    }
}
