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
    public class XuatXuConfig : IEntityTypeConfiguration<XuatXu>
    {
        public void Configure(EntityTypeBuilder<XuatXu> builder)
        {
            builder.ToTable("XuatXu");
            builder.HasKey(x => x.IdXuatXu);
            builder.Property(x => x.Ma).HasColumnType("varchar(50)");
            builder.Property(x => x.Ten).HasColumnType("nvarchar(1000)");
        }
    }
}
