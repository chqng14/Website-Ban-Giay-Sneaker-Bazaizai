using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.GioHangChiTiet
{
    public class GioHangMiniViewModel
    {
        public List<SanPhamGioHangViewModel>? SanPhamGioHangViewModels { get; set; } = new List<SanPhamGioHangViewModel> { };
        public double TongTien => SanPhamGioHangViewModels!.Sum(x => x.GiaSanPham * x.SoLuong);
        public int SoLuongSanPham => SanPhamGioHangViewModels!.Count();
    }
}
