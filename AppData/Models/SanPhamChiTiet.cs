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
        public string? IdChiTietSp { get; set; }
        public string? Ma { get; set; }
        public bool? Day { get; set; }
        public string? MoTa { get; set; }
        public int? SoLuongTon { get; set; }
        public int? SoLuongDaBan { get; set; }
        public DateTime? NgayTao { get; set; }
        public bool? NoiBat { get; set; }
        public double? GiaBan { get; set; }
        public double? GiaNhap { get; set; }
        public double? GiaThucTe { get; set; }
        public double? KhoiLuong { get; set; }
        public int? TrangThai { get; set; }
        public int? TrangThaiSale { get; set; }
        public string IdSanPham { get; set; }
        public string IdKieuDeGiay { get; set; }
        public string IdXuatXu { get; set; }
        public string IdChatLieu { get; set; }
        public string IdMauSac { get; set; }
        public string IdKichCo { get; set; }
        public string IdLoaiGiay { get; set; }
        public string IdThuongHieu { get; set; }
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
        public virtual IEnumerable<SanPhamYeuThich> SanPhamYeuThichs { get; set; }
        public virtual IEnumerable<GioHangChiTiet> GioHangChiTiet { get; set; }
        public virtual IEnumerable<KhuyenMaiChiTiet> KhuyenMaiChiTiet { get; set; }
        public virtual List<DanhGia> DanhGias { get; set; }
    }
}
