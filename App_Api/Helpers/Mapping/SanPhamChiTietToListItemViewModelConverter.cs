using App_Data.Models;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using AutoMapper;

namespace App_Api.Helpers.Mapping
{
    public class SanPhamChiTietToListItemViewModelConverter : ITypeConverter<List<SanPhamChiTiet>, DanhSachGiayViewModel>
    {
        public DanhSachGiayViewModel Convert(List<SanPhamChiTiet> source, DanhSachGiayViewModel destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            var sortedSanPhamChiTiet = source
                .Where(x => x.TrangThai == 0 && x.SoLuongDaBan > 0)
                .OrderByDescending(x => x.SoLuongDaBan)
                .Take(10)
                .ToList();

            var danhSachGiay = new DanhSachGiayViewModel
            {
                LstSanPhamMoi = MapToItemViewModelList(source, x => x.TrangThai == 0 && (DateTime.Now - x.NgayTao) < TimeSpan.FromDays(7)),
                LstBanChay = MapToItemViewModelList(sortedSanPhamChiTiet, _ => true),

                LstSanPhamNoiBat = MapToItemViewModelList(source, x => x.TrangThai == 0 && x.NoiBat == true),
            };

            return danhSachGiay;
        }

        private List<ItemViewModel> MapToItemViewModelList(List<SanPhamChiTiet> source, Func<SanPhamChiTiet, bool> filter)
        {
            return source
                .Where(filter)
                .Select(item => new ItemViewModel
                {
                    GiaGoc = item.GiaBan,
                    GiaKhuyenMai = 10000,
                    IdChiTietSp = item?.IdChiTietSp,
                    KhuyenMai = false,
                    TenSanPham = item?.ThuongHieu?.TenThuongHieu + " " + item?.SanPham?.TenSanPham,
                    ThuongHieu = item?.ThuongHieu?.TenThuongHieu,
                    Anh = item?.Anh?.FirstOrDefault()?.Url,
                    SoLanDanhGia = 32,
                    SoSao = 4
                })
                .ToList();
        }
    }
}
