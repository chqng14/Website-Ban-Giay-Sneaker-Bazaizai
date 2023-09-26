using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IAllRepo<KhuyenMai> repos;
        BazaizaiContext context = new BazaizaiContext();
        DbSet<KhuyenMai> khuyenMais;
        public KhuyenMaiController()
        {
            khuyenMais = context.khuyenMais;
            AllRepo<KhuyenMai> all = new AllRepo<KhuyenMai>(context, khuyenMais);
            repos = all;
        }
        [HttpGet]
        public IEnumerable<KhuyenMai> GetAllKhuyenMai()
        {
            return repos.GetAll();
        }

        [HttpPost]

        public bool CreateKhuyenMai( string Ten, DateTime ngayBD, DateTime ngayKT,int trangThai,decimal mucGiam, string PhamVi, string loaiHinh)
        {
            string MaTS;
            if (repos.GetAll().Count() == null)
            {
                MaTS = "KM1";
            }
            else
            {
                MaTS = "KM" + (repos.GetAll().Count() + 1);
            }

            KhuyenMai KhuyenMai = new KhuyenMai();
            KhuyenMai.TenKhuyenMai = Ten;
            KhuyenMai.MaKhuyenMai = MaTS;
            KhuyenMai.IdKhuyenMai = Guid.NewGuid().ToString();
            KhuyenMai.TrangThai = trangThai;
            KhuyenMai.NgayBatDau = ngayBD;
            KhuyenMai.NgayKetThuc =ngayKT;
            KhuyenMai.MucGiam =mucGiam;
            KhuyenMai.PhamVi=PhamVi;
            KhuyenMai.LoaiHinhKM = loaiHinh;
            return repos.AddItem(KhuyenMai);
        }


        [HttpPut("{id}")]
        public bool EditKhuyenMai(string id, string Ten, DateTime ngayBD, DateTime ngayKT, int trangThai, decimal mucGiam, string PhamVi, string loaiHinh)
        {
            var KhuyenMai = repos.GetAll().First(p => p.IdKhuyenMai == id);
            KhuyenMai.TenKhuyenMai = Ten;
            KhuyenMai.TrangThai = trangThai;
            KhuyenMai.NgayBatDau = ngayBD;
            KhuyenMai.NgayKetThuc = ngayKT;
            KhuyenMai.MucGiam = mucGiam;
            KhuyenMai.PhamVi = PhamVi;
            KhuyenMai.LoaiHinhKM = loaiHinh;
            return repos.EditItem(KhuyenMai);
        }

        [HttpDelete("{id}")]
        public bool DeleteKhuyenMai(string id)
        {
            var KhuyenMai = repos.GetAll().First(p => p.IdKhuyenMai == id);
            return repos.RemoveItem(KhuyenMai);
        }
    }
}
