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
    internal class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem
        >
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.Property(x => x.Count).IsRequired(true);
            builder.HasOne(x => x.Product).WithMany(x=> x.BasketItems);
            builder.HasOne(x => x.AppUser).WithMany(x => x.BasketItems);

        }
    }
}
