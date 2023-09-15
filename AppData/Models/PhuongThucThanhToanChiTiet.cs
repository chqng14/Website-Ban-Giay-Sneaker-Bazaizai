using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class PhuongThucThanhToanChiTiet
    {
        public Guid IDPhuongThucThanhToanChiTiet { get; set; }
        public Guid IdHoaDon { get; set; }
        public Guid IdThanhToan { get; set; }

        public double SoTien { get; set; }
        public virtual PhuongThucThanhToan PhuongThucThanhToan { get; set; }
        public virtual HoaDon HoaDons { get; set; }
    }
}
