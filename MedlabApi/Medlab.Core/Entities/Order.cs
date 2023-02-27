using Medlab.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class Order : BaseEntity, ITrackable
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
        public string? Note { get; set; }
        public bool? OrderStatus { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
