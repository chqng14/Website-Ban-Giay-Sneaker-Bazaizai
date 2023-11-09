﻿using App_Data.Models;
using App_Data.ViewModels.DanhGia;
using App_Data.ViewModels.HoaDonChiTietDTO;
using App_Data.ViewModels.Voucher;

namespace App_View.IServices
{
    public interface IDanhGiaService
    {
        public Task<List<DanhGia>> GetAllDanhGia();
        public Task<bool> CreateDanhGia(DanhGia DanhGia);
        public Task<bool> UpdateDanhGia(DanhGia DanhGia);
        public Task<bool> DeleteDanhGia(string IdDanhGia);

        public Task<DanhGia?> GetDanhGiaById(string IdDanhGia);
        public Task<List<DanhGiaViewModel>> GetListAsyncViewModel(string IdProductChiTiet);

    }
}
