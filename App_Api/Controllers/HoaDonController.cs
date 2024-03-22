using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.DanhGia;
using App_Data.ViewModels.HoaDon;
using App_Data.ViewModels.HoaDonChiTietDTO;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using static App_Data.Repositories.TrangThai;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private PTThanhToanChiTietController _PTThanhToanChiTietController;
        private HoaDonChiTietController _hoaDonChiTietController;
        private readonly IHoaDonRepos _hoaDon;
        private readonly IMapper _mapper;
        private PTThanhToanController _PTThanhToanController;
        private SanPhamChiTietController _sanPhamChiTietController;
        private readonly IKhachHangRepo _khachHangRepo;
        private readonly IHoaDonChiTietRepos _hoaDonChiTietRepos;
        private DanhGiaController _danhGiaController;
        public HoaDonController(IMapper mapper, SanPhamChiTietController sanPhamChiTietController)
        {
            _hoaDon = new HoaDonRepos(mapper);
            _mapper = mapper;
            _hoaDonChiTietController = new HoaDonChiTietController(mapper);
            _PTThanhToanChiTietController = new PTThanhToanChiTietController();
            _PTThanhToanController = new PTThanhToanController();
            _sanPhamChiTietController = sanPhamChiTietController;
            _khachHangRepo = new KhachHangRepo();
            _hoaDonChiTietRepos = new HoaDonChiTietRepos(mapper);
            _danhGiaController = new DanhGiaController();
        }
        [HttpPost]
        public async Task<HoaDon> TaoHoaDonTaiQuay(HoaDon hoaDon)
        {
            return _hoaDon.TaoHoaDonTaiQuay(hoaDon);
        }
        // POST api/<HoaDonController>
        [HttpPost]
        public async Task<string> TaoHoaDonOnlineDTO(HoaDonDTO HoaDonDTO)
        {
            var hoadon = _mapper.Map<HoaDon>(HoaDonDTO);
            _hoaDon.AddBill(hoadon);
            return hoadon.MaHoaDon;
        }
        [HttpGet]
        public async Task<List<HoaDonChoDTO>> GetAllHoaDonCho()
        {
            return _hoaDon.GetAllHoaDonCho();
        }
        [HttpGet]
        public async Task<List<HoaDonViewModel>> GetHoaDonOnline()
        {
            return _hoaDon.GetHoaDon();
        }


        [HttpGet]
        public async Task<List<HoaDonTest>> GetHoaDonOnlineTest(string idNguoiDung)
        {
            var danhSachHoaDon = new List<HoaDonTest>();

            var danhSachHoaDonGoc = (await GetHoaDonOnline()).Where(c => c.IdNguoiDung == idNguoiDung);
            foreach (var hoadon in danhSachHoaDonGoc)
            {
                var hoadonct = (await _hoaDonChiTietController.GetAllHoaDon()).Where(c => c.IdHoaDon == hoadon.IdHoaDon).ToList();
                var loaithanhtoan = await GetPTThanhToan(hoadon.IdHoaDon);


                var sanPhamList = new List<SanPhamTest>();

                foreach (var item in hoadonct)
                {
                    var sp = await _sanPhamChiTietController.GetSanPhamViewModel(item.IdSanPhamChiTiet);
                    var danhgia = await _danhGiaController.GetDanhGiaViewModelById(item.IdSanPhamChiTiet + "*" + hoadon.IdHoaDon);
                    var dg = new DanhGiaViewModel();
                    if (danhgia != null)
                    {
                        if (danhgia.IdSanPhamChiTiet == sp.IdChiTietSp && sp.IdChiTietSp == item.IdSanPhamChiTiet)
                        {
                            dg = danhgia;
                        }
                    }
                    var sanPhamObject = new SanPhamTest
                    {
                        IdSanPhamChiTiet = item.IdSanPhamChiTiet,
                        SoLuong = item.SoLuong,
                        GiaBan = item.GiaBan,
                        //GiaGoc = item.GiaGoc,
                        //GiaNhap = item.GiaGoc,
                        TenSanPham = sp.SanPham,
                        LinkAnh = sp.ListTenAnh,
                        TenMauSac = sp.MauSac,
                        TenKichCo = sp.KichCo,
                        TenThuongHieu = sp.ThuongHieu,
                        TrangThaiGiaoHang = hoadon.TrangThaiGiaoHang,
                        TongTien = item.GiaBan * item.SoLuong,
                        DanhGia = dg,
                    };
                    sanPhamList.Add(sanPhamObject);
                }

                var hoadontest = new HoaDonTest()
                {
                    IdHoaDon = hoadon.IdHoaDon,
                    MaHoaDon = hoadon.MaHoaDon,
                    TienGiam = hoadon.TienGiam,
                    TienShip = hoadon.TienShip,
                    TongTien = hoadon.TongTien,
                    TongGia = hoadon.TongTien + hoadon.TienShip - (hoadon.TienGiam ?? 0),
                    SanPham = sanPhamList,
                    NgayTao = hoadon.NgayTao,
                    NgayGiaoDuKien = hoadon.NgayGiaoDuKien,
                    TrangThaiGiaoHang = hoadon.TrangThaiGiaoHang,
                    TrangThaiThanhToan = hoadon.TrangThaiThanhToan,
                    TenNguoiNhan = hoadon.TenNguoiNhan,
                    DiaChi = hoadon.DiaChi,
                    SDT = hoadon.SDT,
                    LoaiThanhToan = loaithanhtoan,
                    MoTa = hoadon.MoTa,
                    LiDoHuy = hoadon.LiDoHuy,
                };

                danhSachHoaDon.Add(hoadontest);
            }

            return danhSachHoaDon;
        }
        [HttpGet]
        public async Task<HoaDonTest> GetHoaDonOnlineById(string idHoadon, string idNguoiDung)
        {
            var hoadon = (await GetHoaDonOnline()).FirstOrDefault(c => c.IdHoaDon == idHoadon && c.IdNguoiDung == idNguoiDung);

            var hoadonct = (await _hoaDonChiTietController.GetAllHoaDon()).Where(c => c.IdHoaDon == hoadon.IdHoaDon).ToList();
            var loaithanhtoan = await GetPTThanhToan(hoadon.IdHoaDon);


            var sanPhamList = new List<SanPhamTest>();

            foreach (var item in hoadonct)
            {
                var sp = await _sanPhamChiTietController.GetSanPhamViewModel(item.IdSanPhamChiTiet);
                var danhgia = await _danhGiaController.GetDanhGiaViewModelById(item.IdSanPhamChiTiet + "*" + idHoadon);
                var dg = new DanhGiaViewModel();
                if (danhgia != null)
                {
                    if (danhgia.IdSanPhamChiTiet == sp.IdChiTietSp && sp.IdChiTietSp == item.IdSanPhamChiTiet)
                    {
                        dg = danhgia;
                    }
                }
                var sanPhamObject = new SanPhamTest
                {
                    IdSanPhamChiTiet = item.IdSanPhamChiTiet,
                    SoLuong = item.SoLuong,
                    GiaBan = item.GiaBan,
                    //GiaGoc = item.GiaGoc,
                    //GiaNhap = item.GiaGoc,
                    TenSanPham = sp.SanPham,
                    LinkAnh = sp.ListTenAnh,
                    TenMauSac = sp.MauSac,
                    TenKichCo = sp.KichCo,
                    TenThuongHieu = sp.ThuongHieu,
                    TrangThaiGiaoHang = hoadon.TrangThaiGiaoHang,
                    TongTien = item.GiaBan * item.SoLuong,
                    DanhGia = dg,
                };
                sanPhamList.Add(sanPhamObject);
            }

            var hoadontest = new HoaDonTest()
            {
                IdHoaDon = hoadon.IdHoaDon,
                MaHoaDon = hoadon.MaHoaDon,
                TienGiam = hoadon.TienGiam,
                TienShip = hoadon.TienShip,
                TongTien = hoadon.TongTien,
                TongGia = hoadon.TongTien + hoadon.TienShip - (hoadon.TienGiam ?? 0),
                SanPham = sanPhamList,
                NgayTao = hoadon.NgayTao,
                NgayGiaoDuKien = hoadon.NgayGiaoDuKien,
                TrangThaiGiaoHang = hoadon.TrangThaiGiaoHang,
                TrangThaiThanhToan = hoadon.TrangThaiThanhToan,
                TenNguoiNhan = hoadon.TenNguoiNhan,
                DiaChi = hoadon.DiaChi,
                SDT = hoadon.SDT,
                LoaiThanhToan = loaithanhtoan,
                MoTa = hoadon.MoTa,
                LiDoHuy = hoadon.LiDoHuy,
            };

            return hoadontest;
        }

        [HttpGet]
        public async Task<HoaDonTest> GetHoaDonOnlineByMa(string Ma)
        {
            try
            {
                var hoadon = (await GetHoaDonOnline()).FirstOrDefault(c => c.MaHoaDon.ToUpper() == Ma.ToUpper());
                if (hoadon == null)
                {
                    return null;
                }
                if (hoadon.TrangThaiGiaoHang == (int)TrangThaiGiaoHang.TaiQuay)
                {
                    return null;
                }
                if (!_hoaDon.CheckKhachHang(hoadon.IdThongTinGH))
                {
                    return null;
                }
                var hoadonct = (await _hoaDonChiTietController.GetAllHoaDon()).Where(c => c.IdHoaDon == hoadon.IdHoaDon).ToList();
                var loaithanhtoan = await GetPTThanhToan(hoadon.IdHoaDon);


                var sanPhamList = new List<SanPhamTest>();

                foreach (var item in hoadonct)
                {
                    var sp = await _sanPhamChiTietController.GetSanPhamViewModel(item.IdSanPhamChiTiet);
                    var sanPhamObject = new SanPhamTest
                    {
                        IdSanPhamChiTiet = item.IdSanPhamChiTiet,
                        SoLuong = item.SoLuong,
                        GiaBan = item.GiaBan,
                        //GiaGoc = item.GiaGoc,
                        //GiaNhap = item.GiaGoc,
                        TenSanPham = sp.SanPham,
                        LinkAnh = sp.ListTenAnh,
                        TenMauSac = sp.MauSac,
                        TenKichCo = sp.KichCo,
                        TenThuongHieu = sp.ThuongHieu,
                        TrangThaiGiaoHang = hoadon.TrangThaiGiaoHang,
                        TongTien = item.GiaBan * item.SoLuong,
                    };
                    sanPhamList.Add(sanPhamObject);
                }

                var hoadontest = new HoaDonTest()
                {
                    IdHoaDon = hoadon.IdHoaDon,
                    MaHoaDon = hoadon.MaHoaDon,
                    TienGiam = hoadon.TienGiam,
                    TienShip = hoadon.TienShip,
                    TongTien = hoadon.TongTien,
                    TongGia = hoadon.TongTien + hoadon.TienShip - (hoadon.TienGiam ?? 0),
                    SanPham = sanPhamList,
                    NgayTao = hoadon.NgayTao,
                    NgayGiaoDuKien = hoadon.NgayGiaoDuKien,
                    TrangThaiGiaoHang = hoadon.TrangThaiGiaoHang,
                    TrangThaiThanhToan = hoadon.TrangThaiThanhToan,
                    TenNguoiNhan = hoadon.TenNguoiNhan,
                    DiaChi = hoadon.DiaChi,
                    SDT = hoadon.SDT,
                    LoaiThanhToan = loaithanhtoan,
                    MoTa = hoadon.MoTa,
                    LiDoHuy = hoadon.LiDoHuy,
                };

                return hoadontest;

            }
            catch (Exception)
            {
                return null;
            }

        }

        [HttpGet]
        public async Task<List<HoaDonTest>> GetHoaDonOnlineAdmin()
        {
            var danhSachHoaDon = new List<HoaDonTest>();

            var danhSachHoaDonGoc = await GetHoaDonOnline();
            foreach (var hoadon in danhSachHoaDonGoc)
            {
                var hoadonct = (await _hoaDonChiTietController.GetAllHoaDon()).Where(c => c.IdHoaDon == hoadon.IdHoaDon).ToList();
                var loaithanhtoan = await GetPTThanhToan(hoadon.IdHoaDon);


                var sanPhamList = new List<SanPhamTest>(); // Tạo JArray mới cho mỗi hóa đơn

                foreach (var item in hoadonct)
                {
                    var sp = await _sanPhamChiTietController.GetSanPhamViewModel(item.IdSanPhamChiTiet);
                    var sanPhamObject = new SanPhamTest
                    {
                        IdSanPhamChiTiet = item.IdSanPhamChiTiet,
                        SoLuong = item.SoLuong,
                        GiaBan = item.GiaBan,
                        //GiaGoc = item.GiaGoc,
                        //GiaNhap = item.GiaGoc,
                        TenSanPham = sp.SanPham,
                        LinkAnh = sp.ListTenAnh,
                        TenMauSac = sp.MauSac,
                        TenKichCo = sp.KichCo,
                        TenThuongHieu = sp.ThuongHieu,
                        TrangThaiGiaoHang = hoadon.TrangThaiGiaoHang,
                        TongTien = item.GiaBan * item.SoLuong,
                    };
                    sanPhamList.Add(sanPhamObject);
                }

                var hoadontest = new HoaDonTest()
                {
                    IdHoaDon = hoadon.IdHoaDon,
                    MaHoaDon = hoadon.MaHoaDon,
                    TienGiam = hoadon.TienGiam,
                    TienShip = hoadon.TienShip,
                    TongTien = hoadon.TongTien,
                    TongGia = hoadon.TongTien + hoadon.TienShip - (hoadon.TienGiam ?? 0),
                    SanPham = sanPhamList,
                    NgayTao = hoadon.NgayTao,
                    NgayGiaoDuKien = hoadon.NgayGiaoDuKien,
                    TrangThaiGiaoHang = hoadon.TrangThaiGiaoHang,
                    TrangThaiThanhToan = hoadon.TrangThaiThanhToan,
                    TenNguoiNhan = hoadon.TenNguoiNhan,
                    DiaChi = hoadon.DiaChi,
                    SDT = hoadon.SDT,
                    LoaiThanhToan = loaithanhtoan,
                    MoTa = hoadon.MoTa,
                    LiDoHuy = hoadon.LiDoHuy,
                };

                danhSachHoaDon.Add(hoadontest);
            }

            return danhSachHoaDon;
        }

        [HttpPut]
        public async Task<bool> UpdateTrangThaiGiaoHangHoaDon(string idHoaDon, string? idNguoiDung, int trangThaiGiaoHang, string? Lido, DateTime? ngayCapNhatGanNhat)
        {
            var hoadon = _hoaDon.GetHoaDonUpdate().FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            hoadon.IdNguoiSuaGanNhat = idNguoiDung;
            hoadon.TrangThaiGiaoHang = trangThaiGiaoHang;
            hoadon.LiDoHuy = Lido;
            hoadon.NgayCapNhatGanNhat = ngayCapNhatGanNhat;
            if (trangThaiGiaoHang == (int)TrangThaiGiaoHang.DaGiao)
            {
                hoadon.TrangThaiThanhToan = 1;
            }
            return _hoaDon.EditBill(hoadon);
        }
        [HttpPut]
        public async Task<bool> UpdateDiaChi(string idHoaDon, string diaChi)
        {
            var hoadon = _hoaDon.GetHoaDonUpdate().FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            hoadon.DiaChi = diaChi;
            return _hoaDon.EditBill(hoadon);
        }
        [HttpPut]
        public async Task<bool> UpdateTrangThaiHoaDonOnline(string idHoaDon, int TrangThaiThanhToan)
        {
            var hoadon = _hoaDon.GetHoaDonUpdate().FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            hoadon.TrangThaiThanhToan = TrangThaiThanhToan;
            await _hoaDonChiTietController.SuaTrangThaiHoaDon(idHoaDon, TrangThaiThanhToan);
            return _hoaDon.EditBill(hoadon);
        }

        [HttpPut]
        public async Task<bool> UpdateNgayHoaDonOnline(string idHoaDon, DateTime? NgayThanhToan, DateTime? NgayNhan, DateTime? NgayShip)
        {
            var hoadon = _hoaDon.GetHoaDonUpdate().FirstOrDefault(c => c.IdHoaDon == idHoaDon);
            hoadon.NgayNhan = NgayNhan ?? hoadon.NgayNhan;
            hoadon.NgayShip = NgayShip ?? hoadon.NgayShip;
            hoadon.NgayThanhToan = NgayThanhToan ?? hoadon.NgayThanhToan;
            return _hoaDon.EditBill(hoadon);
        }

        [HttpGet]
        public async Task<string> GetPTThanhToan(string idhoadon)
        {
            var pt = _PTThanhToanChiTietController.PhuongThucThanhToanChiTietByIdPTTT(idhoadon);
            if (pt == null)
            {
                return "Không có";
            }
            else
            {
                var idpt = _PTThanhToanController.ShowAll().FirstOrDefault(c => c.IdPhuongThucThanhToan == pt.IdThanhToan);
                return idpt.TenPhuongThucThanhToan;
            }
        }
        [HttpPost]
        public async Task<string> TaoKhachHang(KhachHang khachHang)
        {
            return _khachHangRepo.TaoKhachHang(khachHang);
        }
        [HttpGet]
        public async Task<List<KhachHang>> GetAllKhachHang()
        {
            return _khachHangRepo.GetKhachHangs();
        }
        [HttpPut]
        public async Task<List<HoaDonChiTiet>> HuyHoaDon(string maHD, string lyDoHuy, string idUser)
        {
            try
            {
                var idHoaDon = _hoaDon.HuyHoaDon(maHD, lyDoHuy, idUser);
                if (idHoaDon != null)
                {
                    return _hoaDonChiTietRepos.HuyHoaDonChiTiet(idHoaDon);
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }

        }

        [HttpPut]
        public async Task<bool> ThanhToanTaiQuay(HoaDon hoaDon)
        {
            try
            {
                if (_hoaDon.ThanhToanTaiQuay(hoaDon))
                {
                    return _hoaDonChiTietRepos.ThanhToanHoaDonChiTiet(hoaDon.IdHoaDon);
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
