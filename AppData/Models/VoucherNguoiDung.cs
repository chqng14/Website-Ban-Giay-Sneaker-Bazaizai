using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class VoucherNguoiDung
    {
        public Guid IdVouCherNguoiDung { get; set; }
        public Guid IdNguoiDung { get; set; }
        public Guid IdVouCher { get; set; }
        public int TrangThai { get; set; }
        public virtual NguoiDung NguoiDungs { get; set; }
        public virtual Voucher Vouchers { get; set; }

    }
}
