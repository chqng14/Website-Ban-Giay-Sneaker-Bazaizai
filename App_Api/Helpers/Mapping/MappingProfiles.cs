using App_Data.Models;
using App_Data.ViewModels.MauSac;
using App_Data.ViewModels.SanPhamChiTiet;
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
        }
    }
}
