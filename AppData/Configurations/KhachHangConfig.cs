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
    public class KhachHangConfig :IEntityTypeConfiguration<KhachHang>
    {
        public void Configure(EntityTypeBuilder<KhachHang> builder)
        {
            builder.ToTable("KhachHang");
            builder.HasKey(c => c.IdKhachHang);
            builder.Property(c => c.TenKhachHang).HasColumnType("nvarchar(300)");
            builder.Property(c => c.SDT).HasColumnType("nvarchar(10)");
            builder.Property(c => c.TrangThai).HasColumnType("int");
            builder.HasOne(x => x.NguoiDung).WithMany(x => x.KhachHangs).HasForeignKey(x => x.IdNguoiDung);
        }
    }
}
