using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.SanPhamChiTietViewModel
{
    public class DanhSachGiayViewModel
    {
        public List<ItemShopViewModel>? LstSanPhamMoi { get; set; } = new List<ItemShopViewModel> { };
        public List<ItemShopViewModel>? LstBanChay { get; set; } = new List<ItemShopViewModel> { };
        public List<ItemShopViewModel>? LstTopDanhGia { get; set; } = new List<ItemShopViewModel> { };
        public List<ItemShopViewModel>? LstSanPhamNoiBat { get; set; } = new List<ItemShopViewModel> { };
        public List<ItemShopViewModel>? LstAllSanPham { get; set; } = new List<ItemShopViewModel> { };
    }
}
