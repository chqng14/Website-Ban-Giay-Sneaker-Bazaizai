using App_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.ThongKe
{
    public class ThongKeViewModel
    {
        public DateTime NgayTao { get; set; }
        public int SoLuong { get; set; }
        public double GiaGoc { get; set; }
        public double GiaBan { get; set; }
        public double Tong { get; set; }
    }
}
