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
    public class VoucherNguoiDungConfig : IEntityTypeConfiguration<VoucherNguoiDung>
    {
        public void Configure(EntityTypeBuilder<VoucherNguoiDung> builder)
        {
            builder.ToTable("VoucherNguoiDung");
            builder.HasKey(c => c.IdVouCherNguoiDung);

            builder.HasOne(x => x.Vouchers).WithMany(c => c.VoucherNguoiDungs).HasForeignKey(c => c.IdVouCher);

            builder.HasOne(x => x.NguoiDungs).WithMany(c => c.VoucherNguoiDungs).HasForeignKey(c => c.IdNguoiDung);
        }
    }
}
