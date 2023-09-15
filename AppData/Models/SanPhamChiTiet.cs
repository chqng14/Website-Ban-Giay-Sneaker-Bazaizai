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

        public double GiaBan { get; set; }

        public double GiaNhap { get; set; }

        public int TrangThai { get; set; }

        public int TrangThaiSale { get; set; }

        public Guid IdSanPham { get; set; }
        public Guid IdKieuDeGiay { get; set; }
        public Guid IdXuatXu { get; set; }
        public Guid IdChatLieu { get; set; }
        public Guid IdMauSac { get; set; }
        public Guid IdKichCo { get; set; }
        public Guid IdLoaiGiay { get; set; }
        public Guid IdThuongHieu { get; set; }
        public virtual SanPham SanPham { get; set; }
        public virtual KieuDeGiay KieuDeGiay { get; set; }
        public virtual XuatXu XuatXu { get; set; }
        public virtual ChatLieu ChatLieu { get; set; }
        public virtual MauSac MauSac { get; set; }
        public virtual KichCo KichCo { get; set; }
        public virtual LoaiGiay LoaiGiay { get; set; }
        public virtual ThuongHieu ThuongHieu { get; set; }
        public virtual IEnumerable<Anh> Anh { get; set; }
        public virtual IEnumerable<HoaDonChiTiet> HoaDonChiTiet { get; set; }
        public virtual IEnumerable<GioHangChiTiet> GioHangChiTiet { get; set; }
        public virtual IEnumerable<KhuyenMaiChiTiet> KhuyenMaiChiTiet { get; set; }
    }
}
