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
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.Property(x => x.Name).IsRequired(true)
                .HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired(true)
                .HasMaxLength(500);
            builder.Property(x => x.ImageUrl).IsRequired(true)
                .HasMaxLength(200);
            builder.Property(x => x.DetailedDesc).IsRequired(true)
                .HasMaxLength(1000);
            builder.Property(x => x.Icon).IsRequired(true)
                .HasMaxLength(100);
            
        }
    }
}
