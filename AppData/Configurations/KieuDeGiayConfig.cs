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
    public class KieuDeGiayConfig : IEntityTypeConfiguration<KieuDeGiay>
    {
        public void Configure(EntityTypeBuilder<KieuDeGiay> builder)
        {
            builder.ToTable("KieuDeGiay");
            builder.HasKey(x => x.IdKieuDeGiay);
            builder.Property(x => x.MaKieuDeGiay).HasColumnType("varchar(50)");
            builder.Property(x => x.TenKieuDeGiay).HasColumnType("nvarchar(1000)");
        }
    }
}
