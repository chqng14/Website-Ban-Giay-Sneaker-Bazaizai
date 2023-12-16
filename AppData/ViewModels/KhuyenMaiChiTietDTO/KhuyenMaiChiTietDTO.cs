using App_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.KhuyenMaiChiTietDTO
{
    public class KhuyenMaiChiTietDTO
    {
        public string? IdKhuyenMaiChiTiet { get; set; }
        public string? IdKhuyenMai { get; set; }
        public string? IdSanPhamChiTiet { get; set; }
        public string? KhuyenMai { get; set; }
        public string? SanPham { get; set; }
        public string? MoTa { get; set; }
        public int? TrangThai { get; set; }

    }
}
