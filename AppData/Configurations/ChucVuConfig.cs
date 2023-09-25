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
    public class ChucVuConfig : IEntityTypeConfiguration<ChucVu>
    {
        public void Configure(EntityTypeBuilder<ChucVu> builder)
        {
            builder.HasKey(c => c.IdChucVu);
            builder.Property(c => c.MaChucVu).HasColumnType("nvarchar(100)");
            builder.Property(c => c.TenChucVu).HasColumnType("nvarchar(300)");
            builder.Property(c => c.TrangThai).HasColumnType("int");
        }
    }
}
