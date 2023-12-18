﻿using App_Data.ViewModels.Voucher;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class KhuyenMai
    {
        public string? IdKhuyenMai { get; set; }


        public string? MaKhuyenMai { get; set; }

        [Required(ErrorMessage = "Tên khuyến mãi là trường bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Tên khuyến mãi không được vượt quá 100 ký tự.")]
		[RegularExpression(@"^[a-zA-Z0-9\sàáảãạăắằẳẵặâấầẩẫậèéẻẽẹêềếểễệđìíỉĩịòóỏõọôốồổỗộơớờởỡợùúủũụưứừửữựỳỵỷỹ%]*$", ErrorMessage = "Tên Khuyến mại không được chứa ký tự đặc biệt.")]
		public string? TenKhuyenMai { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là trường bắt buộc.")]
        public DateTime? NgayBatDau { get; set; }
        [Required(ErrorMessage = "Ngày kết thúc là trường bắt buộc.")]
        [CustomNgayKetThucKhuyenMaiValidation(ErrorMessage = "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.")]
        public DateTime? NgayKetThuc { get; set; }

        [Required(ErrorMessage = "Loại hình khuyến mãi là trường bắt buộc.")]

        public int? LoaiHinhKM { get; set; }

        [Required(ErrorMessage = "Mức giảm là trường bắt buộc.")]
        [Range(0, 100, ErrorMessage = "Mức giảm phải nằm trong khoảng từ 0 đến 100.")]
        public decimal? MucGiam { get; set; }

        public int? TrangThai { get; set; }
        public string? Url { get; set; }
        public virtual ICollection<KhuyenMaiChiTiet>? KhuyenMaiChiTiet { get; set; }

    }
    public class CustomNgayKetThucKhuyenMaiValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if(value!=null)
            {
                var ngayKetThuc = (DateTime)value;
                var model = (KhuyenMai)validationContext.ObjectInstance;

                if (ngayKetThuc <= model.NgayBatDau)
                {
                    return new ValidationResult("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");

                }
                return ValidationResult.Success;
            }
                return new ValidationResult("Ngày kết thúc không được để trống.");
        }
    }
}
