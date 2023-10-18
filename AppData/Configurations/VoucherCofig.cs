using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_Data.Models;

namespace App_Data.Configurations
{
    public class VoucherCofig : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("Voucher");
            builder.HasKey(c => c.IdVoucher);
            builder.Property(c => c.MaVoucher).HasColumnType("varchar(50)");
            builder.Property(c => c.TenVoucher).HasColumnType("nvarchar(1000)");
            builder.Property(c => c.SoLuong).HasColumnType("int");
            builder.Property(c => c.MucUuDai);
        }
    }
}
