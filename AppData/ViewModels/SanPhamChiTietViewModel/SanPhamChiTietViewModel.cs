using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.SanPhamChiTietViewModel
{
    public class SanPhamChiTietViewModel
    {
        public string? IdChiTietSp { get; set; }

        public string? Day { get; set; }
        public string? Ma { get; set; }
        public string? MoTa { get; set; }
        public int? SoLuongTon { get; set; }
        public int? SoLuongDaBan { get; set; }
        public double? GiaBan { get; set; }
        public string NgayTao { get; set; }
        public double? GiaNhap { get; set; }
        public double? GiaThucTe { get; set; }
        public string? KhoiLuong { get; set; }
        public string? SanPham { get; set; }
        public string? KieuDeGiay { get; set; }
        public string? XuatXu { get; set; }
        public string? ChatLieu { get; set; }
        public string? MauSac { get; set; }
        public int? KichCo { get; set; }
        public string? LoaiGiay { get; set; }
        public string? ThuongHieu { get; set; }
        public int? TrangThai { get; set; }
        public List<string>? ListTenAnh { get; set; }
    }
}
