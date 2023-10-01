using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Repositories
{
    public class HoaDonChiTietRepos : IHoaDonChiTietRepos
    {
        BazaizaiContext context;
        public HoaDonChiTietRepos()
        {
            context = new BazaizaiContext();
        }
        public bool AddBillDetail(HoaDonChiTiet item)
        {
            try
            {
                context.hoaDonChiTiets.Add(item);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EditBillDetail(HoaDonChiTiet item)
        {
            try
            {
                var id = context.hoaDonChiTiets.Find(item.IdHoaDonChiTiet);
                context.hoaDonChiTiets.Update(id);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<HoaDonChiTiet> GetAll()
        {
            return context.hoaDonChiTiets.ToList();
        }

        public bool RemoveBillDetail(HoaDonChiTiet item)
        {
            try
            {
                var id = context.hoaDonChiTiets.Find(item.IdHoaDonChiTiet);
                context.hoaDonChiTiets.Remove(id);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
