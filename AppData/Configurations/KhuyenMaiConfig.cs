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
    public class KhuyenMaiConfig : IEntityTypeConfiguration<KhuyenMai>
    {
        public void Configure(EntityTypeBuilder<KhuyenMai> builder)
        {
            builder.ToTable("KhuyenMai");
            builder.HasKey(e => e.IdKhuyenMai);
            builder.Property(e => e.TenKhuyenMai).HasMaxLength(1000);
            builder.Property(e => e.MaKhuyenMai).HasColumnType("varchar(50)");
            builder.Property(e => e.MucGiam).HasColumnType("decimal(18, 0)").HasDefaultValueSql("((0))");
            builder.Property(e => e.NgayKetThuc).HasColumnType("datetime");
            builder.Property(e => e.NgayBatDau).HasColumnType("datetime");
            builder.Property(e => e.LoaiHinhKM).HasMaxLength(1000);
            builder.Property(e => e.TrangThai).HasDefaultValueSql("((0))");
            builder.Property(e => e.Url).HasColumnType("nvarchar(max)");
        }
    }
}
