using App_Data.Models;
using App_Data.ViewModels.SanPhamChiTiet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.Cart
{
    public class GioHangDTO
    {
        public SanPhamChiTietDTO sanPhamChiTietDTO { get; set; }
        public NguoiDung NguoiDung { get; set; }
        public string IdGioHangChiTiet { get; set; }
        public string IdNguoiDung { get; set; }
        public string IdSanPhamCT { get; set; }
        public string? TenSanPham { get; set; }
        public string? TenMauSac { get; set; }
        public string? TenKichCo { get; set; }
        public int SoLuong { get; set; }
        public decimal? GiaGoc { get; set; }
        public int? TrangThai { get; set; }
        public string? LinkAnh { get; set; }
    }
}
