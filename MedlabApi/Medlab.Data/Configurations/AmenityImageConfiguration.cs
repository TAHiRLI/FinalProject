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
    internal class AmenityImageConfiguration : IEntityTypeConfiguration<AmenityImage>
    {
        public void Configure(EntityTypeBuilder<AmenityImage> builder)
        {
            builder.Property(x => x.ImageUrl).IsRequired(true)
                .HasMaxLength(200);

        }
    }
}
