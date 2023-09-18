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
    public class SanPhamYeuThichConfig : IEntityTypeConfiguration<SanPhamYeuThich>
    {
        public void Configure(EntityTypeBuilder<SanPhamYeuThich> builder)
        {
            builder.HasKey(c => c.IdSanPhamYeuThich);
            builder.Property(e => e.IdSanPhamYeuThich).HasDefaultValueSql("(newid())");
            builder.HasOne(d => d.NguoiDung).WithMany(p => p.SanPhamYeuThich).HasForeignKey(d => d.IdNguoiDung);
            builder.HasOne(d => d.SanPhamChiTiet).WithMany(p => p.SanPhamYeuThichs).HasForeignKey(d => d.IdSanPhamChiTiet);
            builder.Property(e => e.TrangThai).HasDefaultValueSql("((0))");
        }
    }
}
