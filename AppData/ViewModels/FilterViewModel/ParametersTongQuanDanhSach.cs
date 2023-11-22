using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.FilterViewModel
{
    public class ParametersTongQuanDanhSach
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string? SearchValue { get; set; }
        public string? IdSanPham { get; set; }
        public string? IdThuongHieu { get; set; }
        public string? IdKieuDeGiay { get; set; }
        public string? IdChatLieu { get; set; }
        public string? IdLoaiGiay { get; set; }
        public string? IdXuatXu { get; set; }
    }
}
