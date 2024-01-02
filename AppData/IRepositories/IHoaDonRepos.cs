using App_Data.Models;
using App_Data.ViewModels.HoaDon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.IRepositories
{
    public interface IHoaDonRepos
    {
        public HoaDon TaoHoaDonTaiQuay(HoaDon hoaDon);
        public List<HoaDonViewModel> GetHoaDon();
        public List<HoaDon> GetHoaDonUpdate();
        public List<HoaDonChoDTO> GetAllHoaDonCho();
        public bool AddBill(HoaDon item);
        public bool EditBill(HoaDon item);
        public string HuyHoaDon(string maHD, string lyDoHuy, string idUser);
        public bool ThanhToanTaiQuay(HoaDon item);
        public bool CheckKhachHang(string idThongTin);
    }
}
