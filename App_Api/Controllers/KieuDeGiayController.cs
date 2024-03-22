using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.KieuDeGiayDTO;
using App_Data.ViewModels.LoaiGiayDTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KieuDeGiayController : ControllerBase
    {
        private readonly IAllRepo<KieuDeGiay> allRepo;
        BazaizaiContext dbContext = new BazaizaiContext();
        DbSet<KieuDeGiay> KieuDeGiay;
        private readonly IMapper _mapper;
        public KieuDeGiayController(IMapper mapper)
        {
            KieuDeGiay = dbContext.KieuDeGiays;
            AllRepo<KieuDeGiay> all = new AllRepo<KieuDeGiay>(dbContext, KieuDeGiay);
            allRepo = all;
            _mapper = mapper;
        }
        // GET: api/<KieuDeGiayController>
        [HttpGet]
        public IEnumerable<KieuDeGiay> ShowAllKieuGiay()
        {
            return allRepo.GetAll();
        }

        // GET api/<KieuDeGiayController>/5
        [HttpGet("TimKieuDeGiay={id}")]
        public KieuDeGiay GetKieuDeGiayByID(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdKieuDeGiay == id);
        }

        // POST api/<KieuDeGiayController>
        [HttpPost]
        public bool AddKieuDeGiay(string ten, int trangthai)
        {
            string ma;
            if (allRepo.GetAll().Count() == null)
            {
                ma = "KDG1";
            }
            else
            {
                ma = "KDG" + (allRepo.GetAll().Count() + 1);
            }
            var KieuDeGiay = new KieuDeGiay()
            {
                IdKieuDeGiay = Guid.NewGuid().ToString(),
                MaKieuDeGiay = ma,
                TenKieuDeGiay = ten,
                Trangthai = trangthai
            };
            return allRepo.AddItem(KieuDeGiay);
        }

        [HttpPut("sua-kieu-de-giay")]
        public bool SuaKieuDeGiay(KieuDeGiayDTO kieuDeGiayDTO)
        {
            try
            {
                var nameKieuDe = kieuDeGiayDTO.TenKieuDeGiay!.Trim().ToLower();
                if (!dbContext.KieuDeGiays.Where(x => x.TenKieuDeGiay!.Trim().ToLower() == nameKieuDe).Any())
                {
                    var kieuDe = _mapper.Map<KieuDeGiay>(kieuDeGiayDTO);
                    dbContext.Attach(kieuDe);
                    dbContext.Entry(kieuDe).Property(sp => sp.TenKieuDeGiay).IsModified = true;
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // DELETE api/<KieuDeGiayController>/5
        [HttpDelete("XoaKieuDeGiay={id}")]
        public bool XoaKieuDeGiay(string id)
        {
            var KieuDeGiay = allRepo.GetAll().FirstOrDefault(c => c.IdKieuDeGiay == id);
            return allRepo.RemoveItem(KieuDeGiay);
        }
    }
}
