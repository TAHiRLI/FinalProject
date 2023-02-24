using Medlab.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Data.Configurations
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.Name).IsRequired(true)
                .HasMaxLength(30);
            builder.Property(x => x.CostPrice).IsRequired(true)
               .HasColumnType("Decimal(18,2)");
            builder.Property(x => x.SalePrice).IsRequired(true)
            .HasColumnType("Decimal(18,2)");
            builder.Property(x => x.DiscountPercent).IsRequired(true)
          .HasColumnType("Decimal(18,2)");
            builder.HasOne(x => x.Product).WithMany(x => x.OrderItems).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Order).WithMany(x => x.OrderItems).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
