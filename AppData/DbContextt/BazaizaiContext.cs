using App_Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.DbContextt
{
    public class BazaizaiContext : DbContext
    {
        public BazaizaiContext()
        {
                
        }
     
        public BazaizaiContext(DbContextOptions options) : base(options)
        {
        }

      

        public DbSet<Anh> Anh { get; set; }
        public DbSet<ChatLieu> ChatLieus { get; set; }
        public DbSet<ChucVu> chucVus { get; set; }
        public DbSet<GioHang> gioHangs { get; set; }
        public DbSet<GioHangChiTiet> gioHangChiTiets { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<HoaDonChiTiet> hoaDonChiTiets { get; set; }
        public DbSet<KhuyenMai> khuyenMais { get; set; }
        public DbSet<KhuyenMaiChiTiet> khuyenMaiChiTiets { get; set; }
        public DbSet<KichCo> kichCos { get; set; }
        public DbSet<KieuDeGiay> kieuDeGiays { get; set; }
        public DbSet<LoaiGiay> LoaiGiays { get; set; }
        public DbSet<MauSac> mauSacs { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }
        public DbSet<PhuongThucThanhToanChiTiet> phuongThucThanhToanChiTiets { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<SanPhamChiTiet> sanPhamChiTiets { get; set; }
        public DbSet<SanPhamYeuThich> sanPhamYeuThiches { get; set; }
        public DbSet<ThongTinGiaoHang> thongTinGiaoHangs { get; set; }
        public DbSet<ThuongHieu> thuongHieus { get; set; }
        public DbSet<Voucher> vouchers { get; set; }
        public DbSet<VoucherNguoiDung> voucherNguoiDungs { get; set; }
        public DbSet<XuatXu> xuatXus { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.
               ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=MSI;Initial Catalog=DuAnTotNghiep_BazaizaiStore;Integrated Security=True");
        }

    
    }
}
