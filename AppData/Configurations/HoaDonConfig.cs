using App_Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Configurations
{
    public class HoaDonConfig : IEntityTypeConfiguration<HoaDon>
    {
        public void Configure(EntityTypeBuilder<HoaDon> builder)
        {
            builder.ToTable("HoaDon");
            builder.HasKey(x => x.IdHoaDon); 
            builder.HasOne(x => x.ThongTinGiaoHang).WithMany(x => x.HoaDon).HasForeignKey(x => x.IdThongTinGH);
            builder.HasOne(x => x.NguoiDung).WithMany(x => x.HoaDons).HasForeignKey(x => x.IdNguoiDung);
            builder.HasOne(x => x.KhachHang).WithMany(x => x.HoaDons).HasForeignKey(x => x.IdKhachHang);
            builder.HasOne(x => x.Voucher).WithMany(x => x.HoaDon).HasForeignKey(x => x.IdVoucher);
            builder.Property(x => x.MaHoaDon).HasColumnType("varchar(50)");
            builder.Property(x => x.NgayTao).HasColumnType("DateTime");
            builder.Property(x => x.NgayThanhToan).HasColumnType("DateTime");
            builder.Property(x => x.NgayShip).HasColumnType("DateTime");
            builder.Property(x => x.NgayNhan).HasColumnType("DateTime");
            builder.Property(x => x.MoTa).HasColumnType("nvarchar(MAX)");
        }
    }
}
