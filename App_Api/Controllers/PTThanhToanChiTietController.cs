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
    public class PTThanhToanChiTietController : ControllerBase
    {
        private readonly IAllRepo<PhuongThucThanhToanChiTiet> allRepo;
        BazaizaiContext dbContext = new BazaizaiContext();
        DbSet<PhuongThucThanhToanChiTiet> PhuongThucThanhToanChiTiets;
        public PTThanhToanChiTietController()
        {
            PhuongThucThanhToanChiTiets = dbContext.PhuongThucThanhToanChiTiets;
            AllRepo<PhuongThucThanhToanChiTiet> all = new AllRepo<PhuongThucThanhToanChiTiet>(dbContext, PhuongThucThanhToanChiTiets);
            allRepo = all;
        }
        // GET: api/<PTThanhToanChiTiet>
        [HttpGet]
        public IEnumerable<PhuongThucThanhToanChiTiet> ShowAll()
        {
            return allRepo.GetAll();
        }

        // GET api/<PhuongThucThanhToanController>/5
        [HttpGet("TimPhuongThucThanhToanChiTiet={id}")]
        public PhuongThucThanhToanChiTiet GetPhuongThucThanhToanByID(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdPhuongThucThanhToanChiTiet == id);
        }
        [HttpGet("PhuongThucThanhToanChiTietByIdPTTT")]
        public PhuongThucThanhToanChiTiet PhuongThucThanhToanChiTietByIdPTTT(string idhoadon)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdHoaDon == idhoadon);
        }
        // POST api/<PhuongThucThanhToanController>
        [HttpPost]
        public string AddPhuongThucThanhToanChiTiet(string IdHoaDon, string IdThanhToan, double SoTien)
        {
            string ma;
            if (allRepo.GetAll().Count() == null)
            {
                ma = "PTCT1";
            }
            else
            {
                ma = "PTCT" + (allRepo.GetAll().Count() + 1);
            }
            var PhuongThucThanhToanChiTiet = new PhuongThucThanhToanChiTiet()
            {
                IdPhuongThucThanhToanChiTiet = Guid.NewGuid().ToString(),
                IdHoaDon = IdHoaDon,
                IdThanhToan = IdThanhToan,
                SoTien = SoTien,
            };
            allRepo.AddItem(PhuongThucThanhToanChiTiet);
            return PhuongThucThanhToanChiTiet.IdPhuongThucThanhToanChiTiet;
        }

        [HttpPut("SuaTrangThaiPTThanhToanChiTiet")]
        public bool SuaPhuongThucThanhToanChiTiet(string IdPhuongThucThanhToanChiTiet, int TrangThai)
        {
            var phuongThucThanhToanChiTiet = allRepo.GetAll().FirstOrDefault(c => c.IdPhuongThucThanhToanChiTiet == IdPhuongThucThanhToanChiTiet);
            phuongThucThanhToanChiTiet.TrangThai = TrangThai;
            return allRepo.EditItem(phuongThucThanhToanChiTiet);
        }

        // DELETE api/<PhuongThucThanhToanController>/5
        [HttpDelete("XoaPhuongThucThanhToanChiTiet={id}")]
        public bool XoaPhuongThucThanhToanChiTiet(string id)
        {
            var PhuongThucThanhToanChiTiet = allRepo.GetAll().FirstOrDefault(c => c.IdPhuongThucThanhToanChiTiet == id);
            return allRepo.RemoveItem(PhuongThucThanhToanChiTiet);
        }
        [HttpPost("AddPhuongThucThanhToanChiTietTaiQuay")]
        public bool AddPhuongThucThanhToanChiTietTaiQuay(string IdHoaDon, string IdThanhToan, double SoTien,int TrangThai)
        {
            var PhuongThucThanhToanChiTiet = new PhuongThucThanhToanChiTiet()
            {
                IdPhuongThucThanhToanChiTiet = Guid.NewGuid().ToString(),
                IdHoaDon = IdHoaDon,
                IdThanhToan = IdThanhToan,
                SoTien = SoTien,
                TrangThai = TrangThai
            };
            return allRepo.AddItem(PhuongThucThanhToanChiTiet);
           
        }
        [HttpDelete("XoaPhuongThucThanhToanChiTietBangIdHoaDon")]
        public bool XoaPhuongThucThanhToanChiTietBangIdHoaDon(string id)
        {
            var PhuongThucThanhToanChiTiet = allRepo.GetAll().FirstOrDefault(c => c.IdHoaDon == id);
            return allRepo.RemoveItem(PhuongThucThanhToanChiTiet);
        }
    }
}
