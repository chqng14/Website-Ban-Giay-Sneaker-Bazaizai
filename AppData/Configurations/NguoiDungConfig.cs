﻿using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Data.Models;

namespace App_Data.Configurations
{
    public class NguoiDungConfig : IEntityTypeConfiguration<NguoiDung>
    {
        public void Configure(EntityTypeBuilder<NguoiDung> builder)
        {
            builder.HasKey(c => c.IdNguoiDung);
            builder.Property(c => c.MaNguoiDung).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(c => c.TenNguoiDung).HasColumnType("nvarchar(300)").IsRequired();
            builder.Property(c => c.GioiTinh).HasColumnType("int").IsRequired();
            builder.Property(c => c.NgaySinh).HasColumnType("datetime").IsRequired();
            builder.Property(c => c.SDT).HasColumnType("nvarchar(10)").IsRequired();
            builder.Property(c => c.MatKhau).HasColumnType("nvarchar(300)").IsRequired();
            builder.Property(c => c.Email).HasColumnType("nvarchar(300)").IsRequired();
            builder.Property(c => c.TenDangNhap).HasColumnType("nvarchar(300)").IsRequired();
            builder.Property(c => c.TrangThai).HasColumnType("int").IsRequired();
            builder.Property(c => c.AnhDaiDien).HasColumnType("nvarchar(300)").IsRequired(false);
            builder.HasOne(x => x.ChucVu).WithMany(x => x.NguoiDungs).HasForeignKey(x => x.IdChucVu);

        }
    }
}