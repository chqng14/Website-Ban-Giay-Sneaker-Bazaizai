using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.SanPhamChiTietViewModel
{
    public class ItemShopViewModel
    {
        public string? IdChiTietSp { get; set; }
        public string? ThuongHieu { get; set; }
        public string? TenSanPham { get; set;}
        public string? GiaMin { get; set; }
        public string? GiaMax { get; set; }
        public double? SoSao { get; set; }
        public int? SoLanDanhGia { get; set; }
        public string? MoTaNgan { get; set; }
        public string? MoTaSanPham { get; set; }
        public double? GiaBan { get; set; }
        public double? GiaKhuyenMai { get; set; }
        public string? Anh { get; set; }
    }
}
