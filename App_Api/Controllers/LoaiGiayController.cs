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
    public class LoaiGiayController : ControllerBase
    {
        private readonly IAllRepo<LoaiGiay> allRepo;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        DbSet<LoaiGiay> loaiGiays;
        public LoaiGiayController()
        {
            loaiGiays = DbContextModel.LoaiGiays;
            allRepo = new AllRepo<LoaiGiay>(DbContextModel, loaiGiays);
        }
        // GET: api/<LoaiGiayController>
        [HttpGet]
        public IEnumerable<LoaiGiay> Get()
        {
            return allRepo.GetAll();
        }

        // GET api/<LoaiGiayController>/5
        [HttpGet("GetLoaiGiayById")]
        public LoaiGiay GetLoaiGiayById(Guid id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdLoaiGiay == id);
        }

        // POST api/<LoaiGiayController>
        [HttpPost("Create")]
        public bool Post(string TenLoaiGiay, int TrangThai)
        {
            string ma;
            if (allRepo.GetAll().Count() == null)
            {
                ma = "HD1";
            }
            else
            {
                ma = "HD" + (allRepo.GetAll().Count() + 1);
            }
            LoaiGiay lg = new LoaiGiay()
            {
                IdLoaiGiay = Guid.NewGuid(),
                MaLoaiGiay = ma,
                TenLoaiGiay = TenLoaiGiay,
                TrangThai = TrangThai,
            };
            return allRepo.AddItem(lg);
        }

        // PUT api/<LoaiGiayController>/5
        [HttpPut("Edit")]
        public bool Put(Guid idLoaiGiay, string TenLoaiGiay, string MaLoaiGiay, int TrangThai)
        {
            var lg = allRepo.GetAll().First(p => p.IdLoaiGiay == idLoaiGiay);
            lg.TenLoaiGiay = TenLoaiGiay;
            lg.MaLoaiGiay = MaLoaiGiay;
            lg.TrangThai = TrangThai;
            return allRepo.EditItem(lg);
        }

        // DELETE api/<LoaiGiayController>/5
        [HttpDelete("Delete")]
        public bool Delete(Guid idLoaiGiay)
        {
            var lg = allRepo.GetAll().First(p => p.IdLoaiGiay == idLoaiGiay);
            return allRepo.RemoveItem(lg);
        }
    }
}
