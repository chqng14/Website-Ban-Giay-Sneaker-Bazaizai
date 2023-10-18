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
    public class ChatLieuConfig : IEntityTypeConfiguration<ChatLieu>
    {
        public void Configure(EntityTypeBuilder<ChatLieu> builder)
        {
            builder.ToTable("ChatLieu");
            builder.HasKey(x => x.IdChatLieu);
            builder.Property(x => x.MaChatLieu).HasColumnType("varchar(50)");
            builder.Property(x => x.TenChatLieu).HasColumnType("nvarchar(1000)");
        }
    }
}
