using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.SanPhamChiTiet.SanPhamDTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly IAllRepo<SanPham> allRepo;
        private readonly IMapper _mapper;
        public SanPhamController(IMapper mapper, IAllRepo<SanPham> repository)
        {
            allRepo = repository;
            _mapper = mapper;
        }
        // GET: api/<SanPhamController>
        [HttpGet]
        public IEnumerable<SanPham> ShowAllKieuGiay()
        {
            return allRepo.GetAll();
        }

        // GET api/<SanPhamController>/5
        [HttpGet("TimSanPham={id}")]
        public SanPham GetSanPhamByID(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdSanPham == id);
        }

        // POST api/<SanPhamController>
        [HttpPost]
        public bool AddSanPham(string ten, int trangthai)
        {
            string ma;
            if (allRepo.GetAll().Count() == null)
            {
                ma = "SP1";
            }
            else
            {
                ma = "SP" + (allRepo.GetAll().Count() + 1);
            }
            var SanPham = new SanPham()
            {
                IdSanPham = Guid.NewGuid().ToString(),
                MaSanPham = ma,
                TenSanPham = ten,
                Trangthai = trangthai
            };
            return allRepo.AddItem(SanPham);
        }

        [HttpPut("SuaSanPham")]
        public bool SuaSanPham(SanPhamDTO sanPham)
        {
            try
            {
                var nameSanPhamLower = sanPham.TenSanPham!.Trim().ToLower();
                if (!allRepo.GetAll().Any(x => x.TenSanPham!.Trim().ToLower() == nameSanPhamLower && x.IdSanPham != sanPham.IdSanPham))
                {
                    var sanPhamGet = allRepo.GetAll().First(x => x.IdSanPham == sanPham.IdSanPham);
                    sanPhamGet.TenSanPham = sanPham.TenSanPham;
                    return allRepo.EditItem(sanPhamGet);
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // DELETE api/<SanPhamController>/5
        [HttpDelete("XoaSanPham={id}")]
        public bool XoaSanPham(string id)
        {
            var SanPham = allRepo.GetAll().FirstOrDefault(c => c.IdSanPham == id);
            return allRepo.RemoveItem(SanPham);
        }
    }
}
