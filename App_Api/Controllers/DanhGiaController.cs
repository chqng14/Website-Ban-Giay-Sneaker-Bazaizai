using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.DanhGia;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using static App_Data.Repositories.TrangThai;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhGiaController : ControllerBase
    {
        private readonly IDanhGiaRepo _danhGiaRepo;

        public DanhGiaController(IDanhGiaRepo danhGiaRepo, IMapper mapper)
        {
            _danhGiaRepo = danhGiaRepo;
        }


        [HttpGet("GetDanhGiaById/{id}")]
        public async Task<DanhGia?> GetDanhGia(string id)
        {
            var danhGia = await _danhGiaRepo.GetAllAsync();
            return danhGia.FirstOrDefault(x => x.IdDanhGia == id);
        }

        [HttpGet("GetDanhGiaHienThi")]
        public async Task<List<DanhGia>> GetLstDanhGiaDaDuyet()
        {
            var lstDangGia = await _danhGiaRepo.GetAllAsync();
            return lstDangGia.Where(x => x.TrangThai == (int?)TrangThaiDanhGia.DaDuyet).ToList();
        }



        [HttpGet("GetAllDanhGia")]
        public async Task<List<DanhGia>> GetAllDanhGia()
        {
            return await _danhGiaRepo.GetAllAsync();
        }

        //[HttpGet("GetDanhGiaModelView")]
        //public async Task<List<DanhGiaViewModel>> GetLstDanhGiaViewModel(string productId, string parentId)
        //{

        //    return await _danhGiaRepo.GetListAsyncViewModel(productId, parentId);
        //}

        [HttpPost("AddDanhGia")]
        public async Task<bool> Create(string BinhLuan, string ParentId,
        int SaoSp, int SaoVanChuyen, string IdNguoiDung, string IdSanPhamChiTiet)
        {
            var danhGia = new DanhGia();
            danhGia.IdDanhGia = Guid.NewGuid().ToString();
            danhGia.TrangThai = (int)TrangThaiDanhGia.ChuaDuyet;
            danhGia.IdNguoiDung = IdNguoiDung;
            danhGia.BinhLuan = BinhLuan;
            danhGia.IdSanPhamChiTiet = IdSanPhamChiTiet;
            danhGia.NgayDanhGia = DateTime.Now;
            danhGia.ParentId = ParentId;
            danhGia.SaoSp = SaoSp;
            danhGia.SaoVanChuyen = SaoVanChuyen;
            return await _danhGiaRepo.AddAsync(danhGia);
        }


        [HttpDelete("XoaDanhGia/{id}")]
        public async Task<bool> Delete(string id)
        {
            return await _danhGiaRepo.DeleteAsync(id);
        }

        [HttpPut("ChinhSuaDanhGia")]
        public async Task<bool> Update(string IdDanhGia, string BinhLuan, string ParentId,
        int SaoSp, int SaoVanChuyen, string IdNguoiDung, string IdSanPhamChiTiet)
        {
            var dg = await GetDanhGia(IdDanhGia);
            //if (dg != null && dg.SuaDoi > 0)
            //{
                dg.IdSanPhamChiTiet = IdSanPhamChiTiet;
                dg.IdNguoiDung = IdNguoiDung;
                //dg.SuaDoi = 0;
                dg.TrangThai = (int)TrangThaiDanhGia.ChuaDuyet;
                dg.BinhLuan = BinhLuan;
                dg.SaoSp = SaoSp;
                dg.SaoVanChuyen = SaoVanChuyen;
                dg.ParentId = ParentId;
                return await _danhGiaRepo.UpdateAsync(dg);
            //}
            //else return false;
        }

        [HttpPut("DuyetDanhGia")]
        public async Task<bool> Update(string IdDanhGia)
        {
            var dg = await GetDanhGia(IdDanhGia);
            if (dg != null)
            {
                dg.TrangThai = (int)TrangThaiDanhGia.DaDuyet;
                return await _danhGiaRepo.UpdateAsync(dg);
            }
            else return false;
        }

        [HttpPut("AddLike")]
        public async Task<bool> AddLike(string IdDanhGia)
        {
            var dg = await GetDanhGia(IdDanhGia);
            //if (dg != null && dg.SuaDoi > 0)
            //{
            //    dg.LuotYeuThich = dg.LuotYeuThich + 1;
                return await _danhGiaRepo.UpdateAsync(dg);
            //}
            //else return false;
        }
        [HttpPut("BoLike")]
        public async Task<bool> BoLike(string IdDanhGia)
        {
            var dg = await GetDanhGia(IdDanhGia);
            //if (dg != null && dg.SuaDoi > 0)
            //{
            //    dg.LuotYeuThich = dg.LuotYeuThich - 1;
                return await _danhGiaRepo.UpdateAsync(dg);
            //}
            //else return false;
        }
    }
}
