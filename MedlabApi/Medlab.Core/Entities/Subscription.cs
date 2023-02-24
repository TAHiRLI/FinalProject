using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class Subscription:BaseEntity
    {
        public string Email { get; set; }
        public DateTime LastSentAt { get; set; } = DateTime.UtcNow.AddHours(4);

    }
}
