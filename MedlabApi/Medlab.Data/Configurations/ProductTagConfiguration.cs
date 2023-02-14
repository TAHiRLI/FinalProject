using Medlab.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medlab.Data.Configurations
{
    public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            builder.HasOne(x => x.Product).WithMany(x => x.ProductTags).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Tag).WithMany(x => x.ProductsTags).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
