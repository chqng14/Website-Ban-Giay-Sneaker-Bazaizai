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
    public class PhuongThucThanhToanChiTietConfig : IEntityTypeConfiguration<PhuongThucThanhToanChiTiet>
    {
        public void Configure(EntityTypeBuilder<PhuongThucThanhToanChiTiet> builder)
        {
            builder.ToTable("PhuongThucThanhToanChiTiet");
            builder.HasKey(x => x.IdPhuongThucThanhToanChiTiet);
            builder.HasOne(x => x.HoaDons).WithMany(c => c.PhuongThucThanhToanChiTiet).HasForeignKey(c => c.IdHoaDon);

            builder.HasOne(x => x.PhuongThucThanhToan).WithMany(x => x.PhuongThucThanhToanChiTiets).HasForeignKey(c => c.IdThanhToan);

        }
    }
}
