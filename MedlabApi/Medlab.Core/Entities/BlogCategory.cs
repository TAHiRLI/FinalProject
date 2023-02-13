using Medlab.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class BlogCategory : BaseEntity
    {
        public string Name { get; set; }
        public List<Blog> Blogs { get; set; } = new List<Blog>();
    }
}
