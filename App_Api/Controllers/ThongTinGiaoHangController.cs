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
    public class ThongTinGiaoHangController : ControllerBase
    {
        private readonly IAllRepo<ThongTinGiaoHang> allRepo;
        BazaizaiContext dbContext = new BazaizaiContext();
        DbSet<ThongTinGiaoHang> thongTinGiaoHang;
        public ThongTinGiaoHangController()
        {
            thongTinGiaoHang = dbContext.thongTinGiaoHangs;
            AllRepo<ThongTinGiaoHang> all = new AllRepo<ThongTinGiaoHang>(dbContext, thongTinGiaoHang);
            allRepo = all;
        }
        // GET: api/<ThongTinGiaoHangController>
        [HttpGet("GetAll")]
        public IEnumerable<ThongTinGiaoHang> GetAll()
        {
            return allRepo.GetAll();
        }

        // GET api/<ThongTinGiaoHangController>/5
        [HttpGet("GetByIdUser")]
        public IEnumerable<ThongTinGiaoHang> GetByIdUser(string idNguoiDung)
        {
            return allRepo.GetAll().Where(c => c.IdNguoiDung == idNguoiDung);
        }

        // POST api/<ThongTinGiaoHangController>
        [HttpPost("Create")]
        public bool Create(string idNguoiDung, string TenNguoiNhan, string SDT, string DiaChi)
        {
            var thongTinGiaoHang = new ThongTinGiaoHang()
            {
                IdThongTinGH = Guid.NewGuid().ToString(),
                IdNguoiDung = idNguoiDung,
                TenNguoiNhan = TenNguoiNhan,
                SDT = SDT,
                DiaChi = DiaChi,
                TrangThai = 0
            };
            return allRepo.AddItem(thongTinGiaoHang);

        }

        // PUT api/<ThongTinGiaoHangController>/5
        [HttpPut("Edit")]
        public bool Edit(string idThongTinGH, string idNguoiDung, string TenNguoiNhan, string SĐT, string DiaChi, int TrangThai)
        {
            var ttgh = allRepo.GetAll().FirstOrDefault(c => c.IdThongTinGH == idThongTinGH);
            ttgh.IdNguoiDung = idNguoiDung;
            ttgh.TenNguoiNhan = TenNguoiNhan;
            ttgh.SDT = SĐT;
            ttgh.DiaChi = DiaChi;
            ttgh.TrangThai = TrangThai;
            return allRepo.EditItem(ttgh);
        }

        // DELETE api/<ThongTinGiaoHangController>/5
        [HttpDelete("Delete")]
        public bool Delete(string id)
        {
            var ttgh = allRepo.GetAll().FirstOrDefault(c => c.IdThongTinGH == id);
            return allRepo.RemoveItem(ttgh);
        }
    }
}
