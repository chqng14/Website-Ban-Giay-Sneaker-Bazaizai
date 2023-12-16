using App_Data.ViewModels.HoaDon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.ThongKe
{
    public class ThongKeTheoSanPhamBanOnline
    {

        public string TenSp { get; set; }
        public string MaHoaDon { get; set; }
        public string MaSp { get; set; }
        public string? IdnguoiDung { get; set; }
        public string? IdDonHang { get; set; }
        public DateTime NgayTao { get; set; }
        public double TongTien { get; set; }
        public int SoLuong { get; set; }
        public double GiaGoc { get; set; }
        public double GiaBan { get; set; }
        public double GiaThucTe { get; set; }
        public int SoLuongKhachHang { get;set; }
        public double? TienGiam { get; set; }
        public string SoDt { get; set; }
        public string? Mactsp { get; set; }
        public List<SanPhamTest>? SanPham { get; set; }

    }
}
