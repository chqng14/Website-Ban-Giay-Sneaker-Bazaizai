using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.HoaDonChiTietDTO;
using AutoMapper;
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
        private readonly IHoaDonRepos _hoaDon;
        private readonly IMapper _mapper;
        public HoaDonController(IMapper mapper)
        {
            _hoaDon = new HoaDonRepos();
            _mapper = mapper;
        }
        // GET: api/<HoaDonController>
        [HttpGet]
        public IEnumerable<HoaDon> Get()
        {
            return _hoaDon.GetAll();
        }

        // GET api/<HoaDonController>/5
        [HttpGet("GetHoaDonById")]
        public HoaDon GetHoaDonById(string id)
        {
            return _hoaDon.GetAll().FirstOrDefault(c => c.IdHoaDon == id);
        }

        // POST api/<HoaDonController>
        [HttpPost("Create")]
        public async Task<bool> TaoHoaDonOnlineDTO(HoaDonDTO HoaDonDTO)
        {
            var hoadon = _mapper.Map<HoaDon>(HoaDonDTO);
            return _hoaDon.AddBill(hoadon);
        }

        // PUT api/<HoaDonController>/5
        [HttpPut("Edit")]
        public bool Put(string idHoaDon, string IdVoucher, string IdNguoiDung, string IdKhachHang, string IdThongTinGH, string MaHoaDon, DateTime NgayTao, DateTime NgayThanhToan, DateTime NgayShip, DateTime NgayNhan, double TienShip, double TienGiam, double TongTien, string MoTa, int TrangThai, int TrangThaiThanhToan)
        {
            var hd = _hoaDon.GetAll().First(p => p.IdHoaDon == idHoaDon);
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
            return _hoaDon.EditBill(hd);
        }

        // DELETE api/<HoaDonController>/5
        [HttpDelete("Delete")]
        public bool Delete(string idHoaDon)
        {
            var hd = _hoaDon.GetAll().First(p => p.IdHoaDon == idHoaDon);
            return _hoaDon.RemoveBill(hd);
        }
    }
}
