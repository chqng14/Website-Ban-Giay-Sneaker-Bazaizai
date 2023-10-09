using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models.ViewModels.SanPhamChiTiet
{
    public class SanPhamChiTietViewModel
    {
        public string? IdChiTietSp { get; set; }

        public string? Day { get; set; }

        public string? MoTa { get; set; }

        public int? SoLuongTon { get; set; }

        public double? GiaBan { get; set; }

        public double? GiaNhap { get; set; }

        public string? SanPham { get; set; }
        public string? KieuDeGiay { get; set; }
        public string? XuatXu { get; set; }
        public string? ChatLieu { get; set; }
        public string? MauSac { get; set; }
        public int? KichCo { get; set; }
        public string? LoaiGiay { get; set; }
        public string? ThuongHieu { get; set; }

        public List<string>? ListTenAnh { get; set; }
    }
}
