using App_Data.ViewModels.GioHangChiTiet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.DonHang
{
    public class DonHangViewModel
    {
        public List<SanPhamGioHangViewModel>? SanPhamGioHangViewModels { get; set; } = new List<SanPhamGioHangViewModel> { };
        public string? IdDonHang { get; set; }
        public string? MaDonHang { get; set; }
        public double TongTien { get; set; }
        public double PhiShip { get; set; }
        public string? NgayTao { get; set; }
        public int TrangThaiHoaDon { get; set; }
        public string? NgayGiaoDuKien { get; set; }
    }
}
