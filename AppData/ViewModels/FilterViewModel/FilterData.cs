using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.FilterViewModel
{
    public class FilterData
    {
        public double GiaMin { get; set; }
        public double GiaMax { get; set;}
        public List<string>? LstTheLoai { get; set; }
        public List<string>? LstMauSac { get; set; }
        public List<int>? LstKichCo { get; set; }
        public List<int>? LstRating { get; set; }
        public int TrangHienTai { get; set; } = 1;
        public int SoLuongSanPhamTrenMotTrang { get; set; } = 8;
        public string? Sort { get; set; }
        public string? Brand { get; set;}
    }
}
