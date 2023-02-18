using Medlab.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class DoctorAppointment : BaseEntity, ITrackable
    {
        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public bool? IsApproved { get; set; }
        public decimal TotalPaid { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public DateTime MeetingDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}
