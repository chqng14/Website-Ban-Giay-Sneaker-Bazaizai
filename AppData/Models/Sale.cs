using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class Sale
    {
        public Guid IDSale { get; set; }
        public string Ma { get; set; }
        public string TenKhuyenMai { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string LoạiHinhKM { get; set; }
        public float MucGiam { get; set; }
        public string PhamVi { get; set; }

        public int TrangThai { get; set; }
        public virtual ICollection<SaleDetails> SaleDetails { get; set; }

    }
}
