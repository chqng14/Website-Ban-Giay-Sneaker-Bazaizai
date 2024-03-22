using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
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
        private readonly IAllRepo<KhuyenMai> KMRepos;
        private readonly IAllRepo<KhuyenMaiChiTiet> KmctRepos;
        private readonly IAllRepo<SanPhamChiTiet> SpctRepos;
        private readonly ISanPhamChiTietRespo _sanPhamChiTietRes;
        private readonly IMapper _mapper;
        DbSet<Voucher> Vouchers;
        DbSet<VoucherNguoiDung> voucherNguoiDung;
        BazaizaiContext _dbContext = new BazaizaiContext();
        bool loading = false;
        public AutoUpdateController(IMapper mapper, IAllRepo<KhuyenMai> kMRepos, IAllRepo<KhuyenMaiChiTiet> kmctRepos, IAllRepo<SanPhamChiTiet> spctRepos, ISanPhamChiTietRespo sanPhamChiTietRes)
        {
            voucherNguoiDung = _dbContext.VoucherNguoiDungs;
            AllRepo<VoucherNguoiDung> VcNd = new AllRepo<VoucherNguoiDung>(_dbContext, voucherNguoiDung);
            VcNguoiDungRepos = VcNd;
            Vouchers = _dbContext.Vouchers;
            AllRepo<Voucher> all = new AllRepo<Voucher>(_dbContext, Vouchers);
            VoucherRepo = all;
            _mapper = mapper;
            _dbContext = new BazaizaiContext();
            KMRepos = kMRepos;
            KmctRepos = kmctRepos;
            SpctRepos = spctRepos;
            _sanPhamChiTietRes = sanPhamChiTietRes;
        }
        [HttpPut("CapNhatThongTinKhuyenMai")]
        public void CapNhatThongTinKhuyenMai()
        {
            // Hàm CheckNgayKetThucKhuyenMai
            var ngayKetThucSale = KMRepos.GetAll()
                .Where(p => p.NgayKetThuc <= DateTime.Now && p.TrangThai != (int)TrangThaiSale.HetHan)
                .ToList();
            var ngaySale = KMRepos.GetAll()
                .Where(p => p.NgayKetThuc >= DateTime.Now && p.NgayBatDau<= DateTime.Now&&(  p.TrangThai == (int)TrangThaiSale.HetHan|| p.TrangThai==(int)TrangThaiSale.ChuaBatDau) && p.TrangThai != (int)TrangThaiSale.BuocDung)
                .ToList();
            var saleChuaBatDau = KMRepos.GetAll()
               .Where(p => p.NgayBatDau >= DateTime.Now)
               .ToList();
            foreach (var sale in ngayKetThucSale)
            {
                sale.TrangThai = (int)TrangThaiSale.HetHan;
                KMRepos.EditItem(sale);
            }
            foreach (var sale in ngaySale)
            {
                sale.TrangThai = (int)TrangThaiSale.DangBatDau;
                KMRepos.EditItem(sale);
            }
            foreach (var sale in saleChuaBatDau)
            {
                sale.TrangThai = (int)TrangThaiSale.ChuaBatDau;
                KMRepos.EditItem(sale);
            }

            // Hàm CapNhatTrangThaiSaleDetail
            var lstHetHan = KMRepos.GetAll().Where(x => x.TrangThai == (int)TrangThaiSale.HetHan || x.TrangThai == (int)TrangThaiSale.BuocDung).ToList();
            var lstDangKhuyenMai = KMRepos.GetAll().Where(x => x.TrangThai == (int)TrangThaiSale.DangBatDau).ToList();
            var lstSapKhuyenMai = KMRepos.GetAll().Where(x => x.TrangThai == (int)TrangThaiSale.ChuaBatDau).ToList();
            var lstKMCT = KmctRepos.GetAll();
            foreach (var a in lstHetHan)
            {
                foreach (var b in lstKMCT)
                {
                    if (b.IdKhuyenMai == a.IdKhuyenMai)
                    {
                        b.TrangThai = (int)TrangThaiSaleDetail.NgungKhuyenMai;
                        KmctRepos.EditItem(b);
                    }
                }
            }
            foreach (var a in lstSapKhuyenMai)
            {
                foreach (var b in lstKMCT)
                {
                    if (b.IdKhuyenMai == a.IdKhuyenMai)
                    {
                        b.TrangThai = (int)TrangThaiSaleDetail.SapKhuyenMai;
                        KmctRepos.EditItem(b);
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
                        var spct = SpctRepos.GetAll().Where(x => x.IdChiTietSp == b.IdSanPhamChiTiet);
                        if (spct.Any())
                        {
                            foreach (var c in spct)
                            {
                                c.TrangThaiSale = (int)TrangThaiSaleInProductDetail.DaApDungSale;
                                SpctRepos.EditItem(c);
                            }
                        }
                        KmctRepos.EditItem(b);
                    }
                }
            }

            // Hàm CapNhatGiaBanThucTe
            var KhuyenMaiCTs = KmctRepos.GetAll().ToList();
            var KhuyenMais = KMRepos.GetAll().ToList();
            var lstKhuyenMaiDangHoatDong = KmctRepos.GetAll().Where(x => x.TrangThai == (int)TrangThaiSaleDetail.DangKhuyenMai).ToList();
            var giohang = _dbContext.GioHangChiTiets.ToList();
            var lstCTSP = SpctRepos.GetAll().Where(x => x.TrangThaiSale == (int)TrangThaiSaleInProductDetail.DaApDungSale && x.TrangThai == (int)TrangThaiCoBan.HoatDong).ToList();
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
                            var a = KhuyenMais.FirstOrDefault(x => x.IdKhuyenMai == khuyenMai.IdKhuyenMai);
                            if (a.LoaiHinhKM == 1)
                            {
                                mangKhuyenMai.Add(Convert.ToInt32(a.MucGiam));
                            }
                            else
                            {
                                mangKhuyenMaiDongGia.Add(Convert.ToInt32(a.MucGiam));
                            }
                        }
                        if (mangKhuyenMai.Count > 0&& mangKhuyenMaiDongGia.Count>0)
                        {
							int tempDongGia = 0;
							if ((ctsp.GiaBan - (ctsp.GiaBan * mangKhuyenMai.Max() / 100)) > mangKhuyenMaiDongGia.Max())
							{
							
								for (var i = 0; i < mangKhuyenMaiDongGia.Count; i++)
								{
									if (ctsp.GiaThucTe > mangKhuyenMaiDongGia[i])
									{
										ctsp.GiaThucTe = mangKhuyenMaiDongGia[i];
										tempDongGia++;
									}
								}
							}
							if (tempDongGia == 0)
							{
								ctsp.GiaThucTe = ctsp.GiaBan - (ctsp.GiaBan * mangKhuyenMai.Max() / 100);
							}
						}
                        else if (mangKhuyenMai.Count > 0)
                        {
							ctsp.GiaThucTe = ctsp.GiaBan - (ctsp.GiaBan * mangKhuyenMai.Max() / 100);
						}
                        else
                        {
                            var tempDongGia = 0;
                            ctsp.GiaThucTe = ctsp.GiaBan;
							for (var i = 0; i < mangKhuyenMaiDongGia.Count; i++)
							{
								if (ctsp.GiaThucTe > mangKhuyenMaiDongGia[i])
								{
									ctsp.GiaThucTe = mangKhuyenMaiDongGia[i];
                                    tempDongGia++;
								}
							}
                            if (tempDongGia == 0)
                            {
                                ctsp.GiaThucTe = ctsp.GiaBan;
                                var upDateKmct = lstKhuyenMaiDangHoatDong.FirstOrDefault(x => x.IdSanPhamChiTiet == ctsp.IdChiTietSp);

                                KmctRepos.RemoveItem(upDateKmct);
							}
						}
                        var gioHangChiTiet = giohang.Where(x => x.IdSanPhamCT == ctsp.IdChiTietSp).ToList();
                        foreach (var item in gioHangChiTiet)
                        {
                            item.GiaBan = ctsp.GiaThucTe;
                        }
                        
                    }
                    else
                    {
                        ctsp.GiaThucTe = ctsp.GiaBan;
                        ctsp.TrangThaiSale = (int)TrangThaiSaleInProductDetail.DuocApDungSale;
                        var gioHangChiTiet = giohang.Where(x => x.IdSanPhamCT == ctsp.IdChiTietSp).ToList();
                        foreach (var item in gioHangChiTiet)
                        {
                            item.GiaBan = ctsp.GiaThucTe;
                        }
                        
                    }

                }
                _dbContext.SanPhamChiTiets.UpdateRange(lstCTSP);
                _dbContext.GioHangChiTiets.UpdateRange(giohang);
                _dbContext.SaveChanges();
            }
        }


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
