using App_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.IRepositories
{
    public interface IHoaDonRepos
    {
        public IEnumerable<HoaDon> GetAll();
        public bool AddBill(HoaDon item);
        public bool RemoveBill(HoaDon item);
        public bool EditBill(HoaDon item);
        public List<HoaDon> FindBillByCode(string ma);
    }
}
