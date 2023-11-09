using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class Anh
    {
        [Key]
        public string? IdAnh { get; set; }
        public string? Url { get; set; }
        public int? TrangThai { get; set; }
        public string? IdSanPhamChiTiet { get; set; }
        public DateTime? NgayTao { get; set; }
        public virtual SanPhamChiTiet SanPhamChiTiets { get; set; }
    }
}
