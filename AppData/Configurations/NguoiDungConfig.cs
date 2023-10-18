using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Data.Models;
using System.Reflection.Emit;

namespace App_Data.Configurations
{
    public class NguoiDungConfig : IEntityTypeConfiguration<NguoiDung>
    {
        public void Configure(EntityTypeBuilder<NguoiDung> builder)
        {
            builder.ToTable("NguoiDung");
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(c => c.MaNguoiDung).HasColumnType("varchar(50)");
            builder.Property(c => c.TenNguoiDung).HasColumnType("nvarchar(256)");
            builder.Property(c => c.GioiTinh).HasColumnType("int");
            builder.Property(c => c.NgaySinh).HasColumnType("datetime");
            builder.Property(c => c.DiaChi).HasColumnType("nvarchar(max)");
            builder.Property(c => c.TrangThai).HasColumnType("int");
            builder.Property(c => c.AnhDaiDien).HasColumnType("nvarchar(max)");
        }
    }
}
