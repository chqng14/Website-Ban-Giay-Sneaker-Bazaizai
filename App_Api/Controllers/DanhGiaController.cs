using App_Data.DbContext;
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
        DbSet<DanhGia> DanhGias;
        public DanhGiaController()
        {
            _danhGiaRepo = new DanhGiaRepo();
            DanhGias = context.DanhGias;
            AllRepo<DanhGia> all = new AllRepo<DanhGia>(context, DanhGias);
            repos = all;
        }
        [HttpGet("GetListAsyncViewModel")]
        public async Task<List<DanhGiaViewModel>> GetListAsyncViewModel(string idspchitiet)
        {
            return await _danhGiaRepo.GetListAsyncViewModel(idspchitiet);
        }

        [HttpGet("GetTongSoDanhGiaCuaMoiSpChuaDuyet")]
        public async Task<List<DanhGiaResult>> TongSoDanhGiaCuaMoiSpChuaDuyet()
        {
            var result = await _danhGiaRepo.TongSoDanhGiaCuaMoiSpChuaDuyet();

            // Chuyển đổi từ Tuple sang DanhGiaResult
            var danhGiaResults = result.Select(tuple => new DanhGiaResult
            {
                SanPham = tuple.Item1,
                SoLuongDanhGiaChuaDuyet = tuple.Item2,
                IdSanPham = tuple.Item3,
                IdChiTietSp = tuple.Item4,
            }).ToList();
            return danhGiaResults;
        }

        //public async Task<List<Tuple<string, int>>> TongSoDanhGiaCuaMoiSpChuaDuyet()
        //{
        //    return await _danhGiaRepo.TongSoDanhGiaCuaMoiSpChuaDuyet();
        //}

        [HttpGet("GetLstChiTietDanhGiaCuaMoiSpChuaDuyet")]
        public async Task<List<DanhGiaViewModel>> LstChiTietDanhGiaCuaMoiSpChuaDuyet(string idSanPham)
        {
            return await _danhGiaRepo.LstChiTietDanhGiaCuaMoiSpChuaDuyet(idSanPham);
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
        public bool CreateDanhGia(string IdDanhGia, string? BinhLuan, string? ParentId,
       int SaoSp, int? SaoVanChuyen, string IdNguoiDung, string IdSanPhamChiTiet, string? MoTa, string? ChatLuongSanPham)
        {
            DanhGia danhGia = new DanhGia();
            danhGia.IdDanhGia = IdDanhGia;
            danhGia.TrangThai = (int)TrangThaiDanhGia.ChuaDuyet;
            danhGia.IdNguoiDung = IdNguoiDung;
            danhGia.BinhLuan = BinhLuan;
            danhGia.IdSanPhamChiTiet = IdSanPhamChiTiet;
            danhGia.NgayDanhGia = DateTime.Now;
            danhGia.ParentId = ParentId;
            danhGia.SaoSp = SaoSp;
            danhGia.SaoVanChuyen = SaoVanChuyen;
            danhGia.MoTa = MoTa;
            danhGia.ChatLuongSanPham = ChatLuongSanPham;
            return repos.AddItem(danhGia);
        }

        [HttpGet("GetDanhGiaById/{id}")]
        public DanhGia? GetDanhGiaById(string id)
        {
            return repos.GetAll().FirstOrDefault(p => p.IdDanhGia == id);
        }

        [HttpGet("GetDanhGiaHienThi")]
        public List<DanhGia> GetLstDanhGiaDaDuyet()
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
            var dg = GetDanhGiaById(IdDanhGia);
            if (dg != null)
            {
                dg.TrangThai = (int)TrangThaiDanhGia.DaDuyet;
                return repos.EditItem(dg);
            }
            else return false;
        }
        [HttpGet("GetDanhGiaViewModelById/{id}")]
        public async Task<DanhGiaViewModel?> GetDanhGiaViewModelById(string id)
        {
            return await _danhGiaRepo.GetViewModelByKeyAsync(id);
        }

        //[HttpGet("GetAllDanhGiaChuaDuyetViewModel")]
        //public async Task<List<DanhGiaViewModel>> GetAllDanhGiaChuaDuyetViewModel()
        //{
        //    var lst = (await _danhGiaRepo.LstDanhGia()).Where(x=> x.TrangThai == (int)TrangThaiDanhGia.ChuaDuyet).ToList();
        //    return lst;
        //}
        [HttpGet("GetAllDanhGiaChuaDuyetByDkViewModel")]
        public async Task<List<DanhGiaViewModel>> GetAllDanhGiaChuaDuyetByDkViewModel(int? Dk)
        {
            
            var lst = (await _danhGiaRepo.LstDanhGia()).Where(x => x.TrangThai == (int)TrangThaiDanhGia.ChuaDuyet).ToList();
            if (Dk <= 5&&Dk>=1)
            {
                lst = lst.Where(x => x.SaoSp == Dk).ToList();
            }
            else if(Dk==6)
            {
                lst=lst.Where(x => x.BinhLuan !=null||x.ChatLuongSanPham!=null||x.MoTa!=null).ToList();
            }
            return lst;
        }
        [HttpGet("GetAllDanhGiaDaDuyetByDkViewModel")]
        public async Task<List<DanhGiaViewModel>> GetAllDanhGiaDaDuyetByDkViewModel(int? Dk)
        {

            var lst = (await _danhGiaRepo.LstDanhGia()).Where(x => x.TrangThai == (int)TrangThaiDanhGia.DaDuyet).ToList();
            if (Dk <= 5 && Dk >= 1)
            {
                lst = lst.Where(x => x.SaoSp == Dk).ToList();
            }
            else if (Dk == 6)
            {
                lst = lst.Where(x => x.BinhLuan != null || x.ChatLuongSanPham != null || x.MoTa != null).ToList();
            }
            return lst;
        }
        [HttpGet("GetAllDanhGiaDaDuyetByNd")]
        public async Task<List<DanhGiaViewModel>> GetAllDanhGiaDaDuyetByNd(string idUser)
        {
            var lst = (await _danhGiaRepo.LstDanhGia()).Where(x => x.TrangThai == (int)TrangThaiDanhGia.DaDuyet&& x.IdNguoiDung==idUser).ToList();
            return lst;
        }

    }
}
