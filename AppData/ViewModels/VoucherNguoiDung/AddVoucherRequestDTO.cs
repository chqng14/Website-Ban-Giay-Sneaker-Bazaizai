using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.VoucherNguoiDung
{
    public class AddVoucherRequestDTO
    {
        public string MaVoucher { get; set; }
        public List<string> UserId { get; set; }
    }
}
