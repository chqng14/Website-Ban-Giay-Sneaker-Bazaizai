using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace App_View.Services
{
    public class AgeLimitAttribute : ValidationAttribute
    {
        private int _maxAge;

        public AgeLimitAttribute(int maxAge)
        {
            _maxAge = maxAge;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success!;
            }
            string originalFormat = "dd/MM/yyyy";
            DateTime originalDate = DateTime.ParseExact((string)value, originalFormat, null);
            if (originalDate is DateTime dateOfBirth)
            {
                var today = DateTime.Today;
                var age = today.Year - dateOfBirth.Year;
                if (dateOfBirth.Date > today.AddYears(-15))
                {
                    return new ValidationResult("Người dùng phải từ 15 tuổi trở lên.");
                    //return new ValidationResult("Ngày sinh không hợp lệ.");
                }
                if (age <= _maxAge)
                {
                    return ValidationResult.Success!;
                }
            }
          
            return new ValidationResult("Ngày sinh không hợp lệ.");
        }
    }
}
