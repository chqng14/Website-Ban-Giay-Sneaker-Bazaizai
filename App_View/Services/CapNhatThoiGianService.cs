using App_Data.DbContextt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
                .Where(p => p.NgayKetThuc <= DateTime.Now && p.TrangThai != 5)
                .ToList();

            foreach (var sale in ngayKetThucSale)
            {
                sale.TrangThai = 5;
            }
            _dbContext.SaveChanges();
        }
        
    }
}
