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
    internal class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.Property(x => x.Key)
                .IsRequired(true)
                .HasMaxLength(200);
            builder.Property(x => x.Value)
                .IsRequired(false)
                .HasMaxLength(1000);
        }
    }
}
