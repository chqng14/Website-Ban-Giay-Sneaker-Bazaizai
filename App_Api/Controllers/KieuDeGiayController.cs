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
    public class KieuDeGiayController : ControllerBase
    {
        private readonly IAllRepo<KieuDeGiay> allRepo;
        BazaizaiContext dbContext = new BazaizaiContext();
        DbSet<KieuDeGiay> KieuDeGiay;
        public KieuDeGiayController()
        {
            KieuDeGiay = dbContext.kieuDeGiays;
            AllRepo<KieuDeGiay> all = new AllRepo<KieuDeGiay>(dbContext, KieuDeGiay);
            allRepo = all;
        }
        // GET: api/<KieuDeGiayController>
        [HttpGet]
        public IEnumerable<KieuDeGiay> ShowAllKieuGiay()
        {
            return allRepo.GetAll();
        }

        // GET api/<KieuDeGiayController>/5
        [HttpGet("TimKieuDeGiay={id}")]
        public KieuDeGiay GetKieuDeGiayByID(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdKieuDeGiay == id);
        }

        // POST api/<KieuDeGiayController>
        [HttpPost]
        public bool AddKieuDeGiay(string ten, int trangthai)
        {
            string ma;
            if (allRepo.GetAll().Count() == null)
            {
                ma = "KDG1";
            }
            else
            {
                ma = "KDG" + (allRepo.GetAll().Count() + 1);
            }
            var KieuDeGiay = new KieuDeGiay()
            {
                IdKieuDeGiay = Guid.NewGuid().ToString(),
                MaKieuDeGiay = ma,
                TenKieuDeGiay = ten,
                Trangthai = trangthai
            };
            return allRepo.AddItem(KieuDeGiay);
        }

        [HttpPut("SuaKieuDeGiay={id}")]
        public bool SuaKieuDeGiay(string id, string ma, string ten, int trangthai)
        {
            var KieuDeGiay = new KieuDeGiay()
            {
                IdKieuDeGiay = id,
                MaKieuDeGiay = ma,
                TenKieuDeGiay = ten,
                Trangthai = trangthai
            };
            return allRepo.EditItem(KieuDeGiay);
        }

        // DELETE api/<KieuDeGiayController>/5
        [HttpDelete("XoaKieuDeGiay={id}")]
        public bool XoaKieuDeGiay(string id)
        {
            var KieuDeGiay = allRepo.GetAll().FirstOrDefault(c => c.IdKieuDeGiay == id);
            return allRepo.RemoveItem(KieuDeGiay);
        }
    }
}
