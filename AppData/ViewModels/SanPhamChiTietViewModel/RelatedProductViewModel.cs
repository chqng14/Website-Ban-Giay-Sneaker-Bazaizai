using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.SanPhamChiTietViewModel
{
    public class RelatedProductViewModel
    {
        public string? IdSanPham { get; set; }
        public string? MaSanPham { get; set; }
        public string? SanPham { get; set; }
        public string? MauSac { get; set; }
        public int KichCo { get; set; }
        public double GiaBan { get; set; }
        public double GiaNhap { get; set; }
        public double KhoiLuong { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongDaBan { get; set; }
        public int TrangThai { get; set; }
        public string? Anh { get; set; }
    }
}
