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
    public class ThuongHieuConfig : IEntityTypeConfiguration<ThuongHieu>
    {
        public void Configure(EntityTypeBuilder<ThuongHieu> builder)
        {
            builder.ToTable("ThuongHieu");
            builder.HasKey(e => e.IdThuongHieu);       
            builder.Property(e => e.MaThuongHieu).HasColumnType("varchar(50)");
            builder.Property(c => c.TenThuongHieu).HasColumnType("nvarchar(1000)");
            builder.Property(e => e.TrangThai).HasDefaultValueSql("((0))");
        }
    }
}
