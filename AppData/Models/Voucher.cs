using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class Voucher
    {
        public string? IdVoucher { get; set; }
        public string? MaVoucher { get; set; }
        [Required(ErrorMessage = "Tên voucher là trường bắt buộc.")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Tên voucher phải có ít nhất 5 ký tự.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Tên voucher không được chứa ký tự đặc biệt.")]
        public string? TenVoucher { get; set; }
        public int? DieuKien { get; set; }
        public string? PhamViSanPham { get; set; }
        public int? LoaiHinhUuDai { get; set; }
        public int? SoLuong { get; set; }
        public double? MucUuDai { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int? TrangThai { get; set; }
        public virtual List<VoucherNguoiDung> VoucherNguoiDungs { get; set; }
        public virtual List<HoaDon> HoaDon { get; set; }
    }
}
