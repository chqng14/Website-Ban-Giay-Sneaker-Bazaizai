using App_Data.Models;
using App_Data.ViewModels.HoaDonChiTietDTO;
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
        public List<HoaDonChiTiet> GetAllHoaDonOnline();
        public bool AddBillDetail(HoaDonChiTiet item);
        public bool RemoveBillDetail(HoaDonChiTiet item);
        public bool EditBillDetail(HoaDonChiTiet item);
        public HoaDonChiTietViewModel GetHoaDonDTO(string idHoaDon);
        //public List<HoaDonChiTiet> FindBillByCode(string ma);
        public HoaDonChiTiet ThemSanPhamVaoHoaDon(HoaDonChiTiet hoaDonChiTiet);
        public string UpdateSoLuong(string idHD, string idSanPham, int SoLuongMoi, string SoluongTon);
        public string XoaSanPhamKhoiHoaDon(string idHD, string idSanPham);
        public List<HoaDonChiTiet> HuyHoaDonChiTiet(string idHD);
        public bool ThanhToanHoaDonChiTiet(string idHD);

    }
}