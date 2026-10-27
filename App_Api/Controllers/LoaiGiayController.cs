using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.LoaiGiayDTO;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiGiayController : ControllerBase
    {
        private readonly IAllRepo<LoaiGiay> allRepo;
        private readonly IMapper _mapper;
        public LoaiGiayController(IMapper mapper, IAllRepo<LoaiGiay> repository)
        {
            allRepo = repository;
            _mapper = mapper;
        }
        // GET: api/<LoaiGiayController>
        [HttpGet]
        public IEnumerable<LoaiGiay> Get()
        {
            return allRepo.GetAll();
        }

        // GET api/<LoaiGiayController>/5
        [HttpGet("GetLoaiGiayById")]
        public LoaiGiay GetLoaiGiayById(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdLoaiGiay == id);
        }

        // POST api/<LoaiGiayController>
        [HttpPost("Create")]
        public bool Post(string TenLoaiGiay, int TrangThai)
        {
            string ma;
            if (allRepo.GetAll().Count() == null)
            {
                ma = "LG1";
            }
            else
            {
                ma = "LG" + (allRepo.GetAll().Count() + 1);
            }
            LoaiGiay lg = new LoaiGiay()
            {
                IdLoaiGiay = Guid.NewGuid().ToString(),
                MaLoaiGiay = ma,
                TenLoaiGiay = TenLoaiGiay,
                TrangThai = TrangThai,
            };
            return allRepo.AddItem(lg);
        }

        // PUT api/<LoaiGiayController>/5
        [HttpPut("sua-loai-giay")]
        public bool Put(LoaiGiayDTO loaiGiayDTO)
        {
            try
            {
                var nameLoaiGiay = loaiGiayDTO.TenLoaiGiay!.Trim().ToLower();
                if (!allRepo.GetAll().Any(x => x.TenLoaiGiay!.Trim().ToLower() == nameLoaiGiay && x.IdLoaiGiay != loaiGiayDTO.IdLoaiGiay))
                {
                    var loaiGiay = allRepo.GetAll().First(x => x.IdLoaiGiay == loaiGiayDTO.IdLoaiGiay);
                    loaiGiay.TenLoaiGiay = loaiGiayDTO.TenLoaiGiay;
                    return allRepo.EditItem(loaiGiay);
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // DELETE api/<LoaiGiayController>/5
        [HttpDelete("Delete")]
        public bool Delete(string idLoaiGiay)
        {
            var lg = allRepo.GetAll().First(p => p.IdLoaiGiay == idLoaiGiay);
            return allRepo.RemoveItem(lg);
        }
    }
}
