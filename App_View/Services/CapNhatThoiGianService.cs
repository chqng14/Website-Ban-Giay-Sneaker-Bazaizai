using App_Data.DbContextt;
using App_View.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using static App_Data.Repositories.TrangThai;

namespace App_View.Services
{
    public class CapNhatThoiGianService
    {
        BazaizaiContext _dbContext = new BazaizaiContext();

        public CapNhatThoiGianService()
        {

            _dbContext = new BazaizaiContext();
        }

        public void CheckNgayKetThuc()
        {
            var ngayKetThucSale = _dbContext.khuyenMais
                .Where(p => p.NgayKetThuc <= DateTime.Now && p.TrangThai != 0)
                .ToList();

            foreach (var sale in ngayKetThucSale)
            {
                sale.TrangThai = 0;
            }
            _dbContext.SaveChanges();
        }
        public void CapNhatTrangThaiSaleDetail()
        {
            var lstHetHan = _dbContext.khuyenMais.Where(x => x.TrangThai == 0).ToList();
            var lstKMCT = _dbContext.khuyenMaiChiTiets.ToList();
            foreach(var a in lstHetHan)
            {
                foreach(var b in lstKMCT)
                {
                    if(b.IdKhuyenMai==a.IdKhuyenMai)
                    {
                        b.TrangThai= (int)TrangThaiSaleDetail.DangKhuyenMai;
                    }
                }
            }
            _dbContext.SaveChanges();
        }
        public void CapNhatGiaBanThucTe()
        {
            var lstKhuyenMaiDangHoatDong = _dbContext.khuyenMaiChiTiets.Where(x => x.TrangThai == (int)TrangThaiSaleDetail.DangKhuyenMai).ToList();
            var lstCTSP = _dbContext.sanPhamChiTiets.ToList();
            foreach (var ctsp in lstCTSP)
            {
                bool check = false;  

                foreach (var kmct in lstKhuyenMaiDangHoatDong)
                {
                    if (kmct.IdSanPhamChiTiet == ctsp.IdChiTietSp)
                    {
                        var giaThucTe = _dbContext.khuyenMaiChiTiets.Where(x => x.IdSanPhamChiTiet == ctsp.IdChiTietSp).ToList();
                        int[] mangKhuyenMai = new int[giaThucTe.Count()];
                        int temp = 0;
                        foreach (var khuyenMai in giaThucTe)
                        {
                            var a = _dbContext.khuyenMais.FirstOrDefault(x => x.IdKhuyenMai == khuyenMai.IdKhuyenMai);
                            mangKhuyenMai[temp] = Convert.ToInt32(a.MucGiam);
                            temp++;
                        }
                        ctsp.GiaThucTe = ctsp.GiaBan - (ctsp.GiaBan * mangKhuyenMai.Max()/100);
                        _dbContext.sanPhamChiTiets.Update(ctsp);
                        check = true;  
                        break;
                    }
                }


                if (!check)
                {
                    ctsp.GiaThucTe = ctsp.GiaBan;
                    _dbContext.sanPhamChiTiets.Update(ctsp);
                }
            }
            _dbContext.SaveChanges();
        }
    }
}
