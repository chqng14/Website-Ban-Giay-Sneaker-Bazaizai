using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.GioHangChiTiet
{
    public class SanPhamGioHangViewModel
    {
        public string? IdGioHangChiTiet { get; set; }
        public string? IdSanPhamChiTiet { get; set; }
        public string? TenSanPham { get; set; }
        public string? TenThuongHieu { get; set; }
        public double GiaSanPham { get; set; }
        public int SoLuong { get; set; }
        public string? Anh { get; set; }
    }
}
