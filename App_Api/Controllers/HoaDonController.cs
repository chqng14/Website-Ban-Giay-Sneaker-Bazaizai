using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.HoaDonChiTietDTO;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private PTThanhToanChiTietController _PTThanhToanChiTietController;
        private HoaDonChiTietController _hoaDonChiTietController;
        private readonly IHoaDonRepos _hoaDon;
        private readonly IMapper _mapper;
        private PTThanhToanController _PTThanhToanController;
        public HoaDonController(IMapper mapper)
        {
            _hoaDon = new HoaDonRepos(mapper);
            _mapper = mapper;
            _hoaDonChiTietController = new HoaDonChiTietController(mapper);
            _PTThanhToanChiTietController = new PTThanhToanChiTietController();
            _PTThanhToanController = new PTThanhToanController();
        }
        [HttpPost]
        public async Task<HoaDon> TaoHoaDonTaiQuay(HoaDon hoaDon)
        {
            return _hoaDon.TaoHoaDonTaiQuay(hoaDon);
        }
        // POST api/<HoaDonController>
        [HttpPost]
        public async Task<string> TaoHoaDonOnlineDTO(HoaDonDTO HoaDonDTO)
        {
            var hoadon = _mapper.Map<HoaDon>(HoaDonDTO);
            _hoaDon.AddBill(hoadon);
            return hoadon.MaHoaDon;
        }
        [HttpGet]
        public async Task<List<HoaDonChoDTO>> GetAllHoaDonCho()
        {
            return _hoaDon.GetAllHoaDonCho();
        }
        [HttpGet]
        public async Task<List<HoaDonViewModel>> GetHoaDonOnline()
        {
            return _hoaDon.GetHoaDon();
        }

        [HttpPut]
        public async Task<bool> UpdateTrangThaiHoaDonOnline(string idHoaDon, int TrangThaiThanhToan)
        {
            var hoadon = _hoaDon.GetHoaDonUpdate().FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            hoadon.TrangThaiThanhToan = TrangThaiThanhToan;
            _hoaDonChiTietController.SuaTrangThaiHoaDon(idHoaDon, TrangThaiThanhToan);
            return _hoaDon.EditBill(hoadon);
        }

        [HttpPut]
        public async Task<bool> UpdateNgayHoaDonOnline(string idHoaDon, DateTime NgayThanhToan, DateTime NgayNhan, DateTime NgayShip)
        {
            var hoadon = _hoaDon.GetHoaDonUpdate().FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            hoadon.NgayNhan = NgayNhan;
            hoadon.NgayShip = NgayShip;
            hoadon.NgayThanhToan = NgayThanhToan;
            return _hoaDon.EditBill(hoadon);
        }

        [HttpGet]
        public async Task<string> GetPTThanhToan(string idhoadon)
        {
            var pt = _PTThanhToanChiTietController.PhuongThucThanhToanChiTietByIdPTTT(idhoadon);
            var idpt = _PTThanhToanController.ShowAll().FirstOrDefault(c => c.IdPhuongThucThanhToan == pt.IdThanhToan);
            return idpt.TenPhuongThucThanhToan;
        }
    }
}
