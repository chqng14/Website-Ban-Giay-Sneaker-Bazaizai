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
    public class KhuyenMaiChiTietConfig : IEntityTypeConfiguration<KhuyenMaiChiTiet>
    {
        public void Configure(EntityTypeBuilder<KhuyenMaiChiTiet> builder)
        {
            builder.ToTable("KhuyenMaiChiTiet");
            builder.HasKey(e => e.IdKhuyenMaiChiTiet);
            builder.Property(e => e.IdSanPhamChiTiet).HasColumnName("IdSanPhamChiTiet");
            builder.Property(e => e.IdKhuyenMai).HasColumnName("IdKhuyenMai");
            builder.Property(e => e.TrangThai).HasDefaultValueSql("((0))");
            builder.Property(e => e.MoTa).HasColumnType("nvarchar(max)");
            builder.HasOne(d => d.SanPhamChiTiet).WithMany(p => p.KhuyenMaiChiTiet).HasForeignKey(d => d.IdSanPhamChiTiet);
            builder.HasOne(d => d.KhuyenMai).WithMany(p => p.KhuyenMaiChiTiet).HasForeignKey(d => d.IdKhuyenMai);
        }
    }
}
