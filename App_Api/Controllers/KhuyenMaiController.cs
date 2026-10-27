using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.KhuyenMaiDTO;
using App_Api.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private const string PromotionImageFolder = "AnhSale";
        private readonly IAllRepo<KhuyenMai> repos;
        private readonly BazaizaiContext context;
        private readonly IImageStorage _imageStorage;

        public KhuyenMaiController(
            IAllRepo<KhuyenMai> repository,
            BazaizaiContext dbContext,
            IImageStorage imageStorage)
        {
            repos = repository;
            context = dbContext;
            _imageStorage = imageStorage;
        }
        [HttpGet]
        public IEnumerable<KhuyenMai> GetAllKhuyenMai()
        {
            return repos.GetAll();
        }

        [HttpPost("Create-KhuyenMai")]

        public async Task<IActionResult> CreateKhuyenMaiAsync([FromForm] KhuyenMaiDTO khuyenMai, IFormFile formFile)
        {
            try
            {
                string MaTS;
                if (repos.GetAll().Count() == 0)
                {
                    MaTS = "KM1";
                }
                else
                {
                    MaTS = "KM" + (repos.GetAll().Count() + 1);
                }
                if (formFile is null || formFile.Length == 0)
                {
                    return BadRequest("Ảnh khuyến mãi là bắt buộc.");
                }

                var fileName = await _imageStorage.SaveAsync(formFile, PromotionImageFolder);
                    KhuyenMai KhuyenMai = new KhuyenMai();
                    KhuyenMai.TenKhuyenMai = khuyenMai.TenKhuyenMai;
                    KhuyenMai.MaKhuyenMai = MaTS;
                    KhuyenMai.IdKhuyenMai = khuyenMai.IdKhuyenMai;
                    KhuyenMai.TrangThai = khuyenMai.TrangThai;
                    KhuyenMai.NgayBatDau = khuyenMai.NgayBatDau;
                    KhuyenMai.NgayKetThuc = khuyenMai.NgayKetThuc;
                    KhuyenMai.MucGiam = khuyenMai.MucGiam;
                    KhuyenMai.LoaiHinhKM = khuyenMai.LoaiHinhKM;
                    KhuyenMai.Url = fileName;

                if (!repos.AddItem(KhuyenMai))
                {
                    _imageStorage.Delete(PromotionImageFolder, fileName);
                    return StatusCode(500, "Không thể lưu khuyến mãi.");
                }
                return Ok();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditKhuyenMaiAsync([FromForm] KhuyenMaiDTO khuyenMai, IFormFile? formFile)
        {
            if (formFile is { Length: > 0 })
            {
                var fileName = await _imageStorage.SaveAsync(formFile, PromotionImageFolder);
                var KhuyenMai = repos.GetAll().First(p => p.IdKhuyenMai == khuyenMai.IdKhuyenMai);
                var oldFileName = KhuyenMai.Url;
                KhuyenMai.TenKhuyenMai = khuyenMai.TenKhuyenMai;
                KhuyenMai.TrangThai = khuyenMai.TrangThai;
                KhuyenMai.NgayBatDau = khuyenMai.NgayBatDau;
                KhuyenMai.NgayKetThuc = khuyenMai.NgayKetThuc;
                KhuyenMai.MucGiam = khuyenMai.MucGiam;
                KhuyenMai.LoaiHinhKM = khuyenMai.LoaiHinhKM;
                KhuyenMai.Url = fileName;
                repos.EditItem(KhuyenMai);
                if (!string.IsNullOrWhiteSpace(oldFileName))
                {
                    _imageStorage.Delete(PromotionImageFolder, oldFileName);
                }
            }
            else 
            {
                var KhuyenMai = await context.KhuyenMais.FindAsync(khuyenMai.IdKhuyenMai);
                KhuyenMai.TenKhuyenMai = khuyenMai.TenKhuyenMai;
                KhuyenMai.TrangThai = khuyenMai.TrangThai;
                KhuyenMai.NgayBatDau = khuyenMai.NgayBatDau;
                KhuyenMai.NgayKetThuc = khuyenMai.NgayKetThuc;
                KhuyenMai.MucGiam = khuyenMai.MucGiam;
                KhuyenMai.LoaiHinhKM = khuyenMai.LoaiHinhKM;
                repos.EditItem(KhuyenMai);
            }
            return Ok();
        }
        [HttpPut("EditNoiImage")]
        public async Task<IActionResult> EditKhuyenMai([FromForm] KhuyenMaiDTO khuyenMai)
        {
                var KhuyenMai = await context.KhuyenMais.FindAsync(khuyenMai.IdKhuyenMai);
            KhuyenMai.TenKhuyenMai = khuyenMai.TenKhuyenMai;
            KhuyenMai.TrangThai = khuyenMai.TrangThai;
            KhuyenMai.NgayBatDau = khuyenMai.NgayBatDau;
            KhuyenMai.NgayKetThuc = khuyenMai.NgayKetThuc;
            KhuyenMai.MucGiam = khuyenMai.MucGiam;
            KhuyenMai.LoaiHinhKM = khuyenMai.LoaiHinhKM;
            repos.EditItem(KhuyenMai);
            return Ok();
        }

        [HttpDelete("{id}")]
        public bool DeleteKhuyenMai(string id)
        {
            var KhuyenMai = repos.GetAll().First(p => p.IdKhuyenMai == id);
            return repos.RemoveItem(KhuyenMai);
        }
    }
}
