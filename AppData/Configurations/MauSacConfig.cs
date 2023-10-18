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
    public class MauSacConfig : IEntityTypeConfiguration<MauSac>
    {
        public void Configure(EntityTypeBuilder<MauSac> builder)
        {
            builder.ToTable("MauSac");
            builder.HasKey(x => x.IdMauSac);
            builder.Property(c => c.MaMauSac).HasColumnType("varchar(50)");
            builder.Property(c => c.TenMauSac).HasColumnType("nvarchar(1000)");
            builder.Property(c => c.TrangThai).HasColumnType("int");
        }
    }
}
