
using App_Data.Models;
using App_Data.ViewModels.GioHang;
using App_Data.ViewModels.SanPhamChiTiet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.GioHangChiTiet
{
    public class GioHangChiTietDTO
    {
        public SanPhamChiTietDTO.SanPhamChiTietDTO sanPhamChiTietDTO { get; set; }
        public GioHangDTO GioHangDTO { get; set; }
        public string IdGioHangChiTiet { get; set; }
        public string IdNguoiDung { get; set; }
        public string IdSanPhamCT { get; set; }
        public string? TenSanPham { get; set; }
        public string? TenMauSac { get; set; }
        public int? TenKichCo { get; set; }
        public string? TenThuongHieu { get; set; }
        public int? SoLuong { get; set; }
        public double? GiaGoc { get; set; }
        public double? GiaBan { get; set; }
        public int? TrangThai { get; set; }
        public int? TrangThaiSanPham { get; set; }
        public List<string?> LinkAnh { get; set; }
    }
}
