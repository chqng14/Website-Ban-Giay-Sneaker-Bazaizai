using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.SanPhamChiTietViewModel
{
    public class DanhSachGiayViewModel
    {
        public List<ItemViewModel>? LstSanPhamMoi { get; set; }
        public List<ItemViewModel>? LstBanChay { get; set; }
        public List<ItemViewModel>? LstTopDanhGia { get; set; }
        public List<ItemViewModel>? LstSanPhamNoiBat { get; set; }
    }
}
