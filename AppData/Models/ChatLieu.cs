using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class ChatLieu
    {
        public string? IdChatLieu { get; set; }
        public string? MaChatLieu { get; set; }
        public string? TenChatLieu { get; set; }
        public int? TrangThai { get; set; }
        public virtual List<SanPhamChiTiet> SanPhamChiTiets { get; set; }
    }
}
