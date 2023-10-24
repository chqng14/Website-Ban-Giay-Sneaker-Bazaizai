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

        private readonly IHoaDonRepos _hoaDon;
        private readonly IMapper _mapper;
        public HoaDonController(IMapper mapper)
        {
            _hoaDon = new HoaDonRepos();
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<HoaDon> TaoHoaDonTaiQuay(HoaDon hoaDon)
        {
            return _hoaDon.TaoHoaDonTaiQuay(hoaDon);
        }
        // POST api/<HoaDonController>
        [HttpPost]
        public async Task<bool> TaoHoaDonOnlineDTO(HoaDonDTO HoaDonDTO)
        {
            var hoadon = _mapper.Map<HoaDon>(HoaDonDTO);
            return _hoaDon.AddBill(hoadon);
        }
        [HttpGet]
        public async Task<List<HoaDonChoDTO>> GetAllHoaDonCho()
        {
            return _hoaDon.GetAllHoaDonCho();
        }
        [HttpGet]
        public async Task<List<HoaDonDTO>> GetHoaDonOnline()
        {
            return _hoaDon.GetHoaDon();
        }
    }
}
