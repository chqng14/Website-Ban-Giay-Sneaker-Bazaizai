using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class DanhGia
    {
        [Key]
        public string IdDanhGia { get; set; }
        public string? BinhLuan { get; set; }
        public DateTime? NgayDanhGia { get; set; }
        public int? SaoSp { get; set; }
        public int? SaoVanChuyen { get; set; }
        public int? TrangThai { get; set; }
        public string? IdNguoiDung { get; set; }
        public string? IdSanPhamChiTiet { get; set; }
        public string? ParentId { get; set; }
        public virtual SanPhamChiTiet SanPhamChiTiet { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
        public DanhGia ParentDanhGia { get; set; }

        public ICollection<DanhGia> ChildDanhGias { get; set; }

    }
}
