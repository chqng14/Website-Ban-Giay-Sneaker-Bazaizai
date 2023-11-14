using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.SanPhamChiTietDTO
{
    public class ResponseCreateDTO
    {
        public bool Success { get; set; }
        public string? IdChiTietSp { get; set; }
        public string? DescriptionErr { get; set; }
    }
}
