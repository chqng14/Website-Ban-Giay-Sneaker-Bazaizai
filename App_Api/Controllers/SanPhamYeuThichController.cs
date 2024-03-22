using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.SanPhamYeuThichDTO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamYeuThichController : ControllerBase
    {
        private readonly IAllRepo<SanPhamYeuThich> _allRepoSanPhamYeuThich;
        private readonly BazaizaiContext _bazaizaiContext;
        private readonly IMapper _mapper;

        public SanPhamYeuThichController(IAllRepo<SanPhamYeuThich> allRepoSanPhamYeuThich, IMapper mapper)
        {
            _allRepoSanPhamYeuThich = allRepoSanPhamYeuThich;
            _bazaizaiContext = new BazaizaiContext();
            _mapper = mapper;
        }

        [HttpGet("Get-Danh-Sach-SanPhamYeuThich")]
        public List<SanPhamYeuThichViewModel> GetDanhSachSanPhamYeuThich(string idNguoiDung)
        {
            var lstSanPhamYeuThich = _bazaizaiContext
             .SanPhamYeuThichs
             .Where(spyt => spyt.IdNguoiDung == idNguoiDung)
             .Include(yt => yt.SanPhamChiTiet).ThenInclude(sp => sp.SanPham)
             .Include(yt => yt.SanPhamChiTiet).ThenInclude(sp => sp.LoaiGiay)
             .Include(yt => yt.SanPhamChiTiet).ThenInclude(sp => sp.KichCo)
             .Include(yt => yt.SanPhamChiTiet).ThenInclude(sp => sp.Anh)
             .ToList();
            var lstSanPhamYeuThichViewModel = _mapper.Map<List<SanPhamYeuThich>, List<SanPhamYeuThichViewModel>>(lstSanPhamYeuThich);
            return lstSanPhamYeuThichViewModel;
        }

        [HttpPost("add-sanphamyeuthich")]
        public void AddSanPhamYeuThich(SanPhamYeuThichDTO sanPhamYeuThichDTO)
        {
            var exists = _bazaizaiContext.SanPhamYeuThichs
                 .Any(spyt =>
                 spyt.IdNguoiDung == sanPhamYeuThichDTO.IdNguoiDung &&
                 spyt.IdSanPhamChiTiet == sanPhamYeuThichDTO.IdSanPhamChiTiet
                 );
            if (!exists)
            {
                var sanPhamYeuThich = _mapper.Map<SanPhamYeuThich>(sanPhamYeuThichDTO);
                sanPhamYeuThich.IdSanPhamYeuThich = Guid.NewGuid().ToString();
                _allRepoSanPhamYeuThich.AddItem(sanPhamYeuThich);
            }
        }

        [HttpDelete("Remove-sanphamyeuthich")]
        public void RemoveSanPhamYeuThich(SanPhamYeuThichDTO sanPhamYeuThichDTO)
        {
            var sanPhamYeuThich = _bazaizaiContext
                .SanPhamYeuThichs
                .Where(yt => yt.IdNguoiDung == sanPhamYeuThichDTO.IdNguoiDung && yt.IdSanPhamChiTiet == sanPhamYeuThichDTO.IdSanPhamChiTiet).FirstOrDefault();
            _allRepoSanPhamYeuThich.RemoveItem(sanPhamYeuThich!);
        }


    }
}
