using App_Data.DbContext;
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
        public IEnumerable<PhuongThucThanhToan> ShowAll()
        {
            return allRepo.GetAll();
        }

        // GET api/<PhuongThucThanhToanController>/5
        [HttpGet("PhuongThucThanhToan={id}")]
        public PhuongThucThanhToan GetPhuongThucThanhToanByID(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdPhuongThucThanhToan == id);
        }
        [HttpGet("PhuongThucThanhToanByName")]
        public string GetPhuongThucThanhToanByName(string ten)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.TenPhuongThucThanhToan.ToUpper() == ten.ToUpper()).IdPhuongThucThanhToan;
        }
        // POST api/<PhuongThucThanhToanController>
        [HttpPost]
        public bool AddPhuongThucThanhToan(string ten, string mota, int trangthai)
        {
            var pttts = allRepo.GetAll();
            if (pttts.Where(c => c.TenPhuongThucThanhToan == ten).Any())
            {
                return false;
            }
            string ma;
            if (pttts.Count() == null)
            {
                ma = "PT1";
            }
            else
            {
                ma = "PT" + (allRepo.GetAll().Count() + 1);
            }
            var PhuongThucThanhToan = new PhuongThucThanhToan()
            {
                IdPhuongThucThanhToan = Guid.NewGuid().ToString(),
                MaPhuongThucThanhToan = ma,
                TenPhuongThucThanhToan = ten,
                MoTa = mota,
                TrangThai = trangthai
            };
            return allRepo.AddItem(PhuongThucThanhToan);
        }

        [HttpPut("SuaPhuongThucThanhToan={id}")]
        public bool SuaPhuongThucThanhToan(string id, string ma, string ten, string mota, int trangthai)
        {
            var PhuongThucThanhToan = allRepo.GetAll().FirstOrDefault(c => c.IdPhuongThucThanhToan == id);
            PhuongThucThanhToan.MaPhuongThucThanhToan = ma;
            PhuongThucThanhToan.TenPhuongThucThanhToan = ten;
            PhuongThucThanhToan.MoTa = mota;
            PhuongThucThanhToan.TrangThai = trangthai;
            return allRepo.EditItem(PhuongThucThanhToan);
        }

        // DELETE api/<PhuongThucThanhToanController>/5
        [HttpDelete("XoaPhuongThucThanhToan={id}")]
        public bool XoaPhuongThucThanhToan(string id)
        {
            var PhuongThucThanhToan = allRepo.GetAll().FirstOrDefault(c => c.IdPhuongThucThanhToan == id);
            return allRepo.RemoveItem(PhuongThucThanhToan);
        }
    }
}
