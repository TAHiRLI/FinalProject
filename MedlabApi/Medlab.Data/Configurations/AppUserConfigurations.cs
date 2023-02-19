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
    internal class AppUserConfigurations : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.Fullname).IsRequired(true).HasMaxLength(50);
            builder.Property(x => x.ImageUrl).IsRequired(false).HasDefaultValue("DEFAULT-USER.jpg").HasMaxLength(200);
            builder.HasOne(x => x.Doctor).WithOne(x => x.AppUser).HasForeignKey<Doctor>(u => u.AppUserId);
        } 
    }
}
