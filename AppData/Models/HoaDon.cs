using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class HoaDon
    {
        public Guid IdHoaDon { get; set; }
        public Guid? IdUser { get; set; }
        public Guid? IdVoucher { get; set; }
        public Guid? IdThongTinGH { get; set; }
        public string? MaHoaDon { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public DateTime? NgayShip { get; set; }
        public DateTime? NgayNhan { get; set; }
        public double? TienShip { get; set; }
        public double? TienGiam { get; set; }
        public double? TongTien { get; set; }
        public string? MoTa { get; set; }
        public int? TrangThai { get; set; }
        public int? TrangThaiThanhToan { get; set; }
        public virtual List<HoaDonChiTiet> HoaDonChiTiet { get; set; }
        public virtual ThongTinGiaoHang ThongTinGiaoHang { get; set; }
        public virtual Voucher Voucher { get; set; }
        public virtual List<PhuongThucThanhToanChiTiet> PhuongThucThanhToanChiTiet { get; set; }
    }
}
