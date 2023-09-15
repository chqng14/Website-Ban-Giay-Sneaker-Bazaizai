using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class ThuongHieu
    {
        public Guid IDThuongHieu { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public int TrangThai { get; set; }
        public virtual List<SanPhamChiTiet> SanPhamChiTiets { get; set; }
    }
}
