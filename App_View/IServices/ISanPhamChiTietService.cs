﻿using App_Data.Models;
using App_Data.ViewModels.Anh;
using App_Data.ViewModels.ChatLieuDTO;
using App_Data.ViewModels.KichCoDTO;
using App_Data.ViewModels.KieuDeGiayDTO;
using App_Data.ViewModels.LoaiGiayDTO;
using App_Data.ViewModels.MauSac;
using App_Data.ViewModels.SanPhamChiTiet.SanPhamDTO;
using App_Data.ViewModels.SanPhamChiTiet.ThuongHieuDTO;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_Data.ViewModels.SanPhamChiTietViewModel;
using App_Data.ViewModels.XuatXu;

namespace App_View.IServices
{
 
    public interface ISanPhamChiTietService
    {
        Task<ResponseCreataDTO> AddAysnc(SanPhamChiTietDTO sanPhamChiTietDTO);
        Task<bool> DeleteAysnc(string id);
        Task<bool> UpdateAynsc(SanPhamChiTietDTO sanPhamChiTietDTO);
        Task<SanPhamChiTiet?> GetByKeyAsync(string id);
        Task<List<SanPhamChiTiet>> GetListSanPhamChiTietAsync();
        Task<List<SanPhamChiTietViewModel>> GetListSanPhamChiTietViewModelAsync();
        Task<ResponseCheckAddOrUpdate> CheckSanPhamAddOrUpdate(SanPhamChiTietDTO sanPhamChiTietDTO);
        Task CreateAnhAysnc(string IdChiTietSp, List<IFormFile> lstIFormFile);
        Task DeleteAnhAysnc(ImageDTO responseImageDeleteVM);
        Task<SanPhamDTO?> CreateTenSanPhamAynsc(SanPhamDTO sanPhamDTO);
        Task<ThuongHieuDTO?> CreateTenThuongHieuAynsc(ThuongHieuDTO thuongHieuDTO);
        Task<XuatXuDTO?> CreateTenXuatXuAynsc(XuatXuDTO xuatXuDTO);
        Task<ChatLieuDTO?> CreateTenChatLieuAynsc(ChatLieuDTO xuatXuDTO);
        Task<LoaiGiayDTO?> CreateTenLoaiGiayAynsc(LoaiGiayDTO loaiGiayDTO);
        Task<KieuDeGiayDTO?> CreateTenKieuDeGiayAynsc(KieuDeGiayDTO kieuDeGiayDTO);
        Task<MauSacDTO?> CreateTenMauSacAynsc(MauSacDTO mauSacDTO);
        Task<KichCoDTO?> CreateTenKichCoAynsc(KichCoDTO kichCoDTO);
    }
}
