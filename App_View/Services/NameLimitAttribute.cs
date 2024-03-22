using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace App_View.Services
{
    public class NameLimitAttribute: ValidationAttribute
    {

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return new ValidationResult("Tên không được để trống.");
            }
            else
            {
                string regexPattern = @"^[\p{L}]{2,}( [\p{L}]{1,})+$";
                if (Regex.IsMatch((string)value, regexPattern))
                {                
                    return ValidationResult.Success!;
                }
                else return new ValidationResult("Tên của bạn không hợp lệ.");
            }

            return new ValidationResult("Tên của bạn không hợp lệ.");
        }
    }
}
