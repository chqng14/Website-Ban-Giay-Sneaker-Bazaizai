using App_Data.DbContextt;
using App_Data.Repositories;
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
                .Where(p => p.NgayKetThuc <= DateTime.Now && p.TrangThai != (int)TrangThaiSale.HetHan)
                .ToList();

            foreach (var sale in ngayKetThucSale)
            {
                sale.TrangThai = (int)TrangThaiSale.HetHan;
            }
            _dbContext.SaveChanges();
        }
        public void CapNhatTrangThaiSaleDetail()
        {
            var lstHetHan = _dbContext.khuyenMais.Where(x => x.TrangThai == (int)TrangThaiSale.HetHan).ToList();
            var lstDangBatDau = _dbContext.khuyenMais.Where(x => x.TrangThai == (int)TrangThaiSale.DangBatDau).ToList();
            var lstDangKhuyenMai = _dbContext.khuyenMaiChiTiets.Where(x=>x.TrangThai==(int)TrangThaiSaleDetail.DangKhuyenMai).ToList();
            var lstNgungKhuyenMai = _dbContext.khuyenMaiChiTiets.Where(x => x.TrangThai == (int)TrangThaiSaleDetail.NgungKhuyenMai).ToList();
            foreach (var a in lstHetHan)
            {
                foreach(var b in lstDangKhuyenMai)
                {
                    if(b.IdKhuyenMai==a.IdKhuyenMai)
                    {
                        b.TrangThai= (int)TrangThaiSaleDetail.NgungKhuyenMai;
                    }
                }
            }
            foreach (var a in lstDangBatDau)
            {
                foreach (var b in lstNgungKhuyenMai)
                {
                    if (b.IdKhuyenMai == a.IdKhuyenMai)
                    {
                        b.TrangThai = (int)TrangThaiSaleDetail.DangKhuyenMai;
                    }
                }
            }
            _dbContext.SaveChanges();
        }
        public void CapNhatGiaBanThucTe()
        {
            var KhuyenMaiCTs = _dbContext.khuyenMaiChiTiets.AsNoTracking().ToList();
            var khuyenMais = _dbContext.khuyenMais.AsNoTracking().ToList();
            var lstKhuyenMaiDangHoatDong = _dbContext.khuyenMaiChiTiets.Where(x => x.TrangThai == (int)TrangThaiSaleDetail.DangKhuyenMai).AsNoTracking().ToList();
            var lstCTSP = _dbContext.sanPhamChiTiets.Where(x => x.TrangThaiSale == (int)TrangThaiSaleInProductDetail.DaApDungSale).ToList();
            if (lstCTSP != null && lstCTSP.Count() > 0)
            {
                foreach (var ctsp in lstCTSP)
                {
                    //bool check = false;
                    //    foreach (var kmct in lstKhuyenMaiDangHoatDong)
                    //{
                    //    if (kmct.IdSanPhamChiTiet == ctsp.IdChiTietSp)
                    //    {

                    //        int[] mangKhuyenMai = new int[giaThucTe.Count()];
                    //        int temp = 0;
                    //        foreach (var khuyenMai in giaThucTe)
                    //        {
                    //            var a = _dbContext.khuyenMais.FirstOrDefault(x => x.IdKhuyenMai == khuyenMai.IdKhuyenMai);
                    //            mangKhuyenMai[temp] = Convert.ToInt32(a.MucGiam);
                    //            temp++;
                    //        }
                    //        ctsp.GiaThucTe = ctsp.GiaBan - (ctsp.GiaBan * mangKhuyenMai.Max()/100);
                    //        _dbContext.sanPhamChiTiets.Update(ctsp);

                    //        //check = true;
                    //        break;
                    //    }
                    //if (!check)
                    //{
                    //    ctsp.GiaThucTe = ctsp.GiaBan;
                    //    ctsp.TrangThaiSale = (int)TrangThaiSaleInProductDetail.DuocApDungSale;
                    //    _dbContext.sanPhamChiTiets.Update(ctsp);

                    //}
                    var giaThucTe = lstKhuyenMaiDangHoatDong.Where(x => x.IdSanPhamChiTiet == ctsp.IdChiTietSp).ToList();

                    if (giaThucTe.Any())
                    {

                        int[] mangKhuyenMai = new int[giaThucTe.Count()];
                        int temp = 0;
                        foreach (var khuyenMai in giaThucTe)
                        {
                            var a = khuyenMais.FirstOrDefault(x => x.IdKhuyenMai == khuyenMai.IdKhuyenMai);
                            mangKhuyenMai[temp] = Convert.ToInt32(a.MucGiam);
                            temp++;
                        }
                        ctsp.GiaThucTe = ctsp.GiaBan - (ctsp.GiaBan * mangKhuyenMai.Max() / 100);
                        //_dbContext.sanPhamChiTiets.Update(ctsp);
                        //_dbContext.SaveChanges();
                    }
                    else
                    {
                        ctsp.GiaThucTe = ctsp.GiaBan;
                        ctsp.TrangThaiSale = (int)TrangThaiSaleInProductDetail.DuocApDungSale;
                        //_dbContext.sanPhamChiTiets.Update(ctsp);
                        //_dbContext.SaveChanges();
                    }
                }

                _dbContext.sanPhamChiTiets.UpdateRange(lstCTSP);
                _dbContext.SaveChanges();
            }
        }
    }
}
