using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Interfaces
{
    public interface ITrackable
    {
       DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
