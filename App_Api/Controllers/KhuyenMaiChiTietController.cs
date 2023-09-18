using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiChiTietController : ControllerBase
    {
        private readonly IAllRepo<KhuyenMaiChiTiet> repos;
        BazaizaiContext context = new BazaizaiContext();
        DbSet<KhuyenMaiChiTiet> khuyenMaiChiTiets;
        public KhuyenMaiChiTietController()
        {
            khuyenMaiChiTiets = context.khuyenMaiChiTiets;
            AllRepo<KhuyenMaiChiTiet> all = new AllRepo<KhuyenMaiChiTiet>(context, khuyenMaiChiTiets);
            repos = all;
        }
        [HttpGet]
        public IEnumerable<KhuyenMaiChiTiet> GetAllKhuyenMai()
        {
            return repos.GetAll();
        }

        [HttpPost]

        public bool CreateKhuyenMaiChiTiet(string mota, int trangThai, Guid IDKm, Guid IDSpCt)
        {
            string MaTS;
            if (repos.GetAll().Count() == null)
            {
                MaTS = "KMCT1";
            }
            else
            {
                MaTS = "KMCT" + (repos.GetAll().Count() + 1);
            }
            KhuyenMaiChiTiet b = new KhuyenMaiChiTiet();
            b.IDKhuyenMaiChiTiet = Guid.NewGuid();
            b.IDKhuyenMai = IDKm;
            b.IDSanPhamChiTiet = IDSpCt;
            b.MoTa=mota;
            b.TrangThai=trangThai;
            return repos.AddItem(b);
        }


        [HttpPut("{id}")]
        public bool EditKhuyenMaiChiTiet(Guid id, string mota, int trangThai, Guid IDKm, Guid IDSpCt)
        {
            var b = repos.GetAll().First(p => p.IDKhuyenMaiChiTiet == a.IDKhuyenMaiChiTiet);
            b.IDKhuyenMai = IDKm;
            b.IDSanPhamChiTiet = IDSpCt;
            b.MoTa = mota;
            b.TrangThai = trangThai;
            return repos.EditItem(b);
        }

        [HttpDelete("{id}")]
        public bool DeleteKhuyenMaiChiTiet(Guid id)
        {
            var KhuyenMaiChiTiet = repos.GetAll().First(p => p.IDKhuyenMaiChiTiet == id);
            return repos.RemoveItem(KhuyenMaiChiTiet);
        }
    }
}
