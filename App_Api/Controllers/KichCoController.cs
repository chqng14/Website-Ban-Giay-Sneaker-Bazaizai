using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.KichCoDTO;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KichCoController : ControllerBase
    {
        private readonly IAllRepo<KichCo> repos;
        BazaizaiContext context = new BazaizaiContext();
        DbSet<KichCo> KichCos;
        private readonly IMapper _mapper;
        public KichCoController(IMapper mapper)
        {
            KichCos = context.KichCos;
            AllRepo<KichCo> all = new AllRepo<KichCo>(context, KichCos);
            repos = all;
            _mapper = mapper;
        }
        [HttpGet]
        public IEnumerable<KichCo> GetAllKichCo()
        {
            return repos.GetAll();
        }

        [HttpPost]

        public bool CreateKichCo( int TrangThai, int KichCo)
        {
            string MaTS;
            if (repos.GetAll().Count() == null)
            {
                MaTS = "KC1";
            }
            else
            {
                MaTS = "KC" + (repos.GetAll().Count() + 1);
            }
            KichCo b = new KichCo();
            b.IdKichCo = Guid.NewGuid().ToString();
            b.MaKichCo = MaTS;
            b.SoKichCo = KichCo;      
            b.TrangThai = TrangThai;
            return repos.AddItem(b);
        }


        [HttpPut("sua-kich-co")]
        public bool EditKichCo(KichCoDTO kichCoDTO)
        {
            try
            {
                var kichCo = kichCoDTO.SoKichCo;
                if (!context.KichCos.Where(x => x.SoKichCo == kichCo).Any())
                {
                    var kichCoGet = _mapper.Map<KichCo>(kichCoDTO);
                    context.Attach(kichCoGet);
                    context.Entry(kichCoGet).Property(sp => sp.SoKichCo).IsModified = true;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            };
        }

        [HttpDelete("{id}")]
        public bool DeleteKichCo(string id)
        {
            try
            {
                var KichCo = repos.GetAll().First(p => p.IdKichCo == id);
                return repos.RemoveItem(KichCo);
            }
            catch (Exception)
            {
                return false;
            }
           
        }
    }
}
