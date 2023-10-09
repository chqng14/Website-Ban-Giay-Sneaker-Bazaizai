using App_Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class ChucVu: IdentityRole<string>
    {
        public string? MaChucVu { get; set; }
        public int? TrangThai { get; set; }
    }
}
