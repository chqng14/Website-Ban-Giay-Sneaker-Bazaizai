using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangChiTietController : ControllerBase
    {
        private readonly IAllRepo<GioHangChiTiet> allRepo;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        DbSet<GioHangChiTiet> gioHangChiTiets;
        public GioHangChiTietController()
        {
            gioHangChiTiets = DbContextModel.gioHangChiTiets;
            allRepo = new AllRepo<GioHangChiTiet>(DbContextModel, gioHangChiTiets);
        }
        // GET: api/<GioHangChiTietController>
        [HttpGet]
        public IEnumerable<GioHangChiTiet> Get()
        {
            return allRepo.GetAll();
        }

        // GET api/<GioHangChiTietController>/5
        [HttpGet("GetGioHangCTById")]
        public GioHangChiTiet GetGioHangCTById(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdGioHangChiTiet == id);
        }

        // POST api/<GioHangChiTietController>
        [HttpPost("Create")]
        public bool Post(string IdNguoiDung, string IdSanPhamChiTiet, int SoLuong, double GiaGoc, int TrangThai)
        {
            GioHangChiTiet ghct = new GioHangChiTiet()
            {
                IdGioHangChiTiet = Guid.NewGuid().ToString(),
                IdNguoiDung = IdNguoiDung,
                IdSanPhamCT = IdSanPhamChiTiet,
                Soluong = SoLuong,
                GiaGoc = GiaGoc,
                TrangThai = TrangThai,
            };
            return allRepo.AddItem(ghct);
        }

        // PUT api/<GioHangChiTietController>/5
        [HttpPut("Edit")]
        public bool Put(string IdGioHangChiTiet, string IdNguoiDung, string IdSanPhamChiTiet, int SoLuong, double GiaGoc, int TrangThai)
        {
            var ghct = allRepo.GetAll().FirstOrDefault(c => c.IdGioHangChiTiet == IdGioHangChiTiet);
            ghct.IdNguoiDung = IdNguoiDung;
            ghct.IdSanPhamCT = IdSanPhamChiTiet;
            ghct.Soluong = SoLuong;
            ghct.GiaGoc = GiaGoc;
            ghct.TrangThai = TrangThai;
            return allRepo.EditItem(ghct);
        }

        // DELETE api/<GioHangChiTietController>/5
        [HttpDelete("Delete")]
        public bool Delete(string id)
        {
            var ghct = allRepo.GetAll().FirstOrDefault(c => c.IdGioHangChiTiet == id);
            return allRepo.RemoveItem(ghct);
        }
    }
}
