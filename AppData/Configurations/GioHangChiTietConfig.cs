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
    public class GioHangChiTietConfig : IEntityTypeConfiguration<GioHangChiTiet>
    {
        public void Configure(EntityTypeBuilder<GioHangChiTiet> builder)
        {
            builder.ToTable("GioHangChiTiet");
            builder.HasKey(x => x.IdGioHangChiTiet); 
            builder.Property(x => x.Soluong).HasColumnType("int");
            builder.Property(x => x.TrangThai).HasColumnType("int");
            builder.HasOne(x => x.GioHang).WithMany(x => x.GioHangChiTiet).HasForeignKey(x => x.IdNguoiDung);
            builder.HasOne(x => x.SanPhamChiTiet).WithMany(x => x.GioHangChiTiet).HasForeignKey(x => x.IdSanPhamCT);
        }
    }
}
