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
    public class PhuongThucThanhToanConfig : IEntityTypeConfiguration<PhuongThucThanhToan>
    {
        public void Configure(EntityTypeBuilder<PhuongThucThanhToan> builder)
        {
            builder.ToTable("PhuongThucThanhToan");
            builder.HasKey(x => x.IdPhuongThucThanhToan);
            builder.Property(x => x.MaPhuongThucThanhToan).HasColumnType("varchar(50)");
            builder.Property(x => x.TenPhuongThucThanhToan).HasColumnType("nvarchar(1000)");
            builder.Property(x => x.MoTa).HasColumnType("nvarchar(max)");
        }
    }
}
