using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class SanPhamChiTiet
    {
        [Key]
        public Guid IDChiTietSp { get; set; }

        public string Ma { get; set; }

        public string Day { get; set; }

        public string MoTa { get; set; }

        public int SoLuongTon { get; set; }

        public  double GiaBan { get; set; }

        public double GiaNhap { get; set; }

        public int TrangThai { get; set; }

        public int TrangThaiSale { get; set; }

        public Guid IdProduct { get; set; }
        public Guid IdKieuDeGiay { get; set; }
        public Guid IdXuatXu { get; set; }
        public Guid IdChatLieu { get; set; }
        public Guid IdMauSac { get; set; }
        public Guid IdKichCo { get; set; }
        public Guid IdLoaiGiay { get; set; }
        public Guid IdThuongHieu { get; set; }
    }
}
