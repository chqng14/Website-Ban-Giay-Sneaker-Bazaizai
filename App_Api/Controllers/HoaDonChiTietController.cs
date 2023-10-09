using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.GioHangChiTiet;
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
    public class HoaDonChiTietController : ControllerBase
    {
        BazaizaiContext DbContextModel = new BazaizaiContext();
        private readonly IHoaDonChiTietRepos _hoaDonChiTiet;
        private readonly IMapper _mapper;
        public HoaDonChiTietController(IMapper mapper)
        {
            _hoaDonChiTiet = new HoaDonChiTietRepos(mapper);
            _mapper = mapper;
        }
        // GET: api/<HoaDonChiTietController>
        [HttpGet("GetHoaDonDTO")]
        public async Task<HoaDonChiTietViewModel> GetAll(string idHoaDon)
        {
            return _hoaDonChiTiet.GetHoaDonDTO(idHoaDon);
        }

        // GET api/<HoaDonChiTietController>/5
        [HttpGet("GetHoaDonCTById")]
        public HoaDonChiTiet GetHoaDonCTById(string id)
        {
            return _hoaDonChiTiet.GetAll().FirstOrDefault(c => c.IdHoaDonChiTiet == id);
        }

        // POST api/<HoaDonChiTietController>
        [HttpPost("Create")]
        public async Task<bool> TaoHoaDonDTO(HoaDonChiTietDTO hoaDonChiTietDTO)
        {
            var hoadonChiTiet = _mapper.Map<HoaDonChiTiet>(hoaDonChiTietDTO);
            return _hoaDonChiTiet.AddBillDetail(hoadonChiTiet);
        }

        // PUT api/<HoaDonChiTietController>/5
        [HttpPut("Edit")]
        public async Task<bool> SuaHoaDonDTO(HoaDonChiTietDTO hoaDonChiTietDTO)
        {
            var hoadonChiTiet = _mapper.Map<HoaDonChiTiet>(hoaDonChiTietDTO);
            return _hoaDonChiTiet.EditBillDetail(hoadonChiTiet);
        }

        // DELETE api/<HoaDonChiTietController>/5
        [HttpDelete("Delete")]
        public async Task<bool> Delete(string id)
        {
            var hoadonChiTiet = _hoaDonChiTiet.GetAll().FirstOrDefault(c => c.IdHoaDonChiTiet == id);
            return _hoaDonChiTiet.RemoveBillDetail(hoadonChiTiet);
        }
    }
}
