using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IAllRepo<HoaDon> allRepo;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        DbSet<HoaDon> hoaDons;
        public HoaDonController()
        {
            hoaDons = DbContextModel.HoaDons;
            allRepo = new AllRepo<HoaDon>(DbContextModel, hoaDons);
        }
        // GET: api/<HoaDonController>
        [HttpGet]
        public IEnumerable<HoaDon> Get()
        {
            return allRepo.GetAll();
        }

        // GET api/<HoaDonController>/5
        [HttpGet("GetHoaDonById")]
        public HoaDon GetHoaDonById(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdHoaDon == id);
        }

        // POST api/<HoaDonController>
        [HttpPost("Create")]
        public bool Post(string IdVoucher, string IdNguoiDung, string IdKhachHang, string IdThongTinGH, DateTime NgayTao, DateTime NgayThanhToan, DateTime NgayShip, DateTime NgayNhan, double TienShip, double TienGiam, double TongTien, string MoTa, int TrangThai, int TrangThaiThanhToan)
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
            HoaDon hd = new HoaDon()
            {
                IdHoaDon = Guid.NewGuid().ToString(),
                IdVoucher = IdVoucher,
                IdKhachHang = IdKhachHang,
                IdNguoiDung = IdNguoiDung,
                IdThongTinGH = IdThongTinGH,
                MaHoaDon = ma,
                NgayTao = NgayTao,
                NgayThanhToan = NgayThanhToan,
                NgayShip = NgayShip,
                NgayNhan = NgayNhan,
                TienShip = TienShip,
                TienGiam = TienGiam,
                TongTien = TongTien,
                MoTa = MoTa,
                TrangThai = TrangThai,
                TrangThaiThanhToan = TrangThaiThanhToan,
            };
            return allRepo.AddItem(hd);
        }

        // PUT api/<HoaDonController>/5
        [HttpPut("Edit")]
        public bool Put(string idHoaDon, string IdVoucher, string IdNguoiDung, string IdKhachHang, string IdThongTinGH, string MaHoaDon, DateTime NgayTao, DateTime NgayThanhToan, DateTime NgayShip, DateTime NgayNhan, double TienShip, double TienGiam, double TongTien, string MoTa, int TrangThai, int TrangThaiThanhToan)
        {
            var hd = allRepo.GetAll().First(p => p.IdHoaDon == idHoaDon);
            hd.IdVoucher = IdVoucher;
            hd.IdThongTinGH = IdThongTinGH;
            hd.IdVoucher = IdVoucher;
            hd.IdKhachHang = IdKhachHang;
            hd.IdNguoiDung = IdNguoiDung;
            hd.IdThongTinGH = IdThongTinGH;
            hd.MaHoaDon = MaHoaDon;
            hd.NgayTao = NgayTao;
            hd.NgayThanhToan = NgayThanhToan;
            hd.NgayShip = NgayShip;
            hd.NgayNhan = NgayNhan;
            hd.TienShip = TienShip;
            hd.TienGiam = TienGiam;
            hd.TongTien = TongTien;
            hd.MoTa = MoTa;
            hd.TrangThai = TrangThai;
            hd.TrangThaiThanhToan = TrangThaiThanhToan;
            return allRepo.EditItem(hd);
        }

        // DELETE api/<HoaDonController>/5
        [HttpDelete("Delete")]
        public bool Delete(string idHoaDon)
        {
            var hd = allRepo.GetAll().First(p => p.IdHoaDon == idHoaDon);
            return allRepo.RemoveItem(hd);
        }
    }
}
