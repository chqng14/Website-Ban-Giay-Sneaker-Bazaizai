using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.ViewModels.VoucherNguoiDung;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                            TrangThai = (int)TrangThaiVoucherNguoiDung.KhaDung
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
    }
}
