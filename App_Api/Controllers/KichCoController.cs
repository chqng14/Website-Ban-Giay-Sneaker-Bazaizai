using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
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
        DbSet<KichCo> kichCos;
        public KichCoController()
        {
            kichCos = context.kichCos;
            AllRepo<KichCo> all = new AllRepo<KichCo>(context, kichCos);
            repos = all;
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
            b.IDKichCo = Guid.NewGuid();
            b.MaKichCo = MaTS;
            b.SoKichCo = KichCo;      
            b.TrangThai = TrangThai;
            return repos.AddItem(b);
        }


        [HttpPut("{id}")]
        public bool EditKichCo(Guid id,int TrangThai, int KichCo, string ma)
        {
            var b = repos.GetAll().First(p => p.IDKichCo == id);
            b.MaKichCo = ma;
            b.SoKichCo = KichCo ;
            b.TrangThai = TrangThai;
            return repos.EditItem(b);
        }

        [HttpDelete("{id}")]
        public bool DeleteKichCo(Guid id)
        {
            var KichCo = repos.GetAll().First(p => p.IDKichCo == id);
            return repos.RemoveItem(KichCo);
        }
    }
}
