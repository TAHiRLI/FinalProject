using Medlab.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medlab.Data.Configurations
{
    partial class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired(true)
                 .HasMaxLength(30);
            builder.Property(x => x.Desc).IsRequired(false)
                .HasMaxLength(300);
            builder.Property(x => x.CostPrice).IsRequired(true)
                .HasColumnType("Decimal(18,2)");
            builder.Property(x => x.SalePrice).IsRequired(true)
            .HasColumnType("Decimal(18,2)");
            builder.HasOne(x => x.ProductCategory).WithMany(x => x.Products).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
