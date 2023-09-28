using App_Data.ViewModels.GioHang;
using App_Data.ViewModels.SanPhamChiTiet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.GioHangChiTiet
{
    public class GioHangChiTietDTOCUD
    {
        public SanPhamChiTietDTO sanPhamChiTietDTO { get; set; }
        public GioHangDTO GioHangDTO { get; set; }
        //public string IdGioHangChiTiet { get; set; }
        //public string IdNguoiDung { get; set; }
        //public string IdSanPhamCT { get; set; }
        public int? SoLuong { get; set; }
        public double? GiaGoc { get; set; }
    }
}
