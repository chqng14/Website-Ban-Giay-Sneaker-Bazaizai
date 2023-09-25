using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.SanPhamChiTiet;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamChiTietController : ControllerBase
    {

        private readonly IAllRepo<KichCo> _kickcoRes;
        private readonly IAllRepo<ThuongHieu> _thuongHieuRes;
        private readonly IAllRepo<LoaiGiay> _loaiGiayRes;
        private readonly IAllRepo<KieuDeGiay> _kieuDeGiayRes;
        private readonly IAllRepo<ChatLieu> _chatLieuRes;
        private readonly IAllRepo<SanPham> _sanPhamRes;
        private readonly IAllRepo<MauSac> _mauSacRes;
        private readonly IAllRepo<XuatXu> _xuatXuRes;
        private readonly ISanPhamChiTietRespo _sanPhamChiTietRes;
        private readonly IAllRepo<Anh> _AnhRes;
        private readonly IMapper _mapper;

        public SanPhamChiTietController(IAllRepo<KichCo> kickcoRes, IAllRepo<ThuongHieu> thuongHieuRes, IAllRepo<LoaiGiay> loaiGiayRes, IAllRepo<KieuDeGiay> kieuDeGiayRes, IAllRepo<ChatLieu> chatLieuRes, IAllRepo<SanPham> sanPhamRes, IAllRepo<XuatXu> xuatXuRes, IAllRepo<MauSac> mauSacRes, ISanPhamChiTietRespo sanPhamChiTietRes, IMapper mapper, IAllRepo<Anh> anhRes)
        {
            _kickcoRes = kickcoRes;
            _thuongHieuRes = thuongHieuRes;
            _loaiGiayRes = loaiGiayRes;
            _kieuDeGiayRes = kieuDeGiayRes;
            _chatLieuRes = chatLieuRes;
            _sanPhamRes = sanPhamRes;
            _xuatXuRes = xuatXuRes;
            _mauSacRes = mauSacRes;
            _sanPhamChiTietRes = sanPhamChiTietRes;
            _mapper = mapper;
            _AnhRes = anhRes;
        }

        [HttpPost("check-add-or-update")]
        public async Task<ResponseCheckAddOrUpdate> CheckProductForAddOrUpdate(SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            var productDetails = (await _sanPhamChiTietRes.GetListAsync())
                .FirstOrDefault(x=>
                x.IdXuatXu == sanPhamChiTietDTO.IdXuatXu &&
                x.IdMauSac == sanPhamChiTietDTO.IdMauSac &&
                x.IdKichCo == sanPhamChiTietDTO.IdKichCo &&
                x.IdChatLieu == sanPhamChiTietDTO.IdChatLieu &&
                x.IdSanPham == sanPhamChiTietDTO.IdSanPham &&
                x.IdThuongHieu == sanPhamChiTietDTO.IdThuongHieu &&
                x.IdLoaiGiay == sanPhamChiTietDTO.IdLoaiGiay &&
                x.IdKieuDeGiay == sanPhamChiTietDTO.IdKieuDeGiay
                );

            if (productDetails != null)
            {
                var productDetaiDTOMap = _mapper.Map<SanPhamChiTietDTO>(productDetails);
                productDetaiDTOMap.DanhSachAnh = _AnhRes.GetAll()
                                .Where(img => img.TrangThai == 0 && img.IdSanPhamChiTiet == productDetaiDTOMap.IdChiTietSp)
                                .Select(x => x.Url)
                                .ToList();
                return new ResponseCheckAddOrUpdate() { Success = true, Data = productDetaiDTOMap };

            }
            return new ResponseCheckAddOrUpdate() { Success = false, Data = null };
        }

        private SanPhamChiTietViewModel CreateSanPhamChiTietVM(SanPhamChiTiet spChiTiet)
        {
            return new SanPhamChiTietViewModel()
            {
                ChatLieu = _chatLieuRes.GetAll().Where(x => x.TrangThai == 0).FirstOrDefault(x => x.IdChatLieu == spChiTiet.IdChatLieu)?.TenChatLieu,
                Day = spChiTiet.Day,
                GiaBan = spChiTiet.GiaBan,
                GiaNhap = spChiTiet.GiaNhap,
                IdChiTietSp = spChiTiet.IdChiTietSp,
                KichCo = _kickcoRes.GetAll().Where(x => x.TrangThai == 0).FirstOrDefault(x => x.IdKichCo == spChiTiet.IdKichCo)?.SoKichCo,
                KieuDeGiay = _kieuDeGiayRes.GetAll().Where(x => x.Trangthai == 0).FirstOrDefault(ki => ki.IdKieuDeGiay == spChiTiet.IdKieuDeGiay)?.TenKieuDeGiay,
                LoaiGiay = _loaiGiayRes.GetAll().Where(lo => lo.TrangThai == 0).FirstOrDefault(l => l.IdLoaiGiay == spChiTiet.IdLoaiGiay)?.TenLoaiGiay,
                MauSac = _mauSacRes.GetAll().Where(ma => ma.TrangThai == 0).FirstOrDefault(m => m.IdMauSac == spChiTiet.IdMauSac)?.TenMauSac,
                MoTa = spChiTiet.MoTa,
                SanPham = _sanPhamRes.GetAll().Where(s => s.Trangthai == 0).FirstOrDefault(sp => sp.IdSanPham == spChiTiet.IdSanPham)?.TenSanPham,
                SoLuongTon = spChiTiet.SoLuongTon,
                ThuongHieu = _thuongHieuRes.GetAll().Where(th => th.TrangThai == 0).FirstOrDefault(ite => ite.IdThuongHieu == spChiTiet.IdThuongHieu)?.TenThuongHieu,
                XuatXu = _xuatXuRes.GetAll().Where(xx => xx.TrangThai == 0).FirstOrDefault(it => it.IdXuatXu == spChiTiet.IdXuatXu)?.Ten,
                ListTenAnh = _AnhRes.GetAll().Where(a => a.IdSanPhamChiTiet == spChiTiet.IdChiTietSp && a.TrangThai == 0).Select(x => x.Url).ToList()
            };
        }

        [HttpGet("Get-List-SanPhamChiTietViewModel")]
        public async Task<List<SanPhamChiTietViewModel>> GetListSanPham()
        {
            return (await _sanPhamChiTietRes.GetListAsync())
                .Where(sp => sp.TrangThai == 0)
                .Select(item => CreateSanPhamChiTietVM(item)).ToList();
        }

        [HttpGet("Get-List-SanPhamChiTiet")]
        public async Task<List<SanPhamChiTiet>> GetListSanPhamChiTiet()
        {
            return (await _sanPhamChiTietRes.GetListAsync()).ToList();
        }

        [HttpGet("Get-SanPhamChiTiet/{id}")]
        public async Task<SanPhamChiTiet?> GetSanPham(string id)
        {
            return await _sanPhamChiTietRes.GetByKeyAsync(id);
        }

        [HttpPost("Creat-SanPhamChiTiet")]
        public async Task<ResponseCreataDTO> CreateSanPhamChiTiet(SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            var sanPhamChiTiet = _mapper.Map<SanPhamChiTiet>(sanPhamChiTietDTO);
            sanPhamChiTiet.IdChiTietSp = Guid.NewGuid().ToString();
            sanPhamChiTiet.Ma = !(await _sanPhamChiTietRes.GetListAsync()).Any() ? 
                "MASP1" : 
                "MASP" + ((await _sanPhamChiTietRes.GetListAsync()).Count() + 1);
            sanPhamChiTiet.TrangThai = 0;
            sanPhamChiTiet.TrangThaiSale = 0;
            return new ResponseCreataDTO() 
            { 
                Success = await _sanPhamChiTietRes.AddAsync(sanPhamChiTiet),
                IdChiTietSp = sanPhamChiTiet.IdChiTietSp 
            }; 

        }

        [HttpDelete("Delete-SanPhamChiTiet/{id}")]
        public async Task<bool> DeleteSanPham(string id)
        {
            var sanPhamChiTiet = await GetSanPham(id);
            if (sanPhamChiTiet != null)
            {
                sanPhamChiTiet.TrangThai = 1;
                return await _sanPhamChiTietRes.UpdateAsync(sanPhamChiTiet);
            }
            return false;
        }


        [HttpPut("Update-SanPhamChiTiet")]
        public async Task<bool> Update(SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            var spChiTiet = await GetSanPham(sanPhamChiTietDTO.IdChiTietSp!);
            if (spChiTiet != null)
            {
                _mapper.Map(sanPhamChiTietDTO, spChiTiet);
                return await _sanPhamChiTietRes.UpdateAsync(spChiTiet);
            }
            return false;
        }


    }
}
