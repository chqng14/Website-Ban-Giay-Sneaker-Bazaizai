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
            builder.HasKey(e => e.IDKhuyenMaiChiTiet);
            builder.Property(e => e.IDKhuyenMaiChiTiet).HasDefaultValueSql("(newid())");
            builder.Property(e => e.IDSanPhamChiTiet).HasColumnName("IDSanPhamChiTiet");
            builder.Property(e => e.IDKhuyenMai).HasColumnName("IDKhuyenMai");
            builder.Property(e => e.TrangThai).HasDefaultValueSql("((0))");
            builder.Property(e => e.MoTa).HasMaxLength(500);
            builder.HasOne(d => d.SanPhamChiTiet).WithMany(p => p.KhuyenMaiChiTiet).HasForeignKey(d => d.IDSanPhamChiTiet);
            builder.HasOne(d => d.KhuyenMai).WithMany(p => p.KhuyenMaiChiTiet).HasForeignKey(d => d.IDKhuyenMai);
        }
    }
}
