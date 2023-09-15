using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class ChucVu
    {
        public Guid IdChucVu { get; set; }
        public string MaChucVu { get; set; }
        public string TenChucVu { get; set; }
        public int TrangThai { get; set; }
        public virtual IEnumerable<NguoiDung> NguoiDungs { get; set; }
    }
}
