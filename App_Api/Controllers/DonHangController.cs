using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.ViewModels.DonHang;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly BazaizaiContext _bazaizaiContext;
        public DonHangController()
        {
            _bazaizaiContext = new BazaizaiContext();
        }


        [HttpGet("DonHangs")]
        public async Task<List<DonHangViewModel>> GetDonHangs(string idNguoiDung)
        {
            var lstModelHoaDon = await _bazaizaiContext.HoaDons.Where(hd=>hd.IdNguoiDung == idNguoiDung).AsNoTracking().ToListAsync();
            return lstModelHoaDon.Select(hd => new DonHangViewModel()
            {
                IdDonHang = hd.IdHoaDon,
                MaDonHang = hd.MaHoaDon,
                NgayTao = hd.NgayTao.GetValueOrDefault().ToString("dd-MM-yyyy"),
                PhiShip = hd.TienShip.GetValueOrDefault(),
                TongTien = hd.TongTien.GetValueOrDefault(),
                TrangThaiHoaDon = hd.TrangThaiGiaoHang.GetValueOrDefault(),
                NgayGiaoDuKien = hd.NgayGiaoDuKien.GetValueOrDefault().ToString("dd-MM-yyyy"),
            }).ToList();
        }

    }
}
