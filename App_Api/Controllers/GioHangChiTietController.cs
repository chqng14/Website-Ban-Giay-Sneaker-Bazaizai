using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.SanPhamChiTiet;
using AutoMapper;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangChiTietController : ControllerBase
    {
        private readonly IAllRepo<GioHangChiTiet> allRepo;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        private readonly IGioHangChiTietRepos _gioHangChiTiet;
        private readonly IAllRepo<KichCo> _kickcoRes;
        private readonly IAllRepo<SanPham> _sanPhamRes;
        private readonly IAllRepo<MauSac> _mauSacRes;
        private readonly ISanPhamChiTietRespo _sanPhamChiTietRes;
        private readonly IAllRepo<Anh> _AnhRes;
        private readonly IAllRepo<NguoiDung> _NguoiDung;
        private readonly IMapper _mapper;
        public GioHangChiTietController(IAllRepo<KichCo> kickcoRes, IAllRepo<SanPham> sanPhamRes, IAllRepo<MauSac> mauSacRes, ISanPhamChiTietRespo sanPhamChiTietRes, IMapper mapper, IAllRepo<Anh> anhRes, IAllRepo<NguoiDung> nguoiDung)
        {
            _gioHangChiTiet = new GioHangChiTietRepos(mapper);
            _kickcoRes = kickcoRes;
            _sanPhamRes = sanPhamRes;
            _mauSacRes = mauSacRes;
            _sanPhamChiTietRes = sanPhamChiTietRes;
            _mapper = mapper;
            _AnhRes = anhRes;
            _NguoiDung = nguoiDung;
        }
        // GET: api/<GioHangChiTietController>

        [HttpGet("Get-List-GioHangChiTietDTO")]
        public async Task<IEnumerable<GioHangChiTietDTO>> GetAll()
        {
            return _gioHangChiTiet.GetAllGioHangDTO();
        }

        // GET api/<GioHangChiTietController>/5
        [HttpGet("GetGioHangCTById")]
        public GioHangChiTiet GetGioHangCTById(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdGioHangChiTiet == id);
        }

        // POST api/<GioHangChiTietController>
        [HttpPost("Create")]
        public async Task<bool> TaoGioHangDTO(GioHangChiTietDTOCUD GioHangChiTietDTOCUD)
        {
            var giohangChiTiet = _mapper.Map<GioHangChiTiet>(GioHangChiTietDTOCUD);
            giohangChiTiet.IdGioHangChiTiet = Guid.NewGuid().ToString();
            giohangChiTiet.IdSanPhamCT = GioHangChiTietDTOCUD.sanPhamChiTietDTO.IdChiTietSp;
            giohangChiTiet.IdNguoiDung = GioHangChiTietDTOCUD.GioHangDTO.IdNguoiDung;
            giohangChiTiet.Soluong = 1;
            giohangChiTiet.GiaGoc = GioHangChiTietDTOCUD.sanPhamChiTietDTO.GiaBan;
            giohangChiTiet.TrangThai = 0;
            _gioHangChiTiet.AddCartDetail(giohangChiTiet);
            return true;
        }

        // PUT api/<GioHangChiTietController>/5
        [HttpPut("Edit")]
        public bool Put(string IdGioHangChiTiet, string IdNguoiDung, string IdSanPhamChiTiet, int SoLuong, double GiaGoc, int TrangThai)
        {
            var ghct = allRepo.GetAll().FirstOrDefault(c => c.IdGioHangChiTiet == IdGioHangChiTiet);
            ghct.IdNguoiDung = IdNguoiDung;
            ghct.IdSanPhamCT = IdSanPhamChiTiet;
            ghct.Soluong = SoLuong;
            ghct.GiaGoc = GiaGoc;
            ghct.TrangThai = TrangThai;
            return allRepo.EditItem(ghct);
        }

        // DELETE api/<GioHangChiTietController>/5
        [HttpDelete("Delete")]
        public bool Delete(string id)
        {
            var ghct = allRepo.GetAll().FirstOrDefault(c => c.IdGioHangChiTiet == id);
            return allRepo.RemoveItem(ghct);
        }
    }
}
