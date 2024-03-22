using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangController : ControllerBase
    {
        private readonly IAllRepo<GioHang> allRepo;
        BazaizaiContext dbContext = new BazaizaiContext();
        DbSet<GioHang> GioHang;
        public GioHangController()
        {
            GioHang = dbContext.GioHangs;
            AllRepo<GioHang> all = new AllRepo<GioHang>(dbContext, GioHang);
            allRepo = all;
        }
        // GET: api/<GioHangController>
        [HttpGet]
        public IEnumerable<GioHang> GetAllGioHang()
        {
            return allRepo.GetAll();
        }

        // GET api/<GioHangController>/5
        [HttpGet("{id}")]
        public GioHang GetGioHangById(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdNguoiDung == id);
        }

        // POST api/<GioHangController>
        [HttpPost]
        public bool AddGioHang(string id, int trangthai)
        {
            var GioHang = new GioHang()
            {
                IdNguoiDung = id,
                NgayTao = DateTime.Now,
                TrangThai = trangthai,
            };
            return allRepo.AddItem(GioHang);
        }

        // PUT api/<GioHangController>/5
        [HttpPut("SuaGioHang {id}")]
        public bool EditGioHang(int trangthai)
        {
            var GioHang = new GioHang()
            {
                TrangThai = trangthai
            };
            return allRepo.EditItem(GioHang);
        }
        // DELETE api/<GioHangController>/5
        [HttpDelete("XoaGioHang{id}")]
        public bool DeleteGioHang(string id)
        {
            var GioHang = allRepo.GetAll().FirstOrDefault(c => c.IdNguoiDung == id);
            return allRepo.RemoveItem(GioHang);
        }
    }
}
