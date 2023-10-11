using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.ChatLieuDTO;
using App_Data.ViewModels.KichCoDTO;
using App_Data.ViewModels.KieuDeGiayDTO;
using App_Data.ViewModels.LoaiGiayDTO;
using App_Data.ViewModels.MauSac;
using App_Data.ViewModels.SanPhamChiTiet.SanPhamDTO;
using App_Data.ViewModels.SanPhamChiTiet.ThuongHieuDTO;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using App_Data.ViewModels.Voucher;
using App_Data.ViewModels.XuatXu;
using AutoMapper;

using App_Data.ViewModels.VoucherNguoiDung;

using App_Data.ViewModels.KhuyenMaiChiTietDTO;

using App_Data.ViewModels.HoaDonChiTietDTO;
using App_Data.ViewModels.ThongTinGHDTO;
using App_Data.ViewModels.HoaDon;

namespace App_Api.Helpers.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<MauSacDTO, MauSac>();
            CreateMap<XuatXuDTO, XuatXu>();
            CreateMap<VoucherDTO, Voucher>().ReverseMap();
            CreateMap<VoucherNguoiDungDTO, VoucherNguoiDung>().ReverseMap();

            CreateMap<SanPhamChiTietDTO, SanPhamChiTiet>().ReverseMap();

            CreateMap<GioHangChiTiet, GioHangChiTietDTO>()
                 .ForMember(
                    dest => dest.TenSanPham,
                    opt => opt.MapFrom(src => src.SanPhamChiTiet.SanPham.TenSanPham)
                ).ForMember(
                    dest => dest.TenMauSac,
                    opt => opt.MapFrom(src => src.SanPhamChiTiet.MauSac.TenMauSac)
                ).ForMember(
                    dest => dest.TenKichCo,
                    opt => opt.MapFrom(src => src.SanPhamChiTiet.KichCo.SoKichCo)
                ).ForMember(
                dest => dest.LinkAnh,
                opt => opt.MapFrom(src => src.SanPhamChiTiet.Anh.Select(x => x.Url).ToList())
                );
            CreateMap<GioHangChiTietDTOCUD, GioHangChiTiet>().ReverseMap();

            CreateMap<SanPhamChiTiet, SanPhamChiTietExcelViewModel>()
                .ForMember(
                        dest => dest.SanPham,
                        opt => opt.MapFrom(src => src.SanPham.TenSanPham)
                )
                .ForMember(
                        dest => dest.XuatXu,
                        opt => opt.MapFrom(src => src.XuatXu.Ten)
                    )
                .ForMember(
                        dest => dest.ThuongHieu,
                        opt => opt.MapFrom(src => src.ThuongHieu.TenThuongHieu)
                    )
                .ForMember(
                        dest => dest.MauSac,
                        opt => opt.MapFrom(src => src.MauSac.TenMauSac)
                    )
                .ForMember(
                        dest => dest.KichCo,
                        opt => opt.MapFrom(src => src.KichCo.SoKichCo)
                    )
                .ForMember(
                        dest => dest.ChatLieu,
                        opt => opt.MapFrom(src => src.ChatLieu.TenChatLieu)
                    )
                .ForMember(
                        dest => dest.KieuDeGiay,
                        opt => opt.MapFrom(src => src.KieuDeGiay.TenKieuDeGiay)
                    )
                .ForMember(
                        dest => dest.LoaiGiay,
                        opt => opt.MapFrom(src => src.LoaiGiay.TenLoaiGiay)
                    );
            CreateMap<HoaDonDTO, HoaDon>().ReverseMap();
            CreateMap<ThongTinGHDTO, ThongTinGiaoHang>().ReverseMap();
            CreateMap<HoaDonChiTietDTO, HoaDonChiTiet>().ReverseMap();
            CreateMap<HoaDonChiTiet, HoaDonChiTietViewModel>()
                .ForMember(
                dest => dest.TenSanPham,
                    opt => opt.MapFrom(src => src.SanPhamChiTiet.SanPham.TenSanPham)
                ).ForMember(
                    dest => dest.TenMauSac,
                    opt => opt.MapFrom(src => src.SanPhamChiTiet.MauSac.TenMauSac)
                ).ForMember(
                    dest => dest.TenKichCo,
                    opt => opt.MapFrom(src => src.SanPhamChiTiet.KichCo.SoKichCo)
                ).ForMember(
                dest => dest.TenNguoiNhan,
                opt => opt.MapFrom(src => src.HoaDon.ThongTinGiaoHang.TenNguoiNhan)
                ).ForMember(
                dest => dest.MaVoucher,
                opt => opt.MapFrom(src => src.HoaDon.Voucher.MaVoucher)
                );


            CreateMap<SanPhamChiTiet, SanPhamChiTietDTO>()
                .ForMember(
                        dest => dest.DanhSachAnh,
                        opt => opt.MapFrom(src => src.Anh.Where(a => a.TrangThai == 0).Select(x => x.Url))
                )
                .ForMember(
                        dest => dest.FullName,
                        opt => opt.MapFrom(src => $"{src.ThuongHieu.TenThuongHieu} {src.SanPham.TenSanPham} {src.MauSac.TenMauSac}-{src.KichCo.SoKichCo}")
                )
                .ReverseMap();

            CreateMap<List<SanPhamChiTiet>, DanhSachGiayViewModel>()
                .ConvertUsing<SanPhamChiTietToListItemViewModelConverter>();



            CreateMap<SanPhamChiTiet, SanPhamChiTietViewModel>()
                .ForMember(
                        dest => dest.SanPham,
                        opt => opt.MapFrom(src => $"{src.ThuongHieu.TenThuongHieu} {src.SanPham.TenSanPham}")
                )
                .ForMember(
                        dest => dest.XuatXu,
                        opt => opt.MapFrom(src => src.XuatXu.Ten)
                    )
                .ForMember(
                        dest => dest.ThuongHieu,
                        opt => opt.MapFrom(src => src.ThuongHieu.TenThuongHieu)
                    )
                .ForMember(
                        dest => dest.MauSac,
                        opt => opt.MapFrom(src => src.MauSac.TenMauSac)
                    )
                .ForMember(
                        dest => dest.KichCo,
                        opt => opt.MapFrom(src => src.KichCo.SoKichCo)
                    )
                .ForMember(
                        dest => dest.ChatLieu,
                        opt => opt.MapFrom(src => src.ChatLieu.TenChatLieu)
                    )
                .ForMember(
                        dest => dest.KieuDeGiay,
                        opt => opt.MapFrom(src => src.KieuDeGiay.TenKieuDeGiay)
                    )
                .ForMember(
                        dest => dest.LoaiGiay,
                        opt => opt.MapFrom(src => src.LoaiGiay.TenLoaiGiay)
                    )
                .ForMember(
                        dest => dest.ListTenAnh,
                        opt => opt.MapFrom(src => src.Anh.Where(an => an.TrangThai == 0).OrderBy(x=>x.Url).Select(x => x.Url).ToList())
                    );

            CreateMap<SanPhamChiTiet, ItemShopViewModel>()
                .ForMember(
                        dest => dest.Anh,
                        opt => opt.MapFrom(src => src.Anh!.Where(a => a.TrangThai == 0).OrderBy(x => x.Url).FirstOrDefault()!.Url)
                    )
                .ForMember(
                        dest => dest.IdChiTietSp,
                        opt => opt.MapFrom(src => src.IdChiTietSp)
                    )
                .ForMember(
                        dest => dest.GiaBan,
                        opt => opt.MapFrom(src => src.GiaBan)
                    )
                .ForMember(
                        dest => dest.GiaKhuyenMai,
                        opt => opt.MapFrom(src => 0)
                    )
                .ForMember(
                        dest => dest.SoSao,
                        opt => opt.MapFrom(src => 5)
                    )
                .ForMember(
                        dest => dest.ThuongHieu,
                        opt => opt.MapFrom(src => src.ThuongHieu.TenThuongHieu)
                    )
                .ForMember(
                        dest => dest.TenSanPham,
                        opt => opt.MapFrom(src => $"{src.ThuongHieu.TenThuongHieu} {src.SanPham.TenSanPham}")
                    )
                .ForMember(
                        dest => dest.MoTaNgan,
                        opt => opt.MapFrom(src => "Sản phẩm chính hãng")
                    )
                .ForMember(
                        dest => dest.SoLanDanhGia,
                        opt => opt.MapFrom(src => 32)
                    )
                ;
            CreateMap<SanPhamChiTiet, ItemDetailViewModel>()
                .ForMember(
                        dest => dest.Anh,
                        opt => opt.MapFrom(src => src.Anh.Where(x => x.TrangThai == 0).OrderBy(a=>a.Url)!.FirstOrDefault()!.Url)
                    )
                .ForMember(
                        dest => dest.MoTaSanPham,
                        opt => opt.MapFrom(src => src.MoTa)
                    )
                .ForMember(
                        dest => dest.IdChiTietSp,
                        opt => opt.MapFrom(src => src.IdChiTietSp)
                    )
                .ForMember(
                        dest => dest.GiaBan,
                        opt => opt.MapFrom(src => src.GiaBan)
                    )
                .ForMember(
                        dest => dest.GiaKhuyenMai,
                        opt => opt.MapFrom(src => 0)
                    )
                .ForMember(
                        dest => dest.SoSao,
                        opt => opt.MapFrom(src => 5)
                    )
                .ForMember(
                        dest => dest.ThuongHieu,
                        opt => opt.MapFrom(src => src.ThuongHieu.TenThuongHieu)
                    )
                .ForMember(
                        dest => dest.TenSanPham,
                        opt => opt.MapFrom(src => $"{src.ThuongHieu.TenThuongHieu} {src.SanPham.TenSanPham}")
                    )
                .ForMember(
                        dest => dest.MoTaNgan,
                        opt => opt.MapFrom(src => "Sản phẩm chính hãng")
                    )
                .ForMember(
                        dest => dest.SoLanDanhGia,
                        opt => opt.MapFrom(src => 32)
                    )
                 .ForMember(
                        dest => dest.DanhSachAnh,
                        opt => opt.MapFrom(src => src.Anh.Where(x => x.TrangThai == 0).OrderBy(x => x.Url).Select(a => a.Url).ToList())
                    )
                 .ForMember(
                        dest => dest.SoLuotYeuThich,
                        opt => opt.MapFrom(src => 100)
                    )
                 .ForMember(
                        dest => dest.MauSac,
                        opt => opt.MapFrom(src => src.MauSac.TenMauSac)
                    )
                 .ForMember(
                        dest => dest.Size,
                        opt => opt.MapFrom(src => src.KichCo.SoKichCo)
                    )
                ;
            CreateMap<SanPhamDTO, SanPham>();
            CreateMap<ThuongHieuDTO, ThuongHieu>();
            CreateMap<ChatLieuDTO, ChatLieu>();
            CreateMap<LoaiGiayDTO, LoaiGiay>();
            CreateMap<KieuDeGiayDTO, KieuDeGiay>();
            CreateMap<KichCoDTO, KichCo>();
            CreateMap<KhuyenMaiChiTiet, KhuyenMaiChiTietDTO>()
                .ForMember(
                        dest => dest.KhuyenMai,
                        opt => opt.MapFrom(src => src.KhuyenMai.TenKhuyenMai)
                )
                .ForMember(
                        dest => dest.SanPham,
                        opt => opt.MapFrom(src => src.SanPhamChiTiet.SanPham.TenSanPham)
                )
                .ReverseMap();
        }
    }
}
