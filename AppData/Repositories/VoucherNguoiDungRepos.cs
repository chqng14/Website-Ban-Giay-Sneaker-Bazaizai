using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.VoucherNguoiDung;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App_Data.Repositories.TrangThai;

namespace App_Data.Repositories
{
    public class VoucherNguoiDungRepos : IVoucherNguoiDungRepos
    {
        private readonly BazaizaiContext _context;
        public VoucherNguoiDungRepos()
        {
            _context = new BazaizaiContext();
        }

        public async Task<List<NguoiDung>> GetLstNguoiDungMoi()
        {
            return await Task.Run(() =>
            {
                return _context.NguoiDungs.Where(c => c.TongChiTieu == 0 && c.UserName != "Admin").ToList();
            });
        }


        public async Task<VoucherTaiQuayDto> GetVocherTaiQuay(string id)
        {

            var voucherTaiQuay = _context.VoucherNguoiDungs.FirstOrDefault(c => c.IdVouCherNguoiDung == id && c.TrangThai == (int)TrangThaiVoucherNguoiDung.KhaDung);
            if (voucherTaiQuay != null)
            {
                var voucher = _context.Vouchers.FirstOrDefault(c => c.IdVoucher == voucherTaiQuay.IdVouCher && (c.TrangThai == (int)TrangThaiVoucher.HoatDongTaiQuay || c.TrangThai == (int)TrangThaiVoucher.HoatDong));
                if (voucher != null)
                {
                    return new VoucherTaiQuayDto()
                    {
                        IdVouCherNguoiDung = voucherTaiQuay.IdVouCherNguoiDung,
                        IdNguoiDung = voucherTaiQuay.IdNguoiDung,
                        IdVouCher = voucher.IdVoucher,
                        MaVoucher = voucher.MaVoucher,
                        DieuKien = voucher.DieuKien,
                        LoaiHinhUuDai = voucher.LoaiHinhUuDai,
                        MucUuDai = voucher.MucUuDai,
                        TenVoucher = voucher.TenVoucher,
                        TrangThai = voucher.TrangThai,
                        HinhThuc = true,
                    };
                }
            }
            else
            {
                var voucher = _context.Vouchers.FirstOrDefault(c => c.MaVoucher == id && (c.TrangThai == (int)TrangThaiVoucher.HoatDongTaiQuay || c.TrangThai == (int)TrangThaiVoucher.HoatDong));
                if (voucher != null)
                {
                    voucherTaiQuay = _context.VoucherNguoiDungs.FirstOrDefault(c => c.IdVouCher == voucher.IdVoucher && c.TrangThai == (int)TrangThaiVoucherNguoiDung.KhaDung);
                    if (voucherTaiQuay == null) { return null; }
                    return new VoucherTaiQuayDto()
                    {
                        IdVouCherNguoiDung = voucherTaiQuay.IdVouCherNguoiDung,
                        IdNguoiDung = voucherTaiQuay.IdNguoiDung,
                        IdVouCher = voucher.IdVoucher,
                        MaVoucher = voucher.MaVoucher,
                        DieuKien = voucher.DieuKien,
                        LoaiHinhUuDai = voucher.LoaiHinhUuDai,
                        MucUuDai = voucher.MucUuDai,
                        TenVoucher = voucher.TenVoucher,
                        TrangThai = voucher.TrangThai,
                        HinhThuc = false,
                    };
                }
            }
            return null;
        }

        public async Task<bool> TangVoucherNguoiDung(AddVoucherRequestDTO addVoucherRequestDTO)
        {
            var VoucherGet = await _context.Vouchers.FirstOrDefaultAsync(c => c.MaVoucher == addVoucherRequestDTO.MaVoucher);
            try
            {
                if (VoucherGet != null)
                {

                    foreach (var item in addVoucherRequestDTO.UserId)
                    {
                        VoucherNguoiDung voucherNguoiDung = new VoucherNguoiDung()
                        {
                            IdNguoiDung = item,
                            IdVouCher = VoucherGet.IdVoucher,
                            IdVouCherNguoiDung = Guid.NewGuid().ToString(),
                            TrangThai = (int)TrangThaiVoucherNguoiDung.KhaDung,
                            NgayNhan = DateTime.Now,
                        };
                        await _context.AddAsync(voucherNguoiDung);
                    }
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public async Task<bool> TangVoucherNguoiDungMoi(string ma, string idUser)
        {
            var voucher = await _context.Vouchers.FirstOrDefaultAsync(c => c.MaVoucher == ma && c.TrangThai == (int)TrangThaiVoucher.HoatDong);

            if (voucher != null)
            {
                var voucherNguoi = new VoucherNguoiDung()
                {
                    IdNguoiDung = idUser,
                    IdVouCher = voucher.IdVoucher,
                    IdVouCherNguoiDung = Guid.NewGuid().ToString(),
                    TrangThai = (int)TrangThaiVoucherNguoiDung.KhaDung,
                    NgayNhan = DateTime.Now
                };
                //var existsInVoucherNguoiDung = VcNguoiDungRepos.GetAll().FirstOrDefault(vnd => vnd.IdVouCher == VoucherKhaDung.IdVoucher && vnd.IdNguoiDung == item);
                var existsInVoucherNguoiDung = await _context.VoucherNguoiDungs.AnyAsync(vnd => vnd.IdVouCher == voucher.IdVoucher && vnd.IdNguoiDung == idUser);
                if (!existsInVoucherNguoiDung)
                {
                    await _context.AddAsync(voucherNguoi);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;

        }
    }
}
