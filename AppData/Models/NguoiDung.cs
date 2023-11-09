using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class NguoiDung: IdentityUser<string>
    {
        public string? MaNguoiDung { get; set; }
        public string? TenNguoiDung { get; set; }
        public int? GioiTinh { get; set; }
        public string? DiaChi { get; set; }
        public DateTime? NgaySinh { get; set; }
        public double? TongChiTieu { get; set; } = 0;
        public int? SuaDoi { get; set; } = 1;
        public string? AnhDaiDien { get; set; }
        public int? TrangThai { get; set; }
        public virtual List<VoucherNguoiDung> VoucherNguoiDungs { get; set; }
        public virtual List<ThongTinGiaoHang> ThongTinGiaoHangs { get; set; }
        public virtual List<KhachHang> KhachHangs { get; set; }
        public virtual List<HoaDon> HoaDons { get; set; }
        public virtual List<SanPhamYeuThich> SanPhamYeuThich { get; set; }
        public virtual List<DanhGia> DanhGias { get; set; }
    }
}
