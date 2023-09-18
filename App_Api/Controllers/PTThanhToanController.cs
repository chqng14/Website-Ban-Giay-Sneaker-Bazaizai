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
    public class PTThanhToanController : ControllerBase
    {
        // GET: api/<PTThanhToanController>
        private readonly IAllRepo<PhuongThucThanhToan> allRepo;
        BazaizaiContext dbContext = new BazaizaiContext();
        DbSet<PhuongThucThanhToan> PhuongThucThanhToan;
        public PTThanhToanController()
        {
            PhuongThucThanhToan = dbContext.PhuongThucThanhToans;
            AllRepo<PhuongThucThanhToan> all = new AllRepo<PhuongThucThanhToan>(dbContext, PhuongThucThanhToan);
            allRepo = all;
        }
        // GET: api/<PhuongThucThanhToanController>
        [HttpGet]
        public IEnumerable<PhuongThucThanhToan> ShowAllKieuGiay()
        {
            return allRepo.GetAll();
        }

        // GET api/<PhuongThucThanhToanController>/5
        [HttpGet("TimPhuongThucThanhToan={id}")]
        public PhuongThucThanhToan GetPhuongThucThanhToanByID(Guid id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IDPhuongThucThanhToan == id);
        }

        // POST api/<PhuongThucThanhToanController>
        [HttpPost]
        public bool AddPhuongThucThanhToan(string ten, string mota, int trangthai)
        {
            string ma;
            if (allRepo.GetAll().Count() == null)
            {
                ma = "PT1";
            }
            else
            {
                ma = "PT" + (allRepo.GetAll().Count() + 1);
            }
            var PhuongThucThanhToan = new PhuongThucThanhToan()
            {
                IDPhuongThucThanhToan = Guid.NewGuid(),
                MaPhuongThucThanhToan = ma,
                TenPhuongThucThanhToan = ten,
                MoTa = mota,
                TrangThai = trangthai
            };
            return allRepo.AddItem(PhuongThucThanhToan);
        }

        [HttpPut("SuaPhuongThucThanhToan={id}")]
        public bool SuaPhuongThucThanhToan(Guid id, string ma, string ten, string mota, int trangthai)
        {
            var PhuongThucThanhToan = new PhuongThucThanhToan()
            {
                IDPhuongThucThanhToan = id,
                MaPhuongThucThanhToan = ma,
                TenPhuongThucThanhToan = ten,
                MoTa = mota,
                TrangThai = trangthai
            };
            return allRepo.EditItem(PhuongThucThanhToan);
        }

        // DELETE api/<PhuongThucThanhToanController>/5
        [HttpDelete("XoaPhuongThucThanhToan={id}")]
        public bool XoaPhuongThucThanhToan(Guid id)
        {
            var PhuongThucThanhToan = allRepo.GetAll().FirstOrDefault(c => c.IDPhuongThucThanhToan == id);
            return allRepo.RemoveItem(PhuongThucThanhToan);
        }
    }
}
