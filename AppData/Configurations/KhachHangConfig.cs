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
            builder.HasKey(c => c.IdKhachHang);
            builder.Property(c => c.MaKhachHang).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(c => c.TenKhachHang).HasColumnType("nvarchar(300)").IsRequired();
            builder.Property(c => c.SDT).HasColumnType("nvarchar(10)").IsRequired();
            builder.Property(c => c.TrangThai).HasColumnType("int").IsRequired();
            builder.HasOne(x => x.NguoiDung).WithMany(x => x.KhachHangs).HasForeignKey(x => x.IdNguoiDung);
        }
    }
}
