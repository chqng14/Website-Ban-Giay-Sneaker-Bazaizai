using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.SanPhamYeuThichDTO
{
    public class SanPhamYeuThichViewModel
    {
        public string? TenSanPham { get; set; }
        public string? LoaiGiay { get; set; }
        public string? IdSanPhamChiTiet { get; set; }
        public string? Anh { get; set; }
        public decimal GiaBan { get; set; }
        public decimal GiaThucTe { get; set; }
        public int TrangThaiSale { get; set; }
    }
}
