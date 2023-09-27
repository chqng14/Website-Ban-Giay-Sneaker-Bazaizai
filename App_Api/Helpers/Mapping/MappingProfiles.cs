using App_Data.Models;
using App_Data.ViewModels.Cart;
using App_Data.ViewModels.MauSac;
using App_Data.ViewModels.SanPhamChiTiet;
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
            CreateMap<SanPhamChiTietDTO, SanPhamChiTiet>().ReverseMap();
            CreateMap<GioHangDTO, GioHangChiTiet>().ReverseMap();
        }
    }
}
