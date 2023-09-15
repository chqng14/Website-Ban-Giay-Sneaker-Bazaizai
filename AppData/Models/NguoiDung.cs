using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class NguoiDung
    {
        public Guid IdNguoiDung { get; set; }
        public Guid IdChucVu { get; set; }
        public string MaUser { get; set; }
        public string TenNguoiDung { get; set; }
        public string SDT {get; set; }
        public int GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Email { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau {get; set; }
        public string AnhDaiDien { get; set; }
        public int TrangThai { get; set; }
        public virtual ICollection<ChucVu> ChucVus { get; set; }
        public virtual IEnumerable<ThongTinGiaoHang> ThongTinGiaoHangs { get; set; }
    }
}
