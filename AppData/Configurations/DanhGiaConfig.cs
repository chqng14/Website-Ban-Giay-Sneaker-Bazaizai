using App_Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Configurations
{
    public class DanhGiaConfig : IEntityTypeConfiguration<DanhGia>
    {
        public void Configure(EntityTypeBuilder<DanhGia> builder)
        {
            builder.ToTable("DanhGia");
            builder.HasKey(e => e.IdDanhGia);
            builder.Property(c => c.BinhLuan).HasColumnType("nvarchar(max)");
            builder.Property(e => e.TrangThai).HasDefaultValueSql("((0))");
            builder.HasOne(d => d.NguoiDung).WithMany(p => p.DanhGias).HasForeignKey(d => d.IdNguoiDung);
            builder.HasOne(d => d.SanPhamChiTiet).WithMany(p => p.DanhGias).HasForeignKey(p=>p.IdSanPhamChiTiet);
            //builder.HasOne(d => d.ParentDanhGia).WithMany(p => p.ChildDanhGias).HasForeignKey(d => d.ParentId);
        }
    }
}
