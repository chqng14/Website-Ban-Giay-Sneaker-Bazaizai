using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class SaleDetails
    {
        public Guid IDSaleDetail { get; set; }
        public Guid IDSale { get; set; }
        public Guid IDChiTietSp { get; set; }      
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
        public virtual Sale Sale { get; set; }
    }
}
