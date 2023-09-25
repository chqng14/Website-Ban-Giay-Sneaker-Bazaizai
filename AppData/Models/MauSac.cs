using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class MauSac
    {
        [Key]
        public string? IdMauSac { get; set; }
        public string? MaMauSac { get; set; }
        public string? TenMauSac { get; set; }
        public int? TrangThai { get; set; }
        public virtual List<SanPhamChiTiet> SanPhamChiTiets { get; set; }
    }
}
