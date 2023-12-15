using App_Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.Voucher
{
    public class VoucherDTO
    {
        public string? IdVoucher { get; set; }
        public string? MaVoucher { get; set; }
        [Required(ErrorMessage = "Tên voucher là trường bắt buộc.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Tên voucher phải nhập từ 2-25 kí tự")]
        public string? TenVoucher { get; set; }
        [Required(ErrorMessage = "Điều kiện là trường bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Điều kiện phải là số nguyên không âm.")]
        public int? DieuKien { get; set; }
        [Required(ErrorMessage = "Loại hình khuyến mãi là trường bắt buộc.")]
        public int? LoaiHinhUuDai { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Trường cần nhập tối đa từ 0 đến 999.999.999.")]
        public int? SoLuong { get; set; }
        [CustomMucUuDaiValidation]
        public double? MucUuDai { get; set; }
        public DateTime NgayTao { get; set; }
        [Required(ErrorMessage = "Ngày bắt đầu là trường bắt buộc.")]
        public DateTime NgayBatDau { get; set; }
        [Required(ErrorMessage = "Ngày kết thúc là trường bắt buộc.")]
        [CustomNgayKetThucValidation(ErrorMessage = "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.")]
        public DateTime NgayKetThuc { get; set; }
        public int? TrangThai { get; set; }
        public int SoLuongVoucherDaIn { get; set; }
        public int SoLuongVoucherChuaIN { get; set; }
    }
}
