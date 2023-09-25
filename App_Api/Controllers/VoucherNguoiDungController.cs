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
    public class VoucherNguoiDungController : ControllerBase
    {
        private readonly IAllRepo<VoucherNguoiDung> allRepo;
        BazaizaiContext dbContext = new BazaizaiContext();
        DbSet<VoucherNguoiDung> voucherNguoiDung;
        public VoucherNguoiDungController()
        {
            voucherNguoiDung = dbContext.voucherNguoiDungs;
            AllRepo<VoucherNguoiDung> all = new AllRepo<VoucherNguoiDung>(dbContext, voucherNguoiDung);
            allRepo = all;
        }
        // GET: api/<ChatLieuController>
        [HttpGet]
        public IEnumerable<VoucherNguoiDung> GetAllVouCherNguoiDung(){
            return allRepo.GetAll();
        }

        // GET api/<ChatLieuController>/5
        [HttpGet("{id}")]
        public VoucherNguoiDung GetVoucherNguoiDungById(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdVouCherNguoiDung == id);
        }

        // POST api/<ChatLieuController>
        [HttpPost]
        public bool AddVoucherNguoiDung(string IdNguoiDung,string IdVoucher,int trangthai)
        {

            var voucher = new VoucherNguoiDung()
            {
                IdVouCherNguoiDung = Guid.NewGuid().ToString(),
                IdNguoiDung = IdNguoiDung,
                IdVouCher = IdVoucher,
                TrangThai = trangthai
            };
            return allRepo.AddItem(voucher);
        }

        // PUT api/<ChatLieuController>/5
        [HttpPut("UpdateVoucherNguoiDung{id}")]
        public bool UpdateVoucherNguoiDung(string id, string IdNguoiDung, string IdVoucher, int trangthai)
        {
            var voucher = new VoucherNguoiDung()
            {
                IdVouCherNguoiDung = id,
                IdNguoiDung = IdNguoiDung,
                IdVouCher = IdVoucher,
                TrangThai = trangthai
            };
            return allRepo.EditItem(voucher);
        }
        // DELETE api/<ChatLieuController>/5
        [HttpDelete("XoaVoucherNguoiDung{id}")]
        public bool Delete(string id)
        {
            var cl = allRepo.GetAll().FirstOrDefault(c => c.IdVouCherNguoiDung == id);
            return allRepo.RemoveItem(cl);
        }
    }
}
