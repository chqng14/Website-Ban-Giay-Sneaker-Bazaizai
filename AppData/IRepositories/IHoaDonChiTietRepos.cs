using App_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.IRepositories
{
    public interface IHoaDonChiTietRepos
    {
        public IEnumerable<HoaDonChiTiet> GetAll();
        public bool AddBillDetail(HoaDonChiTiet item);
        public bool RemoveBillDetail(HoaDonChiTiet item);
        public bool EditBillDetail(HoaDonChiTiet item);
        //public List<HoaDonChiTiet> FindBillByCode(string ma);
    }
}
