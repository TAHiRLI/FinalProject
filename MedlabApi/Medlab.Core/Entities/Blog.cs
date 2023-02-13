using Medlab.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class Blog : BaseEntity, ITrackable
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string? PrevText { get; set; }
        public string ImageUrl { get; set; }
        public int? BlogCategoryId { get; set; }
        public BlogCategory? BlogCategory { get; set; }
        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}
