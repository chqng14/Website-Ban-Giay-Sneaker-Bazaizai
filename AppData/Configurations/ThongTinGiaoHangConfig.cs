using App_Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Configurations
{
    public class ThongTinGiaoHangConfig : IEntityTypeConfiguration<ThongTinGiaoHang>
    {
        public void Configure(EntityTypeBuilder<ThongTinGiaoHang> builder)
        {
            builder.ToTable("ThongTinGiaoHang");
            builder.HasKey(c => c.IdThongTinGH);
            builder.Property(c => c.TenNguoiNhan).HasColumnType("nvarchar(300)");
            builder.Property(c => c.SDT).HasColumnType("nvarchar(10)");
            builder.Property(c => c.DiaChi).HasColumnType("nvarchar(max)");

            builder.HasOne(c => c.NguoiDungs).WithMany(c => c.ThongTinGiaoHangs).HasForeignKey(c => c.IdNguoiDung);
        }
    }
}
