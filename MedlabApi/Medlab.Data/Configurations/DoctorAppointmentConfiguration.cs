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
    internal class DoctorAppointmentConfiguration : IEntityTypeConfiguration<DoctorAppointment>
    {
        public void Configure(EntityTypeBuilder<DoctorAppointment> builder)
        {
            builder.Property(x => x.AppUserId).IsRequired(false);
            builder.Property(x => x.DoctorId).IsRequired(false);
            builder.Property(x => x.StartedAt).IsRequired(false);
            builder.Property(x => x.FinishedAt).IsRequired(false);
            builder.Property(x=> x.IsApproved).IsRequired(false);
            builder.Property(x => x.TotalPaid).HasColumnType("decimal(18,2)");
            builder.HasOne(x => x.AppUser).WithMany(x => x.DoctorAppointments).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Doctor).WithMany(x => x.DoctorAppointments).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
