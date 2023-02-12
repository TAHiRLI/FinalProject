using Medlab.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Data.Configurations
{
    public class SliderConfiguration : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.Property(x => x.Title).IsRequired(true)
                  .HasMaxLength(50);
            builder.Property(x => x.Desc).IsRequired(true)
                .HasMaxLength(100);
            builder.Property(x => x.BtnText).IsRequired(true)
                .HasMaxLength(50);
            builder.Property(x => x.BtnUrl).IsRequired(true)
                .HasMaxLength(200);

            builder.Property(x => x.ImageUrl).IsRequired(true)
                .HasMaxLength(200);
            builder.Property(x => x.Order);
        }
    }
}
