using App_Data.Models;
using App_Data.Models.ViewModels.MauSac;
using App_Data.Models.ViewModels.XuatXu;
using AutoMapper;

namespace App_Api.Helpers.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<MauSacDTO, MauSac>();
            CreateMap<XuatXuDTO, XuatXu>();
        }
    }
}
