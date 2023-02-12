using Medlab.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medlab.Data.Configurations
{
    internal class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(x => x.Fullname).IsRequired(true)
                .HasMaxLength(30);
            builder.Property(x => x.ImageUrl).IsRequired(true)
                .HasMaxLength(200);
            builder.Property(x => x.Desc).IsRequired(true)
                .HasMaxLength(200);
            builder.Property(x => x.Positon).IsRequired(true).
                HasMaxLength(50);
            builder.Property(x => x.Instagram).IsRequired(false)
                .HasMaxLength(100);
            builder.Property(x => x.Twitter).IsRequired(false)
                .HasMaxLength(100);
            builder.Property(x => x.Facebook).IsRequired(false)
               .HasMaxLength(100);
            builder.Property(x => x.DetailedDesc).IsRequired(true)
               .HasMaxLength(1000);
            builder.Property(x => x.Email).IsRequired(false)
             .HasMaxLength(100);
            builder.Property(x => x.DepartmentId).IsRequired(false);
            builder.Property(x => x.Salary).HasColumnType("decimal(18,2)");
            builder.HasOne(x => x.Department).WithMany(x => x.Doctors).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
