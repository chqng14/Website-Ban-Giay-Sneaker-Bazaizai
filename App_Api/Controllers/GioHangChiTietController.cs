using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.SanPhamChiTiet;
using AutoMapper;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Identity;
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
        //private readonly SignInManager<NguoiDung> _signInManager;
        //private readonly UserManager<NguoiDung> _userManager;
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
            //_signInManager = signInManager;
            //_userManager = userManager;
        }
        // GET: api/<GioHangChiTietController>

        [HttpGet("Get-List-GioHangChiTietDTO")]
        public async Task<IEnumerable<GioHangChiTietDTO>> GetAll()
        {
            return _gioHangChiTiet.GetAllGioHangDTO();
        }

        [HttpGet("Get-List-SanPhamGioHangVM/{idNguoiDung}")]
        public async Task<List<SanPhamGioHangViewModel>> GetListSanPhamGioHangVmWhenLogin(string idNguoiDung)
        {
            return await _gioHangChiTiet.GetAllSanPhamGioHangWhenLoginAynsc(idNguoiDung);
        }

        // GET api/<GioHangChiTietController>/5
        //[HttpGet("GetGioHangCTById")]
        //public GioHangChiTiet GetGioHangCTById(string id)
        //{
        //    return allRepo.GetAll().FirstOrDefault(c => c.IdGioHangChiTiet == id);
        //}

        // POST api/<GioHangChiTietController>
        [HttpPost("Create")]
        public async Task<bool> TaoGioHangDTO(GioHangChiTietDTOCUD GioHangChiTietDTOCUD)
        {
            //var idNguoiDung = _userManager.GetUserId(User);
            var giohangChiTiet = _mapper.Map<GioHangChiTiet>(GioHangChiTietDTOCUD);
            giohangChiTiet.IdGioHangChiTiet = GioHangChiTietDTOCUD.IdGioHangChiTiet;
            giohangChiTiet.IdSanPhamCT = GioHangChiTietDTOCUD.IdSanPhamCT;
            giohangChiTiet.IdNguoiDung = GioHangChiTietDTOCUD.IdNguoiDung;
            giohangChiTiet.Soluong = GioHangChiTietDTOCUD.SoLuong;
            giohangChiTiet.GiaGoc = GioHangChiTietDTOCUD.GiaGoc;
            giohangChiTiet.GiaBan = GioHangChiTietDTOCUD.GiaBan;
            giohangChiTiet.TrangThai = 0;
            _gioHangChiTiet.AddCartDetail(giohangChiTiet);
            return true;
        }

        // PUT api/<GioHangChiTietController>/5
        [HttpPut("Edit")]
        public bool UpdateCart(string IdSanPhamChiTiet, int SoLuong, string IdNguoiDung)
        {
            var ghct = _gioHangChiTiet.GetAll().FirstOrDefault(c => c.IdSanPhamCT == IdSanPhamChiTiet && c.IdNguoiDung == IdNguoiDung);
            ghct.Soluong = SoLuong;
            return _gioHangChiTiet.EditCartDetail(ghct);
        }

        // DELETE api/<GioHangChiTietController>/5
        [HttpDelete("Delete")]
        public bool Delete(string id)
        {
            var ghct = _gioHangChiTiet.GetAll().FirstOrDefault(c => c.IdGioHangChiTiet == id);
            return _gioHangChiTiet.RemoveCartDetail(ghct);
        }
    }
}
