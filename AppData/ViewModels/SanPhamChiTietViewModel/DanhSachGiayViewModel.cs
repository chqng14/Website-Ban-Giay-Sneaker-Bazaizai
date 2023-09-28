using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.SanPhamChiTietViewModel
{
    public class DanhSachGiayViewModel
    {
        public List<ItemViewModel>? SanPhamMoi { get; set; }
        public List<ItemViewModel>? BanChay { get; set; }
        public List<ItemViewModel>? TopDanhGia { get; set; }
        public List<ItemViewModel>? SanPhamNoiBat { get; set; }
    }
}
