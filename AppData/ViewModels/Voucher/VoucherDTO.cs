﻿using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Số lượng là trường hợp bắt buộc.")]
        public int? SoLuong { get; set; }
        [CustomMucUuDaiValidation]
        [Required(ErrorMessage = "Mức ưu đãi là trường bắt buộc.")]
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
    public class CustomMucUuDaiValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (VoucherDTO)validationContext.ObjectInstance;

            if (model.LoaiHinhUuDai == 0)
            {
                if (value is double mucUuDai && mucUuDai <= 0)
                {
                    return new ValidationResult("Số tiền giảm phải lớn hơn 0.");
                }
            }
            else if (model.LoaiHinhUuDai == 1)
            {
                if (value is double mucUuDai && (mucUuDai <= 0 || mucUuDai > 100))
                {
                    return new ValidationResult("% giảm phải nằm trong khoảng từ 0 đến 100.");
                }
            }
            else if (model.LoaiHinhUuDai == 2)
            {
                // Kiểm tra mức ưu đãi theo điều kiện riêng cho LoaiHinhUuDai là 2
                // Điều kiện này tương tự với khi LoaiHinhUuDai là 0
                if (value is double mucUuDai && mucUuDai <= 0)
                {
                    return new ValidationResult("Số tiền giảm đãi phải lớn hơn 0.");
                }
            }

            return ValidationResult.Success;
        }
    }
    public class CustomNgayKetThucValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ngayKetThuc = (DateTime)value;
            var model = (VoucherDTO)validationContext.ObjectInstance;

            if (ngayKetThuc <= model.NgayBatDau)
            {
                return new ValidationResult("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
            }

            return ValidationResult.Success;
        }
    }
}
