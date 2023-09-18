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
    public class ThuongHieuController : ControllerBase
    {
        private readonly IAllRepo<ThuongHieu> repos;
        BazaizaiContext context = new BazaizaiContext();
        DbSet<ThuongHieu> thuongHieus;
        public ThuongHieuController()
        {
            thuongHieus = context.thuongHieus;
            AllRepo<ThuongHieu> all = new AllRepo<ThuongHieu>(context, thuongHieus);
            repos = all;
        }
        [HttpGet]
        public IEnumerable<ThuongHieu> GetAllKhuyenMai()
        {
            return repos.GetAll();
        }

        [HttpPost]

        //public bool CreateThuongHieu(ThuongHieu a)
        //{
        //    ThuongHieu b = new ThuongHieu();
        //    b.IDThuongHieu = Guid.NewGuid();
        //    b.MaThuongHieu = a.MaThuongHieu;
        //    b.TenThuongHieu = a.TenThuongHieu;
        //    b.TrangThai = a.TrangThai;
        //    return repos.AddItem(b);
        //}
        public bool CreateThuongHieu( int trangThai, string Ten)
        {
            string MaTS;
            if (repos.GetAll().Count() == null)
            {
                MaTS = "TH1";
            }
            else
            {
                MaTS = "TH" + (repos.GetAll().Count() + 1);
            }
            ThuongHieu b = new ThuongHieu();
            b.IdThuongHieu = Guid.NewGuid().ToString();
            b.MaThuongHieu = MaTS;
            b.TenThuongHieu = Ten;
            b.TrangThai = trangThai;
            return repos.AddItem(b);
        }

        [HttpPut("{id}")]
        public bool EditThuongHieu(string id,string Ma, int trangThai, string Ten)
        {
            var b = repos.GetAll().First(p => p.IdThuongHieu == id);
            b.MaThuongHieu = Ma;
            b.TenThuongHieu = Ten;
            b.TrangThai = trangThai;
            return repos.EditItem(b);
        }

        [HttpDelete("{id}")]
        public bool DeleteThuongHieu(string id)
        {
            var ThuongHieu = repos.GetAll().First(p => p.IdThuongHieu == id);
            return repos.RemoveItem(ThuongHieu);
        }
    }
}
