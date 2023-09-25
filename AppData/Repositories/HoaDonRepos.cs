using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Repositories
{
    public class HoaDonRepos : IHoaDonRepos
    {
        BazaizaiContext context;
        public HoaDonRepos()
        {
            context = new BazaizaiContext();
        }
        
        public bool AddBill(HoaDon item)
        {
            try
            {
                context.HoaDons.Add(item);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EditBill(HoaDon item)
        {
            try
            {
                var idhoadon = context.HoaDons.Find(item.IdHoaDon);
                context.HoaDons.Update(idhoadon);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<HoaDon> FindBillByCode(string ma)
        {
            return context.HoaDons.Where(c=>c.MaHoaDon.Contains(ma)).ToList();
        }

        public IEnumerable<HoaDon> GetAll()
        {
            return context.HoaDons.ToList();
        }

        public bool RemoveBill(HoaDon item)
        {
            try
            {
                var idhoadon = context.HoaDons.Find(item.IdHoaDon);
                context.HoaDons.Remove(idhoadon);
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
