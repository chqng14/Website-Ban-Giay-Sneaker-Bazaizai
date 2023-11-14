using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using App_Data.ViewModels.ThongTinGHDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.IRepositories
{
    public interface IThongTinGHRepos
    {
        public IEnumerable<ThongTinGiaoHang> GetAll();
        public IEnumerable<ThongTinGHDTO> GetAllDTO();
        public bool AddThongTinGH(ThongTinGiaoHang item);
        public bool RemoveThongTinGH(ThongTinGiaoHang item);
        public bool EditThongTinGH(ThongTinGiaoHang item);
    }
}
