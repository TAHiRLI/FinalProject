using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class Value:BaseEntity
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Desc { get; set; }
    }
}
