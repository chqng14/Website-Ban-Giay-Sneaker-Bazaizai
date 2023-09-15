using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class Voucher
    {
        public Guid IdVoucher { get; set; }
        public string MaVoucher { get; set; }
        public string TenVoucher { get; set; }
        public string DieuKien { get; set; }
        public string LoaiHinhUuDai { get; set; }
        public int SoLuong { get; set; }
        public double MucUuDai { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int TrangThai { get; set; }
        public virtual IEnumerable<VoucherNguoiDung> VoucherNguoiDungs { get; set; }
        public virtual List<HoaDon> HoaDon { get; set; }
    }
}
