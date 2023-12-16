using App_Data.ViewModels.SanPhamChiTietViewModel;

namespace App_View.Models.ViewModels
{
    public class SanPhamSaleViewModel
    {
        public SanPhamDanhSachViewModel? SanPhamDanhSachView { get; set; } = new SanPhamDanhSachViewModel();
        public SanPhamChiTietViewModel? SanPhamChiTietViewModel { get; set; } = new SanPhamChiTietViewModel();
        public int TrangThaiSale {get; set; }
        public double? GiaThucTe { get; set; }
        public int TrangThai { get; set; }
        public string? IdThuongHieu { get; set; }
        public string? IdLoaiGiay { get; set; }
        public string? IdMauSac { get; set; }
    }
}
