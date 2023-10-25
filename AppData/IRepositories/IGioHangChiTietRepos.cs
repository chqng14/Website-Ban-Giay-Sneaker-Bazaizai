using App_Data.Models;
using App_Data.ViewModels.GioHangChiTiet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.IRepositories
{
    public interface IGioHangChiTietRepos
    {
        public IEnumerable<GioHangChiTiet> GetAll();
        public bool AddCartDetail(GioHangChiTiet item);
        public bool RemoveCartDetail(GioHangChiTiet item);
        public bool EditCartDetail(GioHangChiTiet item);
        public IEnumerable<GioHangChiTietDTO> GetAllGioHangDTO();
        public Task<List<SanPhamGioHangViewModel>> GetAllSanPhamGioHangWhenLoginAynsc(string idNguoiDung);
    }
}
