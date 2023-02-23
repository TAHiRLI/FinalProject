using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class AppUser:IdentityUser
    {
        public string Fullname { get; set; }
        public string? ImageUrl { get; set; }
        public string? ConnectionId { get; set; }
        public string? PeerId { get; set; }
        public List<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public List<DoctorAppointment> DoctorAppointments { get; set; } = new List<DoctorAppointment>();
        public List<ContactMessage> ContactMessages { get; set; } = new List<ContactMessage>();

        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        
    }
}
