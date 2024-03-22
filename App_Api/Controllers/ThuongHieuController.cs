using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.SanPhamChiTiet.ThuongHieuDTO;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
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
        DbSet<ThuongHieu> ThuongHieus;
        private readonly IMapper _mapper;
        public ThuongHieuController(IMapper mapper)
        {
            ThuongHieus = context.ThuongHieus;
            AllRepo<ThuongHieu> all = new AllRepo<ThuongHieu>(context, ThuongHieus);
            repos = all;
            _mapper = mapper;
        }
        [HttpGet]
        public IEnumerable<ThuongHieu> GetAllKhuyenMai()
        {
            return repos.GetAll();
        }

        [HttpPost]

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

        [HttpPut("sua-thuong-hieu")]
        public bool EditThuongHieu(ThuongHieuDTO thuongHieuDTO)
        {
            try
            {
                var nameThuongHieuLower = thuongHieuDTO.TenThuongHieu!.Trim().ToLower();
                if (!context.ThuongHieus.Where(x => x.TenThuongHieu!.Trim().ToLower() == nameThuongHieuLower).Any())
                {
                    var thuongHieu = _mapper.Map<ThuongHieu>(thuongHieuDTO);
                    context.Attach(thuongHieu);
                    context.Entry(thuongHieu).Property(sp => sp.TenThuongHieu).IsModified = true;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpDelete("{id}")]
        public bool DeleteThuongHieu(string id)
        {
            var ThuongHieu = repos.GetAll().First(p => p.IdThuongHieu == id);
            return repos.RemoveItem(ThuongHieu);
        }
    }
}
