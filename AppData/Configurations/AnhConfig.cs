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
    public class AnhConfig : IEntityTypeConfiguration<Anh>
    {
        public void Configure(EntityTypeBuilder<Anh> builder)
        {
            builder.ToTable("Anh");
            builder.HasKey(x => x.IdAnh);
            builder.Property(x => x.Url).HasColumnType("nvarchar(max)");
            builder.HasOne(x => x.SanPhamChiTiets).WithMany(y => y.Anh).
            HasForeignKey(c => c.IdSanPhamChiTiet);
        }
    }
}
