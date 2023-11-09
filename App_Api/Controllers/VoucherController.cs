using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.MauSac;
using App_Data.ViewModels.Voucher;
using AutoMapper;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using static App_Data.Repositories.TrangThai;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IAllRepo<VoucherNguoiDung> VcNguoiDungRepos;
        private readonly IAllRepo<Voucher> allRepo;
        private readonly IMapper _mapper;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        DbSet<Voucher> vouchers;
        DbSet<VoucherNguoiDung> voucherNguoiDung;
        public VoucherController(IMapper mapper)
        {
            voucherNguoiDung = DbContextModel.voucherNguoiDungs;
            AllRepo<VoucherNguoiDung> VcNd = new AllRepo<VoucherNguoiDung>(DbContextModel, voucherNguoiDung);
            VcNguoiDungRepos = VcNd;
            vouchers = DbContextModel.vouchers;
            AllRepo<Voucher> all = new AllRepo<Voucher>(DbContextModel, vouchers);
            allRepo = all;
            _mapper = mapper;
        }
        [HttpGet("GetVoucher")]
        public List<Voucher> GetAllVoucher()
        {
            return allRepo.GetAll().ToList();
        }
        [HttpGet("GetVoucherByMa/{ma}")]
        public Voucher? GetVoucher(string ma)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.MaVoucher == ma);
        }
        [HttpGet("GetVoucherDTOByMa/{id}")]
        public VoucherDTO? GetVoucherDTO(string id)
        {
            var Voucher = allRepo.GetAll().FirstOrDefault(c => c.IdVoucher == id);
            var VoucherDTO = _mapper.Map<VoucherDTO>(Voucher);
            return VoucherDTO;
        }
        private string GenerateRandomVoucherCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            List<Voucher> vouchers = GetAllVoucher(); // Lấy danh sách các mã khuyến mãi hiện có
            bool isDuplicate = false; // Biến kiểm tra trùng lặp
            string voucherCode = ""; // Biến lưu mã khuyến mãi ngẫu nhiên
            do
            {
                voucherCode = new string(Enumerable.Repeat(chars, 8)
                    .Select(s => s[random.Next(s.Length)]).ToArray()); // Tạo mã khuyến mãi ngẫu nhiên
                isDuplicate = vouchers.Any(v => v.MaVoucher == voucherCode); // Kiểm tra xem mã khuyến mãi có trùng với mã nào trong danh sách không
            } while (isDuplicate); // Nếu trùng thì lặp lại quá trình tạo và kiểm tra

            return voucherCode; // Trả về mã khuyến mãi không trùng
        }
        #region VoucherOn
        [HttpPost("CreateVoucher")]
        public bool Create(VoucherDTO voucherDTO)
        {
            voucherDTO.IdVoucher = Guid.NewGuid().ToString();
            var voucher = _mapper.Map<Voucher>(voucherDTO);
            voucher.MaVoucher = GenerateRandomVoucherCode();
            voucher.NgayTao = DateTime.Now;
            if (voucher.NgayBatDau > DateTime.Now)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.ChuaBatDau;
            }
            if (voucher.NgayBatDau <= DateTime.Now)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.HoatDong;
            }
            if (voucher.SoLuong == 0)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.KhongHoatDong;
            }
            if (voucher.SoLuong > 0 && voucher.NgayBatDau < DateTime.Now && voucher.NgayKetThuc > DateTime.Now)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.HoatDong;
            }
            return allRepo.AddItem(voucher);
        }
        [HttpPut("DeleteVoucher/{id}")]
        public bool Delete(string id)
        {
            var VoucherGet = GetAllVoucher().FirstOrDefault(c => c.IdVoucher == id);
            if (VoucherGet != null)
            {
                if (VoucherGet.TrangThai != (int)TrangThaiVoucher.KhongHoatDong)
                {
                    VoucherGet!.TrangThai = (int)TrangThaiVoucher.DaHuy;
                    allRepo.EditItem(VoucherGet);
                    //sau khi huỷ hoạt động voucher sẽ xoá voucher người dùng khi họ chưa dùng
                    var VoucherNguoiDung = VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCher == id && c.TrangThai != (int)TrangThaiVoucherNguoiDung.DaSuDung);
                    return VcNguoiDungRepos.RemoveItem(VoucherNguoiDung);
                }
            }
            return false;
        }
        [HttpPut("RestoreVoucher/{id}")]
        public bool RestoreVoucher(string id)
        {
            var VoucherGet = GetAllVoucher().FirstOrDefault(c => c.IdVoucher == id);
            if (VoucherGet != null)
            {
                if (VoucherGet.NgayBatDau < DateTime.Now && VoucherGet.NgayKetThuc > DateTime.Now)
                {
                    VoucherGet.TrangThai = (int)TrangThaiVoucher.HoatDong;
                }
                else if (VoucherGet.NgayBatDau > DateTime.Now && VoucherGet.NgayKetThuc > DateTime.Now)
                {
                    VoucherGet.TrangThai = (int)TrangThaiVoucher.ChuaBatDau;
                }
                return allRepo.EditItem(VoucherGet);
            }
            return false;
        }
        [HttpPut("UpdateVoucher")]
        public bool Update(VoucherDTO voucherDTO)
        {
            var voucherGet = allRepo.GetAll().FirstOrDefault(c => c.IdVoucher == voucherDTO.IdVoucher);

            DateTime NgayTao = voucherGet.NgayTao;
            if (voucherGet != null)
            {
                _mapper.Map(voucherDTO, voucherGet);
                if (voucherDTO.TrangThai == null)
                {

                }
                if (voucherGet.NgayBatDau > DateTime.Now)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.ChuaBatDau;
                }
                if (voucherGet.NgayBatDau < DateTime.Now)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.HoatDong; ;
                }
                if (voucherGet.SoLuong == 0)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.KhongHoatDong;
                }
                if (voucherGet.SoLuong > 0 && voucherGet.NgayBatDau < DateTime.Now && voucherGet.NgayKetThuc > DateTime.Now)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.HoatDong;
                }
                voucherGet.NgayTao = NgayTao;
                return allRepo.EditItem(voucherGet);
            }
            return false;
        }
        [HttpPut("UpdateVoucherAfterUseIt/{idVoucher}")]
        public bool UpdateVoucherAfterUseIt(string idVoucher, string idUser)
        {
            var voucherNguoiDung = VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCher == idVoucher && c.IdNguoiDung == idUser);
            if (voucherNguoiDung != null)
            {
                voucherNguoiDung.TrangThai = (int)TrangThaiVoucherNguoiDung.DaSuDung;
                VcNguoiDungRepos.EditItem(voucherNguoiDung);
                return true;
            }
            return false;
        }
        #endregion

        #region VoucherTaiQuay

        [HttpPost("CreateVoucherTaiQuay")]
        public bool CreateTaiQuay(VoucherDTO voucherDTO)
        {
            voucherDTO.IdVoucher = Guid.NewGuid().ToString();
            var voucher = _mapper.Map<Voucher>(voucherDTO);
            voucher.MaVoucher = GenerateRandomVoucherCode();
            voucher.NgayTao = DateTime.Now;
            if (voucher.NgayBatDau > DateTime.Now)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.ChuaHoatDongCung;
            }
            if (voucher.NgayBatDau <= DateTime.Now)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.HoatDongCung;
            }
            if (voucher.SoLuong == 0)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.KhongHoatDongCung;
            }
            if (voucher.SoLuong > 0 && voucher.NgayBatDau < DateTime.Now && voucher.NgayKetThuc > DateTime.Now)
            {
                voucher.TrangThai = (int)TrangThaiVoucher.HoatDongCung;
            }
            return allRepo.AddItem(voucher);
        }

        [HttpPut("DeleteVoucherTaiQuay/{id}")]
        public bool DeleteTaiQuay(string id)
        {
            var VoucherGet = GetAllVoucher().FirstOrDefault(c => c.IdVoucher == id);
            if (VoucherGet != null)
            {
                if (VoucherGet.TrangThai != (int)TrangThaiVoucher.KhongHoatDongCung)
                {
                    VoucherGet!.TrangThai = (int)TrangThaiVoucher.DaHuyCung;
                    allRepo.EditItem(VoucherGet);
                    //sau khi huỷ hoạt động voucher sẽ xoá voucher người dùng khi họ chưa dùng
                    var VoucherNguoiDung = VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCher == id && c.TrangThai != (int)TrangThaiVoucherNguoiDung.DaSuDung);
                    return VcNguoiDungRepos.RemoveItem(VoucherNguoiDung);
                }
            }
            return false;
        }
        [HttpPut("RestoreVoucherTaiQuay/{id}")]
        public bool RestoreVoucherTaiQuay(string id)
        {
            var VoucherGet = GetAllVoucher().FirstOrDefault(c => c.IdVoucher == id);
            if (VoucherGet != null)
            {
                if (VoucherGet.NgayBatDau < DateTime.Now && VoucherGet.NgayKetThuc > DateTime.Now)
                {
                    VoucherGet.TrangThai = (int)TrangThaiVoucher.HoatDongCung;
                }
                else if (VoucherGet.NgayBatDau > DateTime.Now && VoucherGet.NgayKetThuc > DateTime.Now)
                {
                    VoucherGet.TrangThai = (int)TrangThaiVoucher.ChuaHoatDongCung;
                }
                return allRepo.EditItem(VoucherGet);
            }
            return false;
        }
        [HttpPut("UpdateVoucherTaiQuay")]
        public bool UpdateTaiQuay(VoucherDTO voucherDTO)
        {
            var voucherGet = allRepo.GetAll().FirstOrDefault(c => c.IdVoucher == voucherDTO.IdVoucher);

            DateTime NgayTao = voucherGet.NgayTao;
            if (voucherGet != null)
            {
                _mapper.Map(voucherDTO, voucherGet);
                if (voucherDTO.TrangThai == null)
                {

                }
                if (voucherGet.NgayBatDau > DateTime.Now)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.ChuaHoatDongCung;
                }
                if (voucherGet.NgayBatDau < DateTime.Now)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.HoatDongCung; ;
                }
                if (voucherGet.SoLuong == 0)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.KhongHoatDongCung;
                }
                if (voucherGet.SoLuong > 0 && voucherGet.NgayBatDau < DateTime.Now && voucherGet.NgayKetThuc > DateTime.Now)
                {
                    voucherGet.TrangThai = (int)TrangThaiVoucher.HoatDongCung;
                }
                voucherGet.NgayTao = NgayTao;
                return allRepo.EditItem(voucherGet);
            }
            return false;
        }
        [HttpPut("UpdateVoucherAfterUseItTaiQuay/{idVoucher}")]
        public bool UpdateVoucherAfterUseItTaiQuay(string idVoucher, string idUser)
        {
            var voucherNguoiDung = VcNguoiDungRepos.GetAll().FirstOrDefault(c => c.IdVouCher == idVoucher && c.IdNguoiDung == idUser);
            if (voucherNguoiDung != null)
            {
                voucherNguoiDung.TrangThai = (int)TrangThaiVoucherNguoiDung.DaSuDung;
                VcNguoiDungRepos.EditItem(voucherNguoiDung);
                return true;
            }
            return false;
        }
        #endregion
    }
}
