using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly IAllRepo<SanPham> allRepo;
        BazaizaiContext dbContext = new BazaizaiContext();
        DbSet<SanPham> SanPham;
        public SanPhamController()
        {
            SanPham = dbContext.SanPhams;
            AllRepo<SanPham> all = new AllRepo<SanPham>(dbContext, SanPham);
            allRepo = all;
        }
        // GET: api/<SanPhamController>
        [HttpGet]
        public IEnumerable<SanPham> ShowAllKieuGiay()
        {
            return allRepo.GetAll();
        }

        // GET api/<SanPhamController>/5
        [HttpGet("TimSanPham={id}")]
        public SanPham GetSanPhamByID(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdSanPham == id);
        }

        // POST api/<SanPhamController>
        [HttpPost]
        public bool AddSanPham(string ten, int trangthai)
        {
            string ma;
            if (allRepo.GetAll().Count() == null)
            {
                ma = "SP1";
            }
            else
            {
                ma = "SP" + (allRepo.GetAll().Count() + 1);
            }
            var SanPham = new SanPham()
            {
                IdSanPham = Guid.NewGuid().ToString(),
                MaSanPham = ma,
                TenSanPham = ten,
                Trangthai = trangthai
            };
            return allRepo.AddItem(SanPham);
        }

        [HttpPut("SuaSanPham={id}")]
        public bool SuaSanPham(string id, string ma, string ten, int trangthai)
        {
            var SanPham = new SanPham()
            {
                IdSanPham = id,
                MaSanPham = ma,
                TenSanPham = ten,
                Trangthai = trangthai
            };
            return allRepo.EditItem(SanPham);
        }

        // DELETE api/<SanPhamController>/5
        [HttpDelete("XoaSanPham={id}")]
        public bool XoaSanPham(string id)
        {
            var SanPham = allRepo.GetAll().FirstOrDefault(c => c.IdSanPham == id);
            return allRepo.RemoveItem(SanPham);
        }
    }
}
