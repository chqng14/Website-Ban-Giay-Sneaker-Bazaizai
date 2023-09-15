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
    public class LoaiGiayConfig : IEntityTypeConfiguration<LoaiGiay>
    {
        public void Configure(EntityTypeBuilder<LoaiGiay> builder)
        {
            builder.HasKey(x => x.IdLoaiGiay);
            builder.Property(c => c.MaLoaiGiay).HasColumnType("nvarchar(1000)").IsRequired(true);
            builder.Property(c => c.TenLoaiGiay).HasColumnType("nvarchar(1000)").IsRequired(true);
            builder.Property(c => c.TrangThai).HasColumnType("int");
        }
    }
}
