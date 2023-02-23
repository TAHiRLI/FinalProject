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
    internal class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> builder)
        {
            builder.Property(x => x.FullName).IsRequired(true)
                 .HasMaxLength(30);
            builder.Property(x => x.Email).IsRequired(true)
                .HasMaxLength(50);
            builder.Property(x => x.PhoneNumber).IsRequired(true)
                .HasMaxLength(20);
            builder.Property(x => x.Message).IsRequired(true)
                .HasMaxLength(500);
            builder.HasOne(x=> x.AppUser).WithMany(x=> x.ContactMessages).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
