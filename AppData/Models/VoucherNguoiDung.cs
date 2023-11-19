using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class VoucherNguoiDung
    {
        public string? IdVouCherNguoiDung { get; set; }
        public string? IdNguoiDung { get; set; }
        public string? IdVouCher { get; set; }
        public DateTime? NgayNhan { get; set; }
        public int? TrangThai { get; set; }
        public virtual NguoiDung? NguoiDungs { get; set; }
        public virtual Voucher? Vouchers { get; set; }

    }
}
