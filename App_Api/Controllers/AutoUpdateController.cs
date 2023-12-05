using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static App_Data.Repositories.TrangThai;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoUpdateController : ControllerBase
    {
        private readonly IAllRepo<VoucherNguoiDung> VcNguoiDungRepos;
        private readonly IAllRepo<Voucher> VoucherRepo;
        private readonly IMapper _mapper;
        DbSet<Voucher> vouchers;
        DbSet<VoucherNguoiDung> voucherNguoiDung;
        BazaizaiContext _dbContext = new BazaizaiContext();
        bool loading = false;
        public AutoUpdateController(IMapper mapper)
        {
            voucherNguoiDung = _dbContext.voucherNguoiDungs;
            AllRepo<VoucherNguoiDung> VcNd = new AllRepo<VoucherNguoiDung>(_dbContext, voucherNguoiDung);
            VcNguoiDungRepos = VcNd;
            vouchers = _dbContext.vouchers;
            AllRepo<Voucher> all = new AllRepo<Voucher>(_dbContext, vouchers);
            VoucherRepo = all;
            _mapper = mapper;
            _dbContext = new BazaizaiContext();
        }
        #region Sale
        [HttpPut("CheckNgayKetThucKhuyenMai")]
        public void CheckNgayKetThuc()
        {
            var ngayKetThucSale = _dbContext.khuyenMais
                .Where(p => p.NgayKetThuc <= DateTime.Now && p.TrangThai != (int)TrangThaiSale.HetHan)
                .ToList();
            var ngaySale = _dbContext.khuyenMais
                .Where(p => p.NgayKetThuc >= DateTime.Now && p.TrangThai == (int)TrangThaiSale.HetHan && p.TrangThai != (int)TrangThaiSale.BuocDung)
                .ToList();
            var saleChuaBatDau = _dbContext.khuyenMais
               .Where(p => p.NgayBatDau >= DateTime.Now)
               .ToList();
            foreach (var sale in ngayKetThucSale)
            {
                sale.TrangThai = (int)TrangThaiSale.HetHan;
            }
            foreach (var sale in ngaySale)
            {
                sale.TrangThai = (int)TrangThaiSale.DangBatDau;
            }
            foreach (var sale in saleChuaBatDau)
            {
                sale.TrangThai = (int)TrangThaiSale.ChuaBatDau;
            }
            _dbContext.SaveChanges();
        }
        [HttpPut("CapNhatTrangThaiSaleDetail")]
        public void CapNhatTrangThaiSaleDetail()
        {
            var lstHetHan = _dbContext.khuyenMais.Where(x => x.TrangThai == (int)TrangThaiSale.HetHan || x.TrangThai == (int)TrangThaiSale.BuocDung).ToList();
            var lstDangKhuyenMai = _dbContext.khuyenMais.Where(x => x.TrangThai == (int)TrangThaiSale.DangBatDau).ToList();
            var lstKMCT = _dbContext.khuyenMaiChiTiets.ToList();
            foreach (var a in lstHetHan)
            {
                foreach (var b in lstKMCT)
                {
                    if (b.IdKhuyenMai == a.IdKhuyenMai)
                    {
                        b.TrangThai = (int)TrangThaiSaleDetail.NgungKhuyenMai;
                    }
                }
            }
            foreach (var a in lstDangKhuyenMai)
            {
                foreach (var b in lstKMCT)
                {
                    if (b.IdKhuyenMai == a.IdKhuyenMai)
                    {
                        b.TrangThai = (int)TrangThaiSaleDetail.DangKhuyenMai;
                        var spct = _dbContext.sanPhamChiTiets.Where(x => x.IdChiTietSp == b.IdSanPhamChiTiet);
                        if (spct.Any())
                        {
                            foreach (var c in spct)
                            {
                                c.TrangThaiSale = (int)TrangThaiSaleInProductDetail.DaApDungSale;
                            }
                            _dbContext.sanPhamChiTiets.UpdateRange(spct);
                        }
                    }
                }
            }
            _dbContext.SaveChanges();
            loading = false;


        }
        [HttpPut("CapNhatGiaBanThucTe")]
        public void CapNhatGiaBanThucTe()
        {
            var KhuyenMaiCTs = _dbContext.khuyenMaiChiTiets.AsNoTracking().ToList();
            var khuyenMais = _dbContext.khuyenMais.AsNoTracking().ToList();
            var lstKhuyenMaiDangHoatDong = _dbContext.khuyenMaiChiTiets.Where(x => x.TrangThai == (int)TrangThaiSaleDetail.DangKhuyenMai).AsNoTracking().ToList();
            var giohang = _dbContext.gioHangChiTiets.ToList();
            var lstCTSP = _dbContext.sanPhamChiTiets.Where(x => x.TrangThaiSale == (int)TrangThaiSaleInProductDetail.DaApDungSale && x.TrangThai == (int)TrangThaiCoBan.HoatDong).ToList();
            if (lstCTSP != null && lstCTSP.Count() > 0)
            {
                foreach (var ctsp in lstCTSP)
                {
                    var giaThucTe = lstKhuyenMaiDangHoatDong.Where(x => x.IdSanPhamChiTiet == ctsp.IdChiTietSp).ToList();

                    if (giaThucTe.Any())
                    {

                        List<int> mangKhuyenMai = new List<int>();
                        List<int> mangKhuyenMaiDongGia = new List<int>();
                        foreach (var khuyenMai in giaThucTe)
                        {
                            var a = khuyenMais.FirstOrDefault(x => x.IdKhuyenMai == khuyenMai.IdKhuyenMai);
                            if (a.LoaiHinhKM == 1)
                            {
                                mangKhuyenMai.Add(Convert.ToInt32(a.MucGiam));
                            }
                            else
                            {
                                mangKhuyenMaiDongGia.Add(Convert.ToInt32(a.MucGiam));
                            }
                        }
                        if (mangKhuyenMaiDongGia.Count > 0)
                        {
                            ctsp.GiaThucTe = mangKhuyenMaiDongGia.Max();
                        }
                        else
                        {
                            ctsp.GiaThucTe = ctsp.GiaBan - (ctsp.GiaBan * mangKhuyenMai.Max() / 100);
                        }
                        var gioHangChiTiet = giohang.Where(x => x.IdSanPhamCT == ctsp.IdChiTietSp).ToList();
                        foreach (var item in gioHangChiTiet)
                        {
                            item.GiaBan = ctsp.GiaThucTe;
                        }
                        //_dbContext.sanPhamChiTiets.Update(ctsp);
                        //_dbContext.SaveChanges();
                    }
                    else
                    {
                        ctsp.GiaThucTe = ctsp.GiaBan;
                        ctsp.TrangThaiSale = (int)TrangThaiSaleInProductDetail.DuocApDungSale;
                        //_dbContext.sanPhamChiTiets.Update(ctsp);
                        //_dbContext.SaveChanges();
                        var gioHangChiTiet = giohang.Where(x => x.IdSanPhamCT == ctsp.IdChiTietSp).ToList();
                        foreach (var item in gioHangChiTiet)
                        {
                            item.GiaBan = ctsp.GiaThucTe;
                        }
                    }

                }
                _dbContext.gioHangChiTiets.UpdateRange(giohang);
                _dbContext.sanPhamChiTiets.UpdateRange(lstCTSP);
                _dbContext.SaveChanges();
            }
        }
        //_dbContext.SaveChanges();
        #endregion
        #region VoucherOnline
        [HttpPut("CapNhatVoucherHetHanOnline")]
        public void CapNhatVoucherHetHanOnline()
        {
            var VoucherCanCapNhat = VoucherRepo.GetAll().Where(c => c.NgayKetThuc < DateTime.Now && c.TrangThai == (int)TrangThaiVoucher.HoatDong).ToList();
            if (VoucherCanCapNhat.Count > 0)
            {
                foreach (var voucher in VoucherCanCapNhat)
                {
                    voucher.TrangThai = (int)TrangThaiVoucher.KhongHoatDong;
                    VoucherRepo.EditItem(voucher);
                }
            }
        }
        [HttpPut("CapNhatVoucherDenHanOnline")]
        public void CapNhatVoucherDenHanOnline()
        {
            var VoucherCanCapNhat = VoucherRepo.GetAll().Where(c => c.NgayBatDau <= DateTime.Now && c.NgayKetThuc >= DateTime.Now && c.TrangThai == (int)TrangThaiVoucher.ChuaBatDau).ToList();
            if (VoucherCanCapNhat.Count > 0)
            {
                foreach (var voucher in VoucherCanCapNhat)
                {
                    voucher.TrangThai = (int)TrangThaiVoucher.HoatDong;
                    VoucherRepo.EditItem(voucher);

                }
            }
        }
        [HttpPut("CapNhatVoucherNguoiDungOnline")]
        public void CapNhatVoucherNguoiDungOnline()
        {
            var VoucherKhongKhaDung = VoucherRepo.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucher.KhongHoatDong || c.TrangThai == (int)TrangThaiVoucher.DaHuy).ToList();
            if (VoucherKhongKhaDung.Count > 0)
            {
                var VoucherNguoiDungDangHoatDong = VcNguoiDungRepos.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucherNguoiDung.KhaDung).ToList();
                foreach (var voucher in VoucherKhongKhaDung)
                {
                    var VoucherNguoiDungCanSua = VoucherNguoiDungDangHoatDong.Where(c => c.IdVouCher == voucher.IdVoucher).ToList();
                    foreach (var user in VoucherNguoiDungCanSua)
                    {
                        user.TrangThai = (int)TrangThaiVoucherNguoiDung.HetHieuLuc;
                        VcNguoiDungRepos.EditItem(user);
                    }
                }
            }
        }
        #endregion
        #region VoucherTaiQuay
        [HttpPut("CapNhatVoucherHetHanTaiQuay")]
        public void CapNhatVoucherHetHanTaiQuay()
        {
            // Lấy danh sách voucher cần cập nhật với điều kiện hết hạn và trạng thái hoặc không hoạt động tại quầy
            var VoucherCanCapNhat = VoucherRepo.GetAll()
                .Where(c => c.NgayKetThuc < DateTime.Now && (c.TrangThai == (int)TrangThaiVoucher.HoatDongTaiQuay))
                .ToList();

            // Kiểm tra nếu có voucher cần cập nhật
            if (VoucherCanCapNhat.Count > 0)
            {
                // Lặp qua từng voucher cần cập nhật và đặt trạng thái là Hết hạn tại quầy
                foreach (var voucher in VoucherCanCapNhat)
                {
                    voucher.TrangThai = (int)TrangThaiVoucher.KhongHoatDongTaiQuay;
                    VoucherRepo.EditItem(voucher);
                }
            }
        }
        [HttpPut("CapNhatVoucherDenHanTaiQuay")]
        public void CapNhatVoucherDenHanTaiQuay()
        {
            var VoucherNeedUpdate = VoucherRepo.GetAll().Where(c => c.NgayBatDau <= DateTime.Now && c.NgayKetThuc > DateTime.Now && c.TrangThai == (int)TrangThaiVoucher.ChuaHoatDongTaiQuay).ToList();
            if (VoucherNeedUpdate.Count > 0)
            {
                foreach (var voucher in VoucherNeedUpdate)
                {
                    voucher.TrangThai = (int)TrangThaiVoucher.HoatDongTaiQuay;
                    VoucherRepo.EditItem(voucher);
                }
            }
        }
        [HttpPut("CapNhatVoucherNguoiDungTaiQuay")]
        public void CapNhatVoucherNguoiDungTaiQuay()
        {
            var VoucherKhongKhaDung = VoucherRepo.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucher.KhongHoatDongTaiQuay || c.TrangThai == (int)TrangThaiVoucher.DaHuyTaiQuay).ToList();
            if (VoucherKhongKhaDung.Count > 0)
            {
                var VoucherNguoiDungDangHoatDong = VcNguoiDungRepos.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucherNguoiDung.KhaDung).ToList();
                foreach (var voucher in VoucherKhongKhaDung)
                {
                    var VoucherNguoiDungCanSua = VoucherNguoiDungDangHoatDong.Where(c => c.IdVouCher == voucher.IdVoucher).ToList();
                    foreach (var user in VoucherNguoiDungCanSua)
                    {
                        user.TrangThai = (int)TrangThaiVoucherNguoiDung.HetHieuLuc;
                        VcNguoiDungRepos.EditItem(user);
                    }
                }
            }
        }
        [HttpPut("CapNhatVoucherNguoiDungTaiQuayKhiVoucherHoatDong")]
        public void CapNhatVoucherNguoiDungTaiQuayKhiVoucherHoatDong()
        {
            var VoucherHoatDong = VoucherRepo.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucher.HoatDongTaiQuay).ToList();
            if (VoucherHoatDong.Count > 0)
            {
                var VoucherNguoiDungChuaBatDau = VcNguoiDungRepos.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucherNguoiDung.ChuaBatDau).ToList();
                foreach (var voucher in VoucherHoatDong)
                {
                    var VoucherNguoiDungCanSua = VoucherNguoiDungChuaBatDau.Where(c => c.IdVouCher == voucher.IdVoucher).ToList();
                    foreach (var user in VoucherNguoiDungCanSua)
                    {
                        user.TrangThai = (int)TrangThaiVoucherNguoiDung.KhaDung;
                        VcNguoiDungRepos.EditItem(user);
                    }
                }
            }
        }
        #endregion
        [HttpPut("CapNhatThoiGianVoucher")]
        public void CapNhatThoiGianVoucher()
        {
            #region VoucherOn
            // Update Voucher Online đã hết hạn
            var CapNhatVoucherHetHanOnline = VoucherRepo.GetAll().Where(c => c.NgayKetThuc < DateTime.Now && c.TrangThai == (int)TrangThaiVoucher.HoatDong).ToList();
            if (CapNhatVoucherHetHanOnline.Count > 0)
            {
                foreach (var voucher in CapNhatVoucherHetHanOnline)
                {
                    voucher.TrangThai = (int)TrangThaiVoucher.KhongHoatDong;
                    VoucherRepo.EditItem(voucher);
                }
            }
            // Update Voucher Online bắt đầu
            var CapNhatVoucherDenHanOnline = VoucherRepo.GetAll().Where(c => c.NgayBatDau <= DateTime.Now && c.NgayKetThuc >= DateTime.Now && c.TrangThai == (int)TrangThaiVoucher.ChuaBatDau).ToList();
            if (CapNhatVoucherDenHanOnline.Count > 0)
            {
                foreach (var voucher in CapNhatVoucherDenHanOnline)
                {
                    voucher.TrangThai = (int)TrangThaiVoucher.HoatDong;
                    VoucherRepo.EditItem(voucher);

                }
            }
            //Update voucher người dùng khi voucher hết hạn
            var VoucherOnlineKhongKhaDung = VoucherRepo.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucher.KhongHoatDong || c.TrangThai == (int)TrangThaiVoucher.DaHuy).ToList();
            if (VoucherOnlineKhongKhaDung.Count > 0)
            {
                var VoucherOnlineNguoiDungDangHoatDong = VcNguoiDungRepos.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucherNguoiDung.KhaDung).ToList();
                foreach (var voucher in VoucherOnlineKhongKhaDung)
                {
                    var VoucherNguoiDungCanSua = VoucherOnlineNguoiDungDangHoatDong.Where(c => c.IdVouCher == voucher.IdVoucher).ToList();
                    foreach (var user in VoucherNguoiDungCanSua)
                    {
                        user.TrangThai = (int)TrangThaiVoucherNguoiDung.HetHieuLuc;
                        VcNguoiDungRepos.EditItem(user);
                    }
                }
            }
            #endregion
            #region VoucherTaiQuay
            // Update Voucher tại quầy đã hết hạn      
            var VoucherTaiQuayCanCapNhat = VoucherRepo.GetAll()
                .Where(c => c.NgayKetThuc < DateTime.Now && (c.TrangThai == (int)TrangThaiVoucher.HoatDongTaiQuay))
                .ToList();
            if (VoucherTaiQuayCanCapNhat.Count > 0)
            {
                foreach (var voucher in VoucherTaiQuayCanCapNhat)
                {
                    voucher.TrangThai = (int)TrangThaiVoucher.KhongHoatDongTaiQuay;
                    VoucherRepo.EditItem(voucher);
                }
            }
            // Update Voucher tại quầy hoạt động
            var VoucherTaiCanCapNhat = VoucherRepo.GetAll().Where(c => c.NgayBatDau <= DateTime.Now && c.NgayKetThuc > DateTime.Now && c.TrangThai == (int)TrangThaiVoucher.ChuaHoatDongTaiQuay).ToList();
            if (VoucherTaiCanCapNhat.Count > 0)
            {
                foreach (var voucher in VoucherTaiCanCapNhat)
                {
                    voucher.TrangThai = (int)TrangThaiVoucher.HoatDongTaiQuay;
                    VoucherRepo.EditItem(voucher);
                }
            }
            //Update Voucher đã in sau khi voucher tại quầy hết hạn 
            var VoucherTaiQuayKhongKhaDung = VoucherRepo.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucher.KhongHoatDongTaiQuay || c.TrangThai == (int)TrangThaiVoucher.DaHuyTaiQuay).ToList();
            if (VoucherTaiQuayKhongKhaDung.Count > 0)
            {
                var VoucherDaInDangHoatDong = VcNguoiDungRepos.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucherNguoiDung.KhaDung).ToList();
                foreach (var voucher in VoucherTaiQuayKhongKhaDung)
                {
                    var VoucherDaInSauKhiHetHieuLuc = VoucherDaInDangHoatDong.Where(c => c.IdVouCher == voucher.IdVoucher).ToList();
                    foreach (var user in VoucherDaInSauKhiHetHieuLuc)
                    {
                        user.TrangThai = (int)TrangThaiVoucherNguoiDung.HetHieuLuc;
                        VcNguoiDungRepos.EditItem(user);
                    }
                }
            }
            // Update Voucher đã in sau khi voucher đến hạn
            var VoucherTaiQuayHoatDong = VoucherRepo.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucher.HoatDongTaiQuay).ToList();
            if (VoucherTaiQuayHoatDong.Count > 0)
            {
                var VoucherDaInChuaBatDau = VcNguoiDungRepos.GetAll().Where(c => c.TrangThai == (int)TrangThaiVoucherNguoiDung.ChuaBatDau).ToList();
                foreach (var voucher in VoucherTaiQuayHoatDong)
                {
                    var VoucherNguoiDungCanSua = VoucherDaInChuaBatDau.Where(c => c.IdVouCher == voucher.IdVoucher).ToList();
                    foreach (var user in VoucherNguoiDungCanSua)
                    {
                        user.TrangThai = (int)TrangThaiVoucherNguoiDung.KhaDung;
                        VcNguoiDungRepos.EditItem(user);
                    }
                }
            }

            #endregion
        }
    }
}
