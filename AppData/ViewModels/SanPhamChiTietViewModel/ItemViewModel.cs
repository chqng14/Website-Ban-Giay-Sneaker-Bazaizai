using App_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.SanPhamChiTietViewModel
{
    public class ItemViewModel
    {
        public string? IdChiTietSp { get; set; }
        public string? TenSanPham { get; set; }
        public string? ThuongHieu { get; set; }
        public bool? KhuyenMai { get; set; }
        public double? SoSao { get; set; }
        public int? SoLanDanhGia { get; set; }
        public double? GiaGoc { get; set; }
        public double? GiaKhuyenMai { get; set; }
        public string? Anh {get; set;}
    }
}
