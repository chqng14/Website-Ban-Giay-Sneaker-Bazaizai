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

namespace App_Api.Helpers.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<MauSacDTO, MauSac>();
            CreateMap<XuatXuDTO, XuatXu>();
            CreateMap<VoucherDTO, Voucher>().ReverseMap();
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
                );
            CreateMap<GioHangChiTietDTOCUD, GioHangChiTiet>().ReverseMap();

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
                        opt => opt.MapFrom(src => src.Anh.Select(x => x.Url).ToList())
                    );
            CreateMap<SanPhamDTO, SanPham>();
            CreateMap<ThuongHieuDTO, ThuongHieu>();
            CreateMap<ChatLieuDTO, ChatLieu>();
            CreateMap<LoaiGiayDTO, LoaiGiay>();
            CreateMap<KieuDeGiayDTO, KieuDeGiay>();
            CreateMap<KichCoDTO, KichCo>();

        }
    }
}
