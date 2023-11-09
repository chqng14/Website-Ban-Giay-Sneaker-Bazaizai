﻿using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.DanhGia;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using static App_Data.Repositories.TrangThai;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhGiaController : ControllerBase
    {
        private readonly IDanhGiaRepo _danhGiaRepo;
        private readonly IAllRepo<DanhGia> repos;
        BazaizaiContext context = new BazaizaiContext();
        DbSet<DanhGia> danhGias;
        public DanhGiaController()
        {
            _danhGiaRepo= new DanhGiaRepo();
            danhGias = context.danhGias;
            AllRepo<DanhGia> all = new AllRepo<DanhGia>(context, danhGias);
            repos = all;
        }

        [HttpGet("GetDanhGiaViewModel")]
        public async Task<List<DanhGiaViewModel>> GetAllDanhGiaViewModel(string idspchitiet)
        {
            return await _danhGiaRepo.GetListAsyncViewModel(idspchitiet);
        }

        [HttpGet("GetSoSaoTB")]
        public async Task<float> GetSoSaoTB(string idspchitiet)
        {
            return await _danhGiaRepo.SoSaoTB(idspchitiet);
        }
        [HttpGet("GetTongSoDanhGia")]
        public async Task<int> GetTongSoDanhGia(string idspchitiet)
        {
            return await _danhGiaRepo.GetTongSoDanhGia(idspchitiet);
        }

        [HttpPost("AddDanhGia")]
        public bool CreateDanhGia(string BinhLuan, string? ParentId,
       int SaoSp, int SaoVanChuyen, string IdNguoiDung, string IdSanPhamChiTiet)
        {
            DanhGia danhGia = new DanhGia();
            danhGia.IdDanhGia = Guid.NewGuid().ToString();
            danhGia.TrangThai = (int)TrangThaiDanhGia.ChuaDuyet;
            danhGia.IdNguoiDung = IdNguoiDung;
            danhGia.BinhLuan = BinhLuan;
            danhGia.IdSanPhamChiTiet = IdSanPhamChiTiet;
            danhGia.NgayDanhGia = DateTime.Now;
            danhGia.ParentId = ParentId;
            danhGia.SaoSp = SaoSp;
            danhGia.SaoVanChuyen = SaoVanChuyen;
            return repos.AddItem(danhGia);
        }

        [HttpGet("GetDanhGiaById/{id}")]
        public DanhGia? GetDanhGiaById(string id)
        {      
            return repos.GetAll().First(p => p.IdDanhGia == id);
        }

        [HttpGet("GetDanhGiaHienThi")]
        public  List<DanhGia> GetLstDanhGiaDaDuyet()
        {
            return repos.GetAll().Where(x => x.TrangThai == (int?)TrangThaiDanhGia.DaDuyet).ToList();
        }
        [HttpGet("GetDanhGiaChuaDuyet")]
        public List<DanhGia> GetLstDanhGiaChuaDuyet()
        {
            return repos.GetAll().Where(x => x.TrangThai == (int?)TrangThaiDanhGia.ChuaDuyet).ToList();
        }



        [HttpGet("GetAllDanhGia")]
        public IEnumerable<DanhGia> GetAllDanhGia()
        {
            return repos.GetAll();
        }

        //[HttpGet("GetDanhGiaModelView")]
        //public async Task<List<DanhGiaViewModel>> GetLstDanhGiaViewModel(string productId, string parentId)
        //{

        //    return await _danhGiaRepo.GetListAsyncViewModel(productId, parentId);
        //}




        [HttpDelete("XoaDanhGia/{id}")]
        public bool Delete(string id)
        {
            var danhGia = repos.GetAll().First(p => p.IdDanhGia == id);
            return repos.RemoveItem(danhGia);
        }

        [HttpPut("ChinhSuaDanhGia")]
        public bool UpdateDanhGia(string IdDanhGia, string BinhLuan,
        int SaoSp, int SaoVanChuyen, string IdNguoiDung, string IdSanPhamChiTiet)
        {
            var dg = GetDanhGiaById(IdDanhGia);
            if (dg != null && dg.SuaDoi > 0)
            {
                dg.IdSanPhamChiTiet = IdSanPhamChiTiet;
                dg.IdNguoiDung = IdNguoiDung;
                dg.SuaDoi = 0;
                dg.TrangThai = (int)TrangThaiDanhGia.ChuaDuyet;
                dg.BinhLuan = BinhLuan;
                dg.SaoSp = SaoSp;
                dg.SaoVanChuyen = SaoVanChuyen;
                return repos.EditItem(dg);
            }
            else return false;
        }

        [HttpPut("DuyetDanhGia")]
        public bool DuyetDanhGia(string IdDanhGia)
        {
            var dg =  GetDanhGiaById(IdDanhGia);
            if (dg != null)
            {
                dg.TrangThai = (int)TrangThaiDanhGia.DaDuyet;
                return repos.EditItem(dg);
            }
            else return false;
        }

        //[HttpPut("AddLike")]
        //public async Task<bool> AddLike(string IdDanhGia)
        //{
        //    var dg = await GetDanhGia(IdDanhGia);
        //    if (dg != null && dg.SuaDoi > 0)
        //    {
        //        dg.LuotYeuThich = dg.LuotYeuThich + 1;
        //        return await _danhGiaRepo.UpdateAsync(dg);
        //    }
        //    else return false;
        //}
        //[HttpPut("BoLike")]
        //public async Task<bool> BoLike(string IdDanhGia)
        //{
        //    var dg = await GetDanhGia(IdDanhGia);
        //    if (dg != null && dg.SuaDoi > 0)
        //    {
        //        dg.LuotYeuThich = dg.LuotYeuThich - 1;
        //        return await _danhGiaRepo.UpdateAsync(dg);
        //    }
        //    else return false;
        //}
    }
}
