using Medlab.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class ProductReview : BaseEntity, ITrackable
    {
        public string Text { get; set; }
        public int Rate { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public bool IsApproved { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}
