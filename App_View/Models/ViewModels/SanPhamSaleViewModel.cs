using App_Data.ViewModels.SanPhamChiTietViewModel;

namespace App_View.Models.ViewModels
{
    public class SanPhamSaleViewModel
    {
        public SanPhamDanhSachViewModel? SanPhamDanhSachView { get; set; } = new SanPhamDanhSachViewModel();
        public int TrangThaiSale {get; set; }
    }
}
