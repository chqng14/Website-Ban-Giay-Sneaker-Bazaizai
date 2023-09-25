using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IAllRepo<Voucher> allRepo;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        DbSet<Voucher> vouchers;

        public VoucherController()
        {
            vouchers = DbContextModel.vouchers;
            AllRepo<Voucher> all = new AllRepo<Voucher>(DbContextModel, vouchers);
            allRepo = all;
        }
        [HttpGet("GetVoucher")]
        public IEnumerable<Voucher> GetAll()
        {
            return allRepo.GetAll();
        }
        [HttpGet("GetVoucherByMa")]
        public Voucher GetAll(string ma)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.MaVoucher == ma);
        }
        [HttpPost("AddVoucher")]
        public bool AddVoucher(string MaVoucher, string TenVoucher, string DieuKien, string LoaiHinhUuDai, int SoLuong, double MucUuDai, DateTime NgayBatDau, DateTime NgayKetThuc, int TrangThai)
        {
            var voucher = new Voucher()
            {
                IdVoucher = Convert.ToString(Guid.NewGuid()),
                MaVoucher = MaVoucher,
                TenVoucher = TenVoucher,
                DieuKien = DieuKien,
                LoaiHinhUuDai = LoaiHinhUuDai,
                SoLuong = SoLuong,
                MucUuDai = MucUuDai,
                NgayBatDau = NgayBatDau,
                NgayKetThuc = NgayKetThuc,
                TrangThai = TrangThai
            };
            return allRepo.AddItem(voucher);
        }
        [HttpPut("{id}")]
        public bool UpdateVoucher(string IdVoucher,string MaVoucher, string TenVoucher, string DieuKien, string LoaiHinhUuDai, int SoLuong, double MucUuDai, DateTime NgayBatDau, DateTime NgayKetThuc, int TrangThai)
        {
            var _voucher = new Voucher()
            {
                IdVoucher = IdVoucher,
                MaVoucher = MaVoucher,
                TenVoucher = TenVoucher,
                DieuKien = DieuKien,
                LoaiHinhUuDai = LoaiHinhUuDai,
                SoLuong = SoLuong,
                MucUuDai = MucUuDai,
                NgayBatDau = NgayBatDau,
                NgayKetThuc = NgayKetThuc,
                TrangThai = TrangThai
            };
            return allRepo.EditItem(_voucher);
        }
        [HttpDelete("{id}")]
        public bool DeleteVoucher(string id)
        {
            return allRepo.RemoveItem(allRepo.GetAll().FirstOrDefault(c => c.IdVoucher == id));
        }
    }
}
