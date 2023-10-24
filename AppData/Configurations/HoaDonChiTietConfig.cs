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
    public class HoaDonChiTietConfig : IEntityTypeConfiguration<HoaDonChiTiet>
    {
        public void Configure(EntityTypeBuilder<HoaDonChiTiet> builder)
        {
            builder.ToTable("HoaDonChiTiet");
            builder.HasKey(x => x.IdHoaDonChiTiet);
            builder.Property(x => x.SoLuong).HasColumnType("int");
            builder.Property(x => x.TrangThai).HasColumnType("int");
            builder.HasOne(x => x.HoaDon).WithMany(x => x.HoaDonChiTiet).HasForeignKey(x => x.IdHoaDon);
            builder.HasOne(x => x.SanPhamChiTiet).WithMany(x => x.HoaDonChiTiet).HasForeignKey(x => x.IdSanPhamChiTiet);
        }
    }
}
