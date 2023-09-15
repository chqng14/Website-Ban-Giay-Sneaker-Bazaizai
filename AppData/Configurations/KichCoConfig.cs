using App_Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Configurations
{
    public class KichCoConfig: IEntityTypeConfiguration<KichCo>
    {
        public void Configure(EntityTypeBuilder<KichCo> builder)
        {
            builder.HasKey(e => e.IDKichCo);
            builder.Property(e => e.IDKichCo).HasDefaultValueSql("(newid())");
            builder.Property(c => c.MaKichCo).HasColumnType("nvarchar(1000)").IsRequired(true);
            builder.Property(c => c.SoKichCo).HasColumnType("int").IsRequired(true);
            builder.Property(e => e.TrangThai).HasDefaultValueSql("((0))");
        }
    }
}
