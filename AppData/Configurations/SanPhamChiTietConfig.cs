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
    public class SanPhamChiTietConfig : IEntityTypeConfiguration<SanPhamChiTiet>
    {
        public void Configure(EntityTypeBuilder<SanPhamChiTiet> builder)
        {
            builder.ToTable("SanPhamChiTiet");
            builder.HasKey(x => x.IdChiTietSp);

            builder.HasOne(x => x.ThuongHieu).WithMany(y => y.SanPhamChiTiets).
            HasForeignKey(c => c.IdThuongHieu);


            builder.HasOne(x => x.SanPham).WithMany(y => y.SanPhamChiTiets).
            HasForeignKey(c => c.IdSanPham);


            builder.HasOne(x => x.KieuDeGiay).WithMany(y => y.SanPhamChiTiets).
            HasForeignKey(c => c.IdKieuDeGiay);


            builder.HasOne(x => x.XuatXu).WithMany(y => y.SanPhamChiTiets).
            HasForeignKey(c => c.IdXuatXu);


            builder.HasOne(x => x.ChatLieu).WithMany(y => y.SanPhamChiTiets).
            HasForeignKey(c => c.IdChatLieu);


            builder.HasOne(x => x.MauSac).WithMany(y => y.SanPhamChiTiets).
            HasForeignKey(c => c.IdMauSac);


            builder.HasOne(x => x.KichCo).WithMany(y => y.SanPhamChiTiets).
            HasForeignKey(c => c.IdKichCo);


            builder.HasOne(x => x.LoaiGiay).WithMany(y => y.SanPhamChiTiets).
            HasForeignKey(c => c.IdLoaiGiay);
            builder.Property(x => x.Ma).HasColumnType("varchar(50)");
            builder.Property(x => x.Day).HasColumnType("bit");
            builder.Property(x => x.NoiBat).HasColumnType("bit");
        }
    }
}
