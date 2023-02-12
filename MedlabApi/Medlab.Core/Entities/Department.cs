using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class Department:BaseEntity
    {
        public string Name { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}
