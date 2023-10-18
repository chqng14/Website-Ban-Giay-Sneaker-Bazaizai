using App_Data.Models;
using App_Data.ViewModels.HoaDonChiTietDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.HoaDon
{
    public class HoaDonChoDTO
    {
        public string Id { get; set; }
        public string? IdNguoiDung { get; set; }
        public string? MaHoaDon { get; set; }
        public int? TrangThaiGiaoHang { get; set; }
        public int? TrangThaiThanhToan { get; set; }
        public List<HoaDonChiTiet>? hoaDonChiTietDTOs { get; set; }
    }
}
