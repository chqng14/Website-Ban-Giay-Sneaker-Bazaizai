using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonChiTietController : ControllerBase
    {
        private readonly IAllRepo<HoaDonChiTiet> allRepo;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        DbSet<HoaDonChiTiet> hoaDonChiTiets;
        public HoaDonChiTietController()
        {
            hoaDonChiTiets = DbContextModel.hoaDonChiTiets;
            allRepo = new AllRepo<HoaDonChiTiet>(DbContextModel, hoaDonChiTiets);
        }
        // GET: api/<HoaDonChiTietController>
        [HttpGet]
        public IEnumerable<HoaDonChiTiet> Get()
        {
            return allRepo.GetAll();
        }

        // GET api/<HoaDonChiTietController>/5
        [HttpGet("GetHoaDonCTById")]
        public HoaDonChiTiet GetHoaDonCTById(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdHoaDonChiTiet == id);
        }

        // POST api/<HoaDonChiTietController>
        [HttpPost("Create")]
        public bool Post(string IdHoaDon, string IdSanPhamChiTiet, int SoLuong, double GiaGoc, double GiaBan, int TrangThai)
        {
            HoaDonChiTiet hdct = new HoaDonChiTiet()
            {
                IdHoaDonChiTiet = Guid.NewGuid().ToString(),
                IdHoaDon = IdHoaDon,
                IdSanPhamChiTiet = IdSanPhamChiTiet,
                SoLuong = SoLuong,
                GiaGoc = GiaGoc,
                GiaBan = GiaBan,
                TrangThai = TrangThai,
            };
            return allRepo.AddItem(hdct);
        }

        // PUT api/<HoaDonChiTietController>/5
        [HttpPut("Edit")]
        public bool Put(string IdHoaDonChiTiet, string IdHoaDon, string IdSanPhamChiTiet, int SoLuong, double GiaGoc, double GiaBan, int TrangThai)
        {
            var hdct = allRepo.GetAll().First(p => p.IdHoaDonChiTiet == IdHoaDonChiTiet);
            hdct.IdHoaDon = IdHoaDon;
            hdct.IdSanPhamChiTiet = IdSanPhamChiTiet;
            hdct.SoLuong = SoLuong;
            hdct.GiaGoc = GiaGoc;
            hdct.GiaBan = GiaBan;
            hdct.TrangThai = TrangThai;
            return allRepo.EditItem(hdct);
        }

        // DELETE api/<HoaDonChiTietController>/5
        [HttpDelete("Delete")]
        public bool Delete(string id)
        {
            var hdct = allRepo.GetAll().First(p => p.IdHoaDonChiTiet == id);
            return allRepo.RemoveItem(hdct);
        }
    }
}
