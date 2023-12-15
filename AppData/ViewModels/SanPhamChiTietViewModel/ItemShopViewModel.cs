using Microsoft.AspNetCore.Mvc.Rendering;
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
        public string? MaSanPham { get; set; }
        public string? ThuongHieu { get; set; }
        public string? MauSac { get; set; }
        public int? KichCo { get; set; }
        public string? TheLoai { get; set; }
        public string? TenSanPham { get; set;}
        public double? GiaMin { get; set; }
        public double? GiaThucTe { get; set; }
        public int? SoLuongTon { get; set; }
        public double? GiaGoc { get; set; }
        public double? GiaMax { get; set; }
        public double? SoSao { get; set; }
        public int? SoLanDanhGia { get; set; }
        public string? MoTaNgan { get; set; }
        public string? MoTaSanPham { get; set; }
        public double? GiaKhuyenMai { get; set; }
        public List<SelectListItem>? LstMauSac { get; set; }
        public string? Anh { get; set; }
        public bool IsKhuyenMai { get; set; }
        public bool IsNoiBat { get; set; }
        public bool IsNew { get; set; }
    }
}
