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
    internal class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.Property(x => x.Title).IsRequired(true)
                 .HasMaxLength(100);
            builder.Property(x => x.Text).IsRequired(true)
                .HasMaxLength(3000);
            builder.Property(x => x.ImageUrl).IsRequired(true)
                .HasMaxLength(200);
            builder.Property(x => x.PrevText).IsRequired(false)
                .HasMaxLength(300);
            builder.HasOne(x => x.BlogCategory).WithMany(x => x.Blogs).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x=> x.Doctor).WithMany(x => x.Blogs).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
