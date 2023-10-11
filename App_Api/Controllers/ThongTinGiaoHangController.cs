﻿using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.HoaDonChiTietDTO;
using App_Data.ViewModels.ThongTinGHDTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongTinGiaoHangController : ControllerBase
    {
        private readonly IAllRepo<GioHangChiTiet> allRepo;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        private readonly IThongTinGHRepos thongTinGHRepos;
        private readonly IMapper _mapper;
        public ThongTinGiaoHangController(IMapper mapper)
        {
            thongTinGHRepos = new ThongTinGHRepos(mapper);
            _mapper = mapper;
        }
        // GET: api/<ThongTinGiaoHangController>
        [HttpGet("GetAll")]
        public IEnumerable<ThongTinGiaoHang> GetAll()
        {
            return thongTinGHRepos.GetAll();
        }

        // GET api/<ThongTinGiaoHangController>/5
        [HttpGet("GetByIdUser")]
        public IEnumerable<ThongTinGiaoHang> GetByIdUser(string idNguoiDung)
        {
            return thongTinGHRepos.GetAll().Where(c => c.IdNguoiDung == idNguoiDung);
        }

        // POST api/<ThongTinGiaoHangController>
        [HttpPost("Create")]
        public async Task<bool> Create(ThongTinGHDTO thongTinGHDTO)
        {
            var thongTinGiaoHang = _mapper.Map<ThongTinGiaoHang>(thongTinGHDTO);
            return thongTinGHRepos.AddThongTinGH(thongTinGiaoHang);

        }

        // PUT api/<ThongTinGiaoHangController>/5
        [HttpPut("Edit")]
        public async Task<bool> Edit(ThongTinGHDTO thongTinGHDTO)
        {
            var thongTinGiaoHang = _mapper.Map<ThongTinGiaoHang>(thongTinGHDTO);
            return thongTinGHRepos.EditThongTinGH(thongTinGiaoHang);

        }

        // DELETE api/<ThongTinGiaoHangController>/5
        [HttpDelete("Delete")]
        public async Task<bool> Delete(string id)
        {
            var ttgh = thongTinGHRepos.GetAll().FirstOrDefault(c => c.IdThongTinGH == id);
            return thongTinGHRepos.RemoveThongTinGH(ttgh);
        }
    }
}