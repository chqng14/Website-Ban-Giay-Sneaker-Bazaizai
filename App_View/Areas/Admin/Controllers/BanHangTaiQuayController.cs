using App_Data.DbContext;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.HoaDonChiTietDTO;
using App_Data.ViewModels.SanPhamChiTietDTO;
using App_View.IServices;
using App_View.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static App_Data.Repositories.TrangThai;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,NhanVien")]

    public class BanHangTaiQuayController : Controller
    {
        private readonly IHoaDonServices _hoaDonServices;
        private readonly ISanPhamChiTietservice _SanPhamChiTietservice;
        private readonly IVoucherNguoiDungservices _VoucherNguoiDungservices;
        private readonly IVoucherservices _Voucherservices;
        private readonly IHoaDonChiTietservices _HoaDonChiTietservices; private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        public BanHangTaiQuayController(ISanPhamChiTietservice SanPhamChiTietservice, IVoucherNguoiDungservices VoucherNguoiDungservices,IVoucherservices Voucherservices, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager)
        {
            HttpClient httpClient = new HttpClient();
            _hoaDonServices = new HoaDonServices();
            _SanPhamChiTietservice = SanPhamChiTietservice;
            _VoucherNguoiDungservices = VoucherNguoiDungservices;
            _Voucherservices = Voucherservices;
            _HoaDonChiTietservices = new HoaDonChiTietservices();
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> DanhSachHoaDonCho()
        {
            var listHoaDonCho = await _hoaDonServices.GetAllHoaDonCho();
            var listsanpham = await _SanPhamChiTietservice.GetDanhSachBienTheItemShopViewModelAsync();
            foreach (var item in listHoaDonCho)
            {
                foreach (var item2 in item.hoaDonChiTietDTOs)
                {
                    var sanpham = listsanpham.FirstOrDefault(c => c.IdChiTietSp == item2.IdSanPhamChiTiet);
                    item2.TenSanPham = sanpham.TenSanPham + "/" + sanpham.MauSac + "/" + sanpham.KichCo;
                    //item2.masanpham  = sanpham
                }
            }
            return View(listHoaDonCho.OrderBy(c => Convert.ToInt32(c.MaHoaDon.Substring(2, c.MaHoaDon.Length - 2))));
        }
        [HttpPost]
        public async Task<IActionResult> TaoHoaDonTaiQuay()
        {
            var user = await _userManager.GetUserAsync(User);
            var idUser = await _userManager.GetUserIdAsync(user);
            var hoaDonMoi = new HoaDon()
            {
                IdHoaDon = Guid.NewGuid().ToString(),
                IdNguoiDung = idUser,
                IdNguoiSuaGanNhat = idUser,
            };
            var newHoaDon = await _hoaDonServices.TaoHoaDonTaiQuay(hoaDonMoi);
            return Json(newHoaDon.MaHoaDon);
        }

        [HttpPost]
        public async Task<IActionResult> ThemSanPhamVaoHoaDon(string maHD, string idSanPham)
        {
            if (string.IsNullOrWhiteSpace(maHD))
            {
                return Ok(new { TrangThai = false, TrangThaiHang = false });
            }
            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(hd => hd.MaHoaDon == maHD);
            var sanPham = (await _SanPhamChiTietservice.GetDanhSachBienTheItemShopViewModelAsync()).FirstOrDefault(c => c.IdChiTietSp == idSanPham);
            if (sanPham.SoLuongTon == 0)
            {
                return Ok(new { TrangThai = false , TrangThaiHang = true });
            }
            var hoaDonChitiet = new HoaDonChiTiet()
            {
                IdHoaDon = hoaDon.Id,
                IdHoaDonChiTiet = Guid.NewGuid().ToString(),
                IdSanPhamChiTiet = idSanPham.ToString(),
                SoLuong = 1,
                TrangThai = (int)TrangThaiHoaDonChiTiet.ChoTaiQuay,
                GiaBan = sanPham.GiaThucTe,
                GiaGoc = sanPham.GiaGoc,
            };
            var hoaDonChiTietTraLai = await _HoaDonChiTietservices.ThemSanPhamVaoHoaDon(hoaDonChitiet);

            await _SanPhamChiTietservice.UpDatSoLuongAynsc(new SanPhamSoLuongDTO()
            {
                IdChiTietSanPham = sanPham.IdChiTietSp,
                SoLuong = 1
            });
            var tongTienThayDoi = hoaDonChiTietTraLai.GiaGoc;
            var soTienTraLaiThayDoi = hoaDonChiTietTraLai.GiaBan;
            var SoTienKhyenMaiGiam = tongTienThayDoi - soTienTraLaiThayDoi;
            return Ok(new
            {
                TrangThai = true,
                IdHoaDon = hoaDonChiTietTraLai.IdHoaDon,
                IdHoaDonChiTiet = hoaDonChiTietTraLai.IdHoaDonChiTiet,
                IdSanPhamChiTiet = hoaDonChiTietTraLai.IdSanPhamChiTiet,
                SoLuong = hoaDonChiTietTraLai.SoLuong,
                GiaBan = hoaDonChiTietTraLai.GiaBan,
                GiaGoc = hoaDonChiTietTraLai.GiaGoc,
                TenSanPham = sanPham.TenSanPham + "/" + sanPham.MauSac + "/" + sanPham.KichCo,
                MaSanPham = sanPham.MaSanPham,
                TongTienThayDoi = tongTienThayDoi,
                SoTienTraLaiThayDoi = soTienTraLaiThayDoi,
                SoTienKhyenMaiGiam = SoTienKhyenMaiGiam,
                SoTienVoucherGiam = 0,
            });
        }

        [HttpGet]
        public async Task<IActionResult> LoadPartialViewDanhSachSanPham(string tukhoa)
        {
            if (!string.IsNullOrWhiteSpace(tukhoa))
                return PartialView("_DanhSachSanPhamPartialView", (await _SanPhamChiTietservice.GetDanhSachBienTheItemShopViewModelAsync()).Where(c => c.TenSanPham.ToLower().Replace(" ", "").Contains(tukhoa.ToLower().Replace(" ", "")) || c.MaSanPham.ToLower().Replace(" ", "").Contains(tukhoa.ToLower().Replace(" ", ""))));
            else
                return PartialView("_DanhSachSanPhamPartialView", await _SanPhamChiTietservice.GetDanhSachBienTheItemShopViewModelAsync());
        }
        [HttpGet]
        public async Task<IActionResult> LoadPartialViewKhachHang(string tukhoa)
        {
            if (!string.IsNullOrWhiteSpace(tukhoa))
                return PartialView("_KhachHangPartialView", (await _hoaDonServices.GetKhachHangs()).Where(c => c.SDT.ToLower().Replace(" ", "").Contains(tukhoa.ToLower().Replace(" ", "")) || c.TenKhachHang.ToLower().Replace(" ", "").Contains(tukhoa.ToLower().Replace(" ", ""))));
            else
                return PartialView("_KhachHangPartialView", await _hoaDonServices.GetKhachHangs());
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateSoLuong(string maHD, string idSanPham, int SoLuongMoi)
        {
            if (string.IsNullOrWhiteSpace(maHD))
            {
                return Ok(new { TrangThai = false });
            }
            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(hd => hd.MaHoaDon == maHD);
            var sanPham = (await _SanPhamChiTietservice.GetDanhSachBienTheItemShopViewModelAsync()).FirstOrDefault(c => c.IdChiTietSp == idSanPham);

            var soLuongThayDoi = await _HoaDonChiTietservices.UpdateSoLuong(hoaDon.Id, idSanPham, SoLuongMoi, sanPham.SoLuongTon.ToString());
            if (soLuongThayDoi != "")
            {
                var tongTienThayDoi = Convert.ToInt32(soLuongThayDoi) * (double)hoaDon.hoaDonChiTietDTOs.FirstOrDefault(c => c.IdSanPhamChiTiet == idSanPham).GiaGoc;
                var soTienTraLaiThayDoi = Convert.ToInt32(soLuongThayDoi) * (double)hoaDon.hoaDonChiTietDTOs.FirstOrDefault(c => c.IdSanPhamChiTiet == idSanPham).GiaBan;
                var SoTienKhyenMaiGiam = tongTienThayDoi - soTienTraLaiThayDoi;
                await _SanPhamChiTietservice.UpDatSoLuongAynsc(new SanPhamSoLuongDTO()
                {
                    IdChiTietSanPham = idSanPham,
                    SoLuong = Convert.ToInt32(soLuongThayDoi)
                });
                if (Convert.ToInt32(sanPham.SoLuongTon) - Convert.ToInt32(soLuongThayDoi) != 0)
                {
                    return Ok(new
                    {
                        TrangThai = true,
                        SoLuongConLai = Convert.ToInt32(sanPham.SoLuongTon) - Convert.ToInt32(soLuongThayDoi),
                        TongTienThayDoi = tongTienThayDoi,
                        SoTienTraLaiThayDoi = soTienTraLaiThayDoi,
                        SoTienKhyenMaiGiam = SoTienKhyenMaiGiam,
                        SoTienVoucherGiam = 0,

                    });
                }
                else
                {
                    return Ok(new
                    {
                        TrangThai = true,
                        TongTienThayDoi = tongTienThayDoi,
                        SoTienTraLaiThayDoi = soTienTraLaiThayDoi,
                        SoTienKhyenMaiGiam = SoTienKhyenMaiGiam,
                        SoLuongConLai = "Hết hàng",
                        SoTienVoucherGiam = 0,

                    });
                }
            }
            else
            {
                var soLuongTrongHoaDon = hoaDon.hoaDonChiTietDTOs.FirstOrDefault(c => c.IdSanPhamChiTiet == idSanPham).SoLuong;
                if (sanPham.SoLuongTon > 0)
                    return Ok(new
                    {
                        TrangThai = false,
                        SoLuongConLai = sanPham.SoLuongTon,
                        SoLuongCu = soLuongTrongHoaDon,
                    });
                else
                    return Ok(new
                    {
                        TrangThai = false,
                        SoLuongConLai = "Hết hàng",
                        SoLuongCu = soLuongTrongHoaDon,
                    });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> XoaSanPhamKhoiHoaDon(string maHD, string idSanPham)
        {
            if (string.IsNullOrWhiteSpace(maHD))
            {
                return Ok(new { TrangThai = false });
            }
            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(hd => hd.MaHoaDon == maHD);
            var soLuongThayDoi = await _HoaDonChiTietservices.XoaSanPhamKhoiHoaDon(hoaDon.Id, idSanPham);
            var tongTienThayDoi = -Convert.ToInt32(soLuongThayDoi) * (double)hoaDon.hoaDonChiTietDTOs.FirstOrDefault(c => c.IdSanPhamChiTiet == idSanPham).GiaGoc;
            var soTienTraLaiThayDoi = -Convert.ToInt32(soLuongThayDoi) * (double)hoaDon.hoaDonChiTietDTOs.FirstOrDefault(c => c.IdSanPhamChiTiet == idSanPham).GiaBan;
            var SoTienKhyenMaiGiam = tongTienThayDoi - soTienTraLaiThayDoi;
            await _SanPhamChiTietservice.UpDatSoLuongAynsc(new SanPhamSoLuongDTO()
            {
                IdChiTietSanPham = idSanPham,
                SoLuong = (-Convert.ToInt32(soLuongThayDoi))
            });
            var sanPham = (await _SanPhamChiTietservice.GetDanhSachBienTheItemShopViewModelAsync()).FirstOrDefault(c => c.IdChiTietSp == idSanPham);
            return Ok(new
            {
                SoLuongConLai = sanPham.SoLuongTon,
                TongTienThayDoi = tongTienThayDoi,
                SoTienTraLaiThayDoi = soTienTraLaiThayDoi,
                SoTienKhyenMaiGiam = SoTienKhyenMaiGiam,
                SoTienVoucherGiam = 0,
            });
        }
        [HttpGet]
        public async Task<IActionResult> HoaDonDuocChon(string maHD)
        {
            if (string.IsNullOrWhiteSpace(maHD))
            {
                return Ok(new
                {
                    TongTienGoc = 0,
                    TienPhaiTra = 0,
                    SoTienKhuyenMaiGiam = 0,
                    SoTienVoucherGiam = 0,
                    MaHoaDon = "",
                    NgayTao = "",
                    TenNhanVien =""
                });
            }
            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(hd => hd.MaHoaDon == maHD);
            var user = await _userManager.FindByIdAsync(hoaDon.IdNguoiDung);
            if (hoaDon == null)
            {
                return Ok(new
                {
                    TongTienGoc = 0,
                    TienPhaiTra = 0,
                    SoTienKhuyenMaiGiam = 0,
                    SoTienVoucherGiam = 0,
                    MaHoaDon = "",
                    NgayTao = "",
                    TenNhanVien = ""

                });
            }
            double tongTienGoc = 0;
            double tienPhaiTra = 0;
            string ngayTao = ((DateTime)hoaDon.NgayTao).ToString("dd/MM/yyyy HH:mm");
            foreach (var item in hoaDon.hoaDonChiTietDTOs)
            {
                tongTienGoc += (double)item.GiaGoc * (int)item.SoLuong;
                tienPhaiTra += (double)item.GiaBan * (int)item.SoLuong;
            }
            return Ok(new
            {
                TongTienGoc = tongTienGoc,
                TienPhaiTra = tienPhaiTra,
                SoTienKhuyenMaiGiam = tongTienGoc - tienPhaiTra,
                SoTienVoucherGiam = 0,
                MaHoaDon = hoaDon.MaHoaDon,
                NgayTao = ngayTao,
                TenNhanVien = user.UserName
            });
        }
        [HttpGet]
        public async Task<IActionResult> ChiTietHoaDonDuocChon(string maHD)
        {
            if (string.IsNullOrWhiteSpace(maHD))
            {
                return PartialView("_HoaDonChiTietPartialView", new List<HoaDonChiTietDTO>());
            }
            var hoaDonDuocChon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(c => c.MaHoaDon == maHD).hoaDonChiTietDTOs;
            var listsanpham = await _SanPhamChiTietservice.GetDanhSachBienTheItemShopViewModelAsync();

            foreach (var item2 in hoaDonDuocChon)
            {
                var sanpham = listsanpham.FirstOrDefault(c => c.IdChiTietSp == item2.IdSanPhamChiTiet);
                item2.TenSanPham = sanpham.TenSanPham + "/" + sanpham.MauSac + "/" + sanpham.KichCo;
                item2.MaSanPham = sanpham.MaSanPham;
            }
            return PartialView("_HoaDonChiTietPartialView", hoaDonDuocChon);
        }
        [HttpPost]
        public async Task<IActionResult> ThemKhachHang(string TenKhachHang, string SDT)
        {
            if (string.IsNullOrWhiteSpace(TenKhachHang)|| string.IsNullOrWhiteSpace(SDT))
            {
                return Ok(new { TrangThai = false });
            }
            var khachHang = new KhachHang()
            {
                IdKhachHang = Guid.NewGuid().ToString(),
                SDT = SDT,
                //IdNguoiDung = ,
                TenKhachHang = TenKhachHang,
                TrangThai = (int)TrangThaiKhachHang.HoatDong,
            };
            var ketqua = await _hoaDonServices.TaoKhachHang(khachHang);
            if (ketqua == "Tạo khách hàng thành công")
            {
                return Ok(new
                {
                    TrangThai = true,
                    SDT = SDT,
                    KetQua = ketqua,
                });
            }
            return Ok(new
            {
                TrangThai = false,
                KetQua = ketqua,
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetVocherTaiQuay(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Ok(new { TrangThai = false });
            }
            var voucherReturn = await _VoucherNguoiDungservices.GetVocherTaiQuay(id.Trim());
            if (voucherReturn != null&&voucherReturn.IdVouCherNguoiDung !=null && voucherReturn.LoaiHinhUuDai <= 1)
            {
                return Ok(new
                {
                    IdVoucherNguoiDung = voucherReturn.IdVouCherNguoiDung,
                    MucGiam = voucherReturn.MucUuDai,
                    DieuKien = voucherReturn.DieuKien,
                    LoaiHinhUuDai = voucherReturn.LoaiHinhUuDai,
                    IdVoucher = id,
                    TrangThai = true
                });
            }
            else
            {
                return Ok(new
                {
                    TrangThai = false
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> QuetSanPhamVaoHoaDon(string maHD, string maSp)
        {
            if (string.IsNullOrWhiteSpace(maHD))
            {
                return Ok(new { TrangThai = false });
            }
            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(hd => hd.MaHoaDon == maHD);
            var sanPham = (await _SanPhamChiTietservice.GetDanhSachBienTheItemShopViewModelAsync()).FirstOrDefault(c => c.MaSanPham == maSp);
            if (sanPham.SoLuongTon == 0)
            {
                return Ok(new { TrangThai = false });
            }
            var hoaDonChitiet = new HoaDonChiTiet()
            {
                IdHoaDon = hoaDon.Id,
                IdHoaDonChiTiet = Guid.NewGuid().ToString(),
                IdSanPhamChiTiet = sanPham.IdChiTietSp.ToString(),
                SoLuong = 1,
                TrangThai = (int)TrangThaiHoaDonChiTiet.ChoTaiQuay,
                GiaBan = sanPham.GiaThucTe,
                GiaGoc = sanPham.GiaGoc,
            };
            var hoaDonChiTietTraLai = await _HoaDonChiTietservices.ThemSanPhamVaoHoaDon(hoaDonChitiet);

            await _SanPhamChiTietservice.UpDatSoLuongAynsc(new SanPhamSoLuongDTO()
            {
                IdChiTietSanPham = sanPham.IdChiTietSp,
                SoLuong = 1
            });
            return Ok(new
            {
                TrangThai = true,
            });
        }
        [HttpGet]
        public async Task<IActionResult> CapNhapThongTinTien(string maHD)
        {

            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(hd => hd.MaHoaDon == maHD);

            double tongTienThayDoi = 0;
            double soTienTraLaiThayDoi = 0;
            if (hoaDon != null)
            {
                foreach (var item in hoaDon.hoaDonChiTietDTOs)
                {
                    tongTienThayDoi += (double)item.GiaGoc * (double)item.SoLuong;
                    soTienTraLaiThayDoi += (double)item.GiaBan * (double)item.SoLuong;
                }
                var SoTienKhyenMaiGiam = tongTienThayDoi - soTienTraLaiThayDoi;
                return Ok(new
                {
                    TrangThai = true,
                    TongTienThayDoi = tongTienThayDoi,
                    SoTienTraLaiThayDoi = soTienTraLaiThayDoi,
                    SoTienKhyenMaiGiam = SoTienKhyenMaiGiam,
                    SoTienVoucherGiam = 0,
                });
            }
            return Ok(new
            {
                TrangThai = false,
                TongTienThayDoi = 0,
                SoTienTraLaiThayDoi = 0,
                SoTienKhyenMaiGiam = 0,
                SoTienVoucherGiam = 0,
            });
        }
        [HttpPost]
        public async Task<IActionResult> HuyHoaDon(string maHD, string lyDoHuy)
        {
            if (string.IsNullOrEmpty(maHD))
            {
                return Ok(new
                {
                    TrangThai = false,
                });
            }
            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(c=>c.MaHoaDon == maHD);
            var user = await _userManager.GetUserAsync(User);
            var idUser = await _userManager.GetUserIdAsync(user);
            var hoadonchitiet = await _HoaDonChiTietservices.HuyHoaDon(maHD, lyDoHuy, idUser);
            if(hoaDon.hoaDonChiTietDTOs.Count() == 0)
            {
                return Ok(
                  new { TrangThai = true, }
                  );
            }
            if (hoadonchitiet.Any())
            {
                foreach (var item in hoadonchitiet)
                {
                    await _SanPhamChiTietservice.UpDatSoLuongAynsc(new SanPhamSoLuongDTO()
                    {
                        IdChiTietSanPham = item.IdSanPhamChiTiet,
                        SoLuong = -(int)item.SoLuong,
                    });
                }
                return Ok(
                    new { TrangThai = true, }
                    );
            }
            return Ok(new
            {
                TrangThai = false,
            });
        }
        [HttpPost]
        public async Task<IActionResult> ThanhToan(string maHD, string SDT,double tongTien,double tienMat,double chuyenKhoan, string idVoucher,int hinhThuc,double tongGiam)
        {
            if (string.IsNullOrEmpty(maHD))
            {
                return Ok(new
                {
                    TrangThai = false,
                    Chuoi = "Vui lòng chọn hóa đơn",
                }) ;
            }
            var hoaDon = (await _hoaDonServices.GetAllHoaDonCho()).FirstOrDefault(c => c.MaHoaDon == maHD);
            var user = await _userManager.GetUserAsync(User);
            var idUser = await _userManager.GetUserIdAsync(user);
            var hoaDonUpdate = new HoaDon()
            {
                IdHoaDon = hoaDon.Id,
                MaHoaDon = hoaDon.MaHoaDon,
                IdNguoiDung = hoaDon.IdNguoiDung,
                NgayTao = hoaDon.NgayTao,
                TrangThaiGiaoHang = hoaDon.TrangThaiGiaoHang,
                TrangThaiThanhToan = (int)TrangThaiHoaDon.DaThanhToan,
                TongTien = tongTien,
                TienGiam = tongGiam,
                NgayThanhToan = DateTime.Now,
                IdNguoiSuaGanNhat= idUser,
            };
            if (!string.IsNullOrWhiteSpace(idVoucher))
            {
                var voucherReturn = await _VoucherNguoiDungservices.GetVocherTaiQuay(idVoucher);
                if (voucherReturn != null && voucherReturn.LoaiHinhUuDai <= 1 && voucherReturn.IdVouCherNguoiDung != null)
                {
                    hoaDonUpdate.IdVoucher = voucherReturn.IdVouCher;
                    if (!await _Voucherservices.UpdateVoucherAfterUseItTaiQuay(voucherReturn.IdVouCherNguoiDung))
                    {
                        return Ok(new
                        {
                            TrangThai = false,
                            Chuoi = "Lỗi khi sử dụng voucher",
                        });
                    }
                   
                }
                else
                {
                    hoaDonUpdate.IdVoucher = null;
                }
            }
            if (!string.IsNullOrWhiteSpace(SDT))
            {
                var khachHang = (await _hoaDonServices.GetKhachHangs()).FirstOrDefault(c => c.SDT == SDT);
                if (khachHang != null)
                {
                    hoaDonUpdate.IdKhachHang = khachHang.IdKhachHang;
                }
                else
                {
                    hoaDonUpdate.IdKhachHang = null;
                }
            }
            if (await _hoaDonServices.ThanhToanTaiQuay(hoaDonUpdate)) {
                if (hinhThuc == 1)
                {
                    var pttt = await _hoaDonServices.GetPTTT("TienMat");
                    if (await _hoaDonServices.TaoPTTTChiTiet(hoaDonUpdate.IdHoaDon, pttt, tienMat, (int)PTThanhToanChiTiet.DaThanhToan))
                    {
                        return Ok(new
                        {
                            TrangThai = true,
                        });
                    }

                }
                if (hinhThuc == 2)
                {
                    var pttt = await _hoaDonServices.GetPTTT("ChuyenKhoan");

                    if (await _hoaDonServices.TaoPTTTChiTiet(hoaDonUpdate.IdHoaDon, pttt, chuyenKhoan, (int)PTThanhToanChiTiet.DaThanhToan))
                    {
                        return Ok(new
                        {
                            TrangThai = true,
                        });
                    }
                }
                if (hinhThuc == 3)
                {
                    var pttt = await _hoaDonServices.GetPTTT("ChuyenKhoan");

                    if (await _hoaDonServices.TaoPTTTChiTiet(hoaDonUpdate.IdHoaDon, pttt, chuyenKhoan, (int)PTThanhToanChiTiet.DaThanhToan))
                    {
                        pttt = await _hoaDonServices.GetPTTT("TienMat");

                        if (await _hoaDonServices.TaoPTTTChiTiet(hoaDonUpdate.IdHoaDon, pttt, tienMat, (int)PTThanhToanChiTiet.DaThanhToan))
                        {
                            return Ok(new
                            {
                                TrangThai = true,
                            });
                        }
                        else
                        {
                            _hoaDonServices.XoaPhuongThucThanhToanChiTietBangIdHoaDon(hoaDonUpdate.IdHoaDon);
                            return Ok(new
                            {
                                TrangThai = false,
                                Chuoi = "Thanh toán không thành công"
                            });
                        }
                    }
                    return Ok(new
                    {
                        TrangThai = false,
                        Chuoi = "Thanh toán không thành công"
                    });
                }
            }
            return Ok(new
            {
                TrangThai = false,
                Chuoi = "Thanh toán không thành công"
            });
        }
       
    }
}
