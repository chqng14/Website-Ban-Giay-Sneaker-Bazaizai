using App_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.ViewModels.DanhGia
{
    public class DanhGiaViewModel
    {
        public string? IdDanhGia { get; set; }
        public string? BinhLuan { get; set; }
        public DateTime? NgayDanhGia { get; set; }
        public int? SaoSp { get; set; }
        public int? SaoVanChuyen { get; set; }
        public int? TrangThai { get; set; }
        public string? IdNguoiDung { get; set; }
        public string? IdSanPhamChiTiet { get; set; }
        public string? ParentId { get; set; }
        public string? TenNguoiDung { get; set; }
        public string? MoTa { get; set; }
        public string? ChatLuongSanPham { get; set; }
        public string? SanPhamTongQuat { get; set; }
        public string? AnhDaiDien { get; set; }
        public int? TongDanhGia { get;set; }
        public string? TenSanPham { get; set; }
        public int? countChuaDuyet { get; set; }
        public string? IdSanPham { get; set; }
        public string? anhSp { get; set; }
    }
}
