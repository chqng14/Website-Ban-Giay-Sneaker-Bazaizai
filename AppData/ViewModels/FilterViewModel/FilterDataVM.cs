using App_Data.ViewModels.SanPhamChiTietViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.FilterViewModel
{
    public class FilterDataVM
    {
        public List<ItemShopViewModel>? Items { get; set; } = new List<ItemShopViewModel>();
        public PagingInfo? PagingInfo { get; set; } = new PagingInfo();
    }
}
