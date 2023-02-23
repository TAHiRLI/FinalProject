using Medlab.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class ContactMessage : BaseEntity, ITrackable
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}
