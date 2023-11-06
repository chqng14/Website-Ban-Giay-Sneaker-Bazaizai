using App_Data.DbContextt;
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

        public async Task<List<NguoiDung>> GetLstNguoiDUngMoi()
        {
            return _context.NguoiDungs.Where(c => c.TongChiTieu == 0).ToList();
        }

        public async Task<bool> TangVoucherNguoiDung(AddVoucherRequestDTO addVoucherRequestDTO)
        {
            var VoucherGet = await _context.vouchers.FirstOrDefaultAsync(c => c.MaVoucher == addVoucherRequestDTO.MaVoucher);
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
                            NgayNhan= DateTime.Now,
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

        public async Task TangVoucherNguoiDungMoi(string ma, string idUser)
        {
            var voucher = await _context.vouchers.FirstOrDefaultAsync(c => c.MaVoucher == ma && c.TrangThai == (int)TrangThaiVoucher.HoatDong);
         
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
                var existsInVoucherNguoiDung = await _context.voucherNguoiDungs.AnyAsync(vnd => vnd.IdVouCher == voucher.IdVoucher && vnd.IdNguoiDung == idUser);
                if (!existsInVoucherNguoiDung)
                {
                    await _context.AddAsync(voucherNguoi);
                    await _context.SaveChangesAsync();
                }
            }
           
        }
    }
}
