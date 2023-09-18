using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class KhuyenMai
    {
        public string IDKhuyenMai { get; set; }
        public string MaKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string LoaiHinhKM { get; set; }
        public decimal MucGiam { get; set; }
        public string PhamVi { get; set; }
        public int TrangThai { get; set; }
        public virtual ICollection<KhuyenMaiChiTiet> KhuyenMaiChiTiet { get; set; }

    }
}
