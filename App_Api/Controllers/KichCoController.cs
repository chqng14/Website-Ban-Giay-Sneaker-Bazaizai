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
        private readonly IMapper _mapper;
        public KichCoController(IMapper mapper, IAllRepo<KichCo> repository)
        {
            repos = repository;
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
                if (!repos.GetAll().Any(x => x.SoKichCo == kichCo && x.IdKichCo != kichCoDTO.IdKichCo))
                {
                    var kichCoGet = repos.GetAll().First(x => x.IdKichCo == kichCoDTO.IdKichCo);
                    kichCoGet.SoKichCo = kichCoDTO.SoKichCo;
                    return repos.EditItem(kichCoGet);
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
