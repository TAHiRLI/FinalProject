using Medlab.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class Doctor : BaseEntity, ITrackable, ISocialLinks
    {
        public string Fullname { get; set; }
        public string ImageUrl { get; set; }
        public string Desc { get; set; }
        public string DetailedDesc { get; set; }
        public decimal Salary { get; set; }
        public bool Gender { get; set; }  // true if male
        public string Positon { get; set; }
        public string? Office { get; set; }
        public decimal MeetingPrice { get; set; }

        public bool IsFeatured { get; set; }

        public string? Email { get; set; }

        public string? Instagram { get; set;}
        public string? Facebook { get; set;}
        public string? Twitter { get; set;}
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(4);


        public Department? Department { get; set; }
        public int? DepartmentId { get; set; }
        public List<Blog> Blogs { get; set; } = new List<Blog>();
        public List<DoctorAppointment> DoctorAppointments { get; set; } = new List<DoctorAppointment>();


        public string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

    }
}
