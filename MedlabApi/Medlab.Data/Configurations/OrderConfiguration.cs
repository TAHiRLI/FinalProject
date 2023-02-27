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
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Fullname).IsRequired(true)
                 .HasMaxLength(30);
            builder.Property(x => x.Email).IsRequired(true)
                .HasMaxLength(50);
            builder.Property(x => x.Address1).IsRequired(true)
                .HasMaxLength(200);
            builder.Property(x => x.Address2).IsRequired(false)
                .HasMaxLength(200);
            builder.Property(x => x.PhoneNumber).IsRequired(true)
                .HasMaxLength(20);
            builder.Property(x => x.Note).IsRequired(false)
                .HasMaxLength(500);
            builder.Property(x => x.ZipCode).IsRequired(true)
                .HasMaxLength(10);
            builder.Property(x => x.OrderStatus).IsRequired(false);
            builder.HasOne(x=> x.AppUser).WithMany(x=> x.Orders).OnDelete(DeleteBehavior.SetNull);

        }
    }
}
