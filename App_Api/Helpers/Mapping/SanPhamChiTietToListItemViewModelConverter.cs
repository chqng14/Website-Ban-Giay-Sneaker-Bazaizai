using App_Data.DbContextt;
using App_Data.Models;
using App_Data.ViewModels.DanhGia;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using static App_Data.Repositories.TrangThai;

namespace App_Api.Helpers.Mapping
{
    public class SanPhamChiTietToListItemViewModelConverter : ITypeConverter<List<SanPhamChiTiet>, DanhSachGiayViewModel>
    {
        private readonly BazaizaiContext _context;
        public SanPhamChiTietToListItemViewModelConverter()
        {
            _context = new BazaizaiContext();
        }
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
            var lstTopDg = source
      .Where(x => x.TrangThai == 0 && x.SoLuongDaBan > 0)
      .GroupBy(x => x.IdSanPham)
      .Select(group => group.First())
      .OrderByDescending(x => x.SoLuongDaBan)
      .Take(10)
      .ToList();



            var danhSachGiay = new DanhSachGiayViewModel
            {
                LstSanPhamMoi = MapToItemViewModelList(source, x => x.TrangThai == 0 && (DateTime.Now - x.NgayTao) < TimeSpan.FromDays(7)),
                LstBanChay = MapToItemViewModelList(sortedSanPhamChiTiet, _ => true),
                LstSanPhamNoiBat = MapToItemViewModelList(source, x => x.TrangThai == 0 && x.NoiBat == true),
                LstTopDanhGia = MapToItemViewModelList(lstTopDg, _ => true)
            };

            return danhSachGiay;
        }

        private List<ItemViewModel> MapToItemViewModelList(List<SanPhamChiTiet> source, Func<SanPhamChiTiet, bool> filter)
        {
            return source
                .Where(filter)
                .OrderByDescending(sp => sp.NgayTao)
                .Take(20)
                .Select(item => new ItemViewModel
                {
                    GiaGoc = item.GiaBan,
                    GiaKhuyenMai = item.GiaThucTe,
                    IdChiTietSp = item?.IdChiTietSp,
                    KhuyenMai = item?.TrangThaiSale == 2 ? true : false,
                    TenSanPham = item?.ThuongHieu?.TenThuongHieu + " " + item?.SanPham?.TenSanPham + "-" + item?.KichCo?.SoKichCo,
                    ThuongHieu = item?.ThuongHieu?.TenThuongHieu,
                    Anh = item?.Anh.OrderBy(a => a.NgayTao)?.FirstOrDefault()?.Url,
                    SoLanDanhGia = GetTongSoDanhGia(item.IdSanPham),
                    SoSao = SoSaoTB(item.IdSanPham)
                })
                .ToList();
        }
        public int GetTongSoDanhGia(string IDsanPham)
        {

            return GetListDg(IDsanPham).Count();
        }
        public List<DanhGia> GetListDg(string IDsanPham)
        {

            var lstDanhGia = _context.sanPhamChiTiets
    .Where(spct => spct.IdSanPham == IDsanPham && spct.DanhGias.Any())
    .SelectMany(spct => spct.DanhGias.Where(danhGia => danhGia.TrangThai == 0 && danhGia.SaoSp != null))
    .ToList();
            return lstDanhGia;
        }
        public double SoSaoTB(string IDsanPham)
        {

            var danhGias = GetListDg(IDsanPham);
            var SoSao = danhGias.Count();
            int? Tong = 0;
            if (SoSao == 0)
            {
                return 0;

            }
            else
            {
                foreach (var item in danhGias)
                {
                    Tong += item.SaoSp;
                }
                double a = 0;
                a = ((double)Tong / SoSao);
                a = (double)Math.Round(a, 1);
                return a;
            }

        }
    }
}
