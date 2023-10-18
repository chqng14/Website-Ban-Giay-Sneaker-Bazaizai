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
            builder.ToTable("KichCo");
            builder.HasKey(e => e.IdKichCo);
            builder.Property(c => c.MaKichCo).HasColumnType("varchar(50)");
            builder.Property(c => c.SoKichCo).HasColumnType("int");
            builder.Property(e => e.TrangThai).HasDefaultValueSql("((0))");
        }
    }
}
