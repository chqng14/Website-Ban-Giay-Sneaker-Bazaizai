using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.FilterDTO
{
    public class FilterAdminDTO
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string? searchValue { get; set; }
        public string? SanPham { get; set; }
        public string? ThuongHieu { get; set; }
        public string? MauSac { get; set; }
        public string? TheLoai { get; set; }
        public string? KichCo { get; set; }
        public string? Sort { get; set; }
    }
}
