using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models.ViewModels.SanPhamChiTiet
{
    public class SanPhamChiTietDTO
    {
        public string? IdChiTietSp { get; set; }

        public string? Day { get; set; }

        public string? MoTa { get; set; }

        public int? SoLuongTon { get; set; }

        public double? GiaBan { get; set; }

        public double? GiaNhap { get; set; }

        public string? IdSanPham { get; set; }
        public string? IdKieuDeGiay { get; set; }
        public string? IdXuatXu { get; set; }
        public string? IdChatLieu { get; set; }
        public string? IdMauSac { get; set; }
        public string? IdKichCo { get; set; }
        public string? IdLoaiGiay { get; set; }
        public string? IdThuongHieu { get; set; }
    }
}
