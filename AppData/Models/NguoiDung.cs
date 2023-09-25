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
        public DateTime? NgaySinh { get; set; }
        public string? AnhDaiDien { get; set; }
        public int? TrangThai { get; set; }
        public virtual List<VoucherNguoiDung> VoucherNguoiDungs { get; set; }
        //public virtual IEnumerable<ThongTinGiaoHang> ThongTinGiaoHangs { get; set; }
        //public virtual IEnumerable<KhachHang> KhachHangs { get; set; }
        public virtual List<ThongTinGiaoHang> ThongTinGiaoHangs { get; set; }
        public virtual List<KhachHang> KhachHangs { get; set; }

        public virtual List<SanPhamYeuThich> SanPhamYeuThich { get; set; }
    }
}
