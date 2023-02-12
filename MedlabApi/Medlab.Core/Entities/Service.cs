using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class Service:BaseEntity
    {


        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Icon { get; set; }
        public bool isFeatured { get; set; }
    }
}
