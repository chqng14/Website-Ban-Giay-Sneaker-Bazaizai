using App_Data.ViewModels.DonHang;
using App_Data.ViewModels.GioHangChiTiet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.DonHangChiTiet
{
    public class DonHangChiTietViewModel: DonHangViewModel
    {
        public double TongTienSanPham => SanPhamGioHangViewModels!.Sum(x => x.GiaSanPham * x.SoLuong);
        public string? DiaChiNhanHang { get; set; }
        public string? TenNguoiNhan { get; set; }
        public string? SDT { get; set; }
        public double? Vouchershop { get; set; }
        public string? PhuongThucThanhToan { get; set; }
    }
}
