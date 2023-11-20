using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.VoucherNguoiDung
{
    public class VoucherTaiQuayDto
    {
        public string? IdVouCherNguoiDung { get; set; }
        public string? IdNguoiDung { get; set; }
        public string? IdVouCher { get; set; }
        public int? TrangThai { get; set; }
        public string? MaVoucher { get; set; }
        public string? TenVoucher { get; set; }
        public int? DieuKien { get; set; }
        public int? LoaiHinhUuDai { get; set; }
        public double? MucUuDai { get; set; }
        public bool HinhThuc { get; set; }
    }
}
