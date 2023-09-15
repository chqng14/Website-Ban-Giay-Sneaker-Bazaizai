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
    public class SanPhamConfig : IEntityTypeConfiguration<SanPham>
    {
        public void Configure(EntityTypeBuilder<SanPham> builder)
        {
            builder.HasKey(x => x.IdSanPham);
            builder.Property(x => x.MaSanPham).HasColumnType("nvarchar(1000)");
            builder.Property(x => x.TenSanPham).HasColumnType("nvarchar(1000)");
        }
    }
}
