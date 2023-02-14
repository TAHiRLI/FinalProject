using Medlab.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medlab.Data.Configurations
{
    internal class ValueConfiguration : IEntityTypeConfiguration<Value>
    {
        public void Configure(EntityTypeBuilder<Value> builder)
        {
            builder.Property(x => x.Name).IsRequired(true)
                .HasMaxLength(30);
            builder.Property(x => x.Desc).IsRequired(true)
                .HasMaxLength(200);
            builder.Property(x => x.Icon).IsRequired(true)
                .HasMaxLength(50);
        }
    }
}
