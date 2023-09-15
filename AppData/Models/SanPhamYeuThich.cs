using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class SanPhamYeuThich
    {
        public Guid IDWhiteList { get; set; }
        public Guid IDUser { get; set; }
        public Guid IDChiTietSp { get; set; }

        public int TrangThai { get; set; }

    }
}
