using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly IAllRepo<NguoiDung> iNguoiDungRepos;
        private readonly IAllRepo<GioHang> iGioHangRepos;

        public NguoiDungController()
        {
            iNguoiDungRepos = new AllRepo<NguoiDung>();
            iGioHangRepos = new AllRepo<GioHang>();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = iNguoiDungRepos.GetAll();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<NguoiDung> GetUserById(string id)
        {
            var result = iNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdNguoiDung == id);
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(string IdChucVu, string TenNguoiDung, string SDT, int GioiTinh, DateTime NgaySinh, string Email, string TenDangNhap, string MatKhau, string AnhDaiDien)
        {
            NguoiDung nd = new NguoiDung();
            Random random = new Random();
            GioHang gh = new GioHang();
            nd.IdNguoiDung = Convert.ToString(Guid.NewGuid());
            nd.MaNguoiDung = "USER" + random.Next(100, 999).ToString();
            nd.TenNguoiDung = TenNguoiDung;
            nd.GioiTinh = GioiTinh;
            nd.NgaySinh = NgaySinh;
            nd.Email = Email;
            nd.SDT = SDT;
            nd.TenDangNhap = TenDangNhap;
            nd.MatKhau = MatKhau;
            nd.IdChucVu =IdChucVu;
            nd.AnhDaiDien = AnhDaiDien;
            nd.TrangThai = 0;
            gh.IdNguoiDung = nd.IdNguoiDung;
            gh.NgayTao = DateTime.Now;
            gh.TrangThai = 0;
            var result = iNguoiDungRepos.AddItem(nd);
            if (result) iGioHangRepos.AddItem(gh);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string IdNguoiDung, string MaNguoiDung, string IdChucVu, string TenNguoiDung, string SDT, int GioiTinh, DateTime NgaySinh, string Email, string TenDangNhap, string MatKhau, string AnhDaiDien, int TrangThai)
        {
            var nd = iNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdNguoiDung == IdNguoiDung);
            nd.MaNguoiDung = MaNguoiDung;
            nd.TenNguoiDung = TenNguoiDung;
            nd.GioiTinh = GioiTinh;
            nd.NgaySinh = NgaySinh;
            nd.SDT = SDT;
            nd.MatKhau = MatKhau;
            nd.Email = Email;
            nd.TenDangNhap = TenDangNhap;
            nd.AnhDaiDien = AnhDaiDien;
            nd.TrangThai = TrangThai;
            nd.IdChucVu = nd.IdChucVu;
            var result = iNguoiDungRepos.EditItem(nd);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var nd = iNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdNguoiDung == id);
            var result = iNguoiDungRepos.RemoveItem(nd);
            return Ok(result);
        }
        [HttpGet("[action]")]
        public async Task<NguoiDung> GetUserByName(string TenUser)
        {
            var nd = iNguoiDungRepos.GetAll().Where(c => c.TenNguoiDung.Contains(TenUser)).FirstOrDefault();
            return nd;
        }
    }
}
