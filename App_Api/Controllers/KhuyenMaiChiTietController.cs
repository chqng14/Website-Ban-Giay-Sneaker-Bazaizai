using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.KhuyenMaiChiTietDTO;
using AutoMapper;
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
        DbSet<KhuyenMaiChiTiet> KhuyenMaiChiTiets;
        private readonly IMapper _mapper;
        public KhuyenMaiChiTietController(IMapper mapper)
        {
            KhuyenMaiChiTiets = context.KhuyenMaiChiTiets;
            AllRepo<KhuyenMaiChiTiet> all = new AllRepo<KhuyenMaiChiTiet>(context, KhuyenMaiChiTiets);
            repos = all;
            _mapper=mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<KhuyenMaiChiTietDTO>> GetAllKhuyenMai()
        {
            var allSale = (await KhuyenMaiChiTiets.Include(c=>c.KhuyenMai).Include(c=>c.SanPhamChiTiet).ThenInclude(c=>c.SanPham).ToListAsync()).ToList();
            var allSaleDTO = _mapper.Map<List<KhuyenMaiChiTietDTO>>(allSale);
            return allSaleDTO;
        }

        [HttpPost]

        public bool CreateKhuyenMaiChiTiet(string mota, int trangThai, string IDKm, string IDSpCt)
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
            b.IdKhuyenMaiChiTiet = Guid.NewGuid().ToString();
            b.IdKhuyenMai = IDKm;
            b.IdSanPhamChiTiet = IDSpCt;
            b.MoTa=mota;
            b.TrangThai=trangThai;
            return repos.AddItem(b);
        }


        [HttpPut("{id}")]
        public bool EditKhuyenMaiChiTiet(string id, string mota, int trangThai, string IDKm, string IDSpCt)
        {
            var b = repos.GetAll().First(p => p.IdKhuyenMaiChiTiet == id);
            b.IdKhuyenMai = IDKm;
            b.IdSanPhamChiTiet = IDSpCt;
            b.MoTa = mota;
            b.TrangThai = trangThai;
            return repos.EditItem(b);
        }

        [HttpDelete("{id}")]
        public bool DeleteKhuyenMaiChiTiet(string id)
        {
            var KhuyenMaiChiTiet = repos.GetAll().First(p => p.IdKhuyenMaiChiTiet == id);
            return repos.RemoveItem(KhuyenMaiChiTiet);
        }
    }
}
