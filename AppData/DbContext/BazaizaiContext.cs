using App_Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App_Data.DbContext
{
    public class BazaizaiContext : IdentityDbContext<NguoiDung, ChucVu, string>
    {
        public BazaizaiContext()
        {

        }

        public BazaizaiContext(DbContextOptions options) : base(options)
        {
        }



        public DbSet<Anh> Anh { get; set; }
        public DbSet<ChatLieu> ChatLieus { get; set; }
        public DbSet<ChucVu> ChucVus { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<GioHangChiTiet> GioHangChiTiets { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<HoaDonChiTiet> HoaDonChiTiets { get; set; }
        public DbSet<KhuyenMai> KhuyenMais { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<KhuyenMaiChiTiet> KhuyenMaiChiTiets { get; set; }
        public DbSet<KichCo> KichCos { get; set; }
        public DbSet<KieuDeGiay> KieuDeGiays { get; set; }
        public DbSet<LoaiGiay> LoaiGiays { get; set; }
        public DbSet<MauSac> MauSacs { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }
        public DbSet<PhuongThucThanhToanChiTiet> PhuongThucThanhToanChiTiets { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<SanPhamChiTiet> SanPhamChiTiets { get; set; }
        public DbSet<SanPhamYeuThich> SanPhamYeuThichs { get; set; }
        public DbSet<ThongTinGiaoHang> ThongTinGiaoHangs { get; set; }
        public DbSet<ThuongHieu> ThuongHieus { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherNguoiDung> VoucherNguoiDungs { get; set; }
        public DbSet<XuatXu> XuatXus { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    var tableName = entityType.GetTableName();
            //    if (tableName.StartsWith("AspNet"))
            //    {
            //        entityType.SetTableName(tableName.Substring(6));
            //    }
            //}
            //Cấu hình tên bảng tùy chỉnh ở đây
            //modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");
            //modelBuilder.Entity<ChucVu>().ToTable("ChucVu");
            modelBuilder.
               ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\mssqllocaldb;Initial Catalog=DuAnTotNghiep_BazaizaiStore;Integrated Security=True");
            //cái này là db online
            //optionsBuilder.UseSqlServer("Server = tcp:bazaizaidb.database.windows.net,1433; Initial Catalog = bazaizaidb; Persist Security Info = False; User ID = bazaizai; Password = Trinhanh0311; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
        }


    }
}
