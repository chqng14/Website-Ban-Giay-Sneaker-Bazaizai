using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.SanPhamChiTietViewModel
{
    public class ItemDetailViewModel : ItemShopViewModel
    {
        public int? SoLuongDaBan { get; set; }
        public int? SoLuotYeuThich { get; set; }
        public List<string>? LstMauSac { get; set; }
        public List<int>? LstKichThuoc { get; set; }
        public List<string>? DanhSachAnh { get; set; }
        public bool IsYeuThich { get; set; }
        public string? XuatXu { get; set; }
        public string? LoaiGiay { get; set; }
        public string? ChatLieu { get; set; }
        public string? KieuDeGiay { get; set; }
        public double? KhoiLuong { get; set; }

    }
}
