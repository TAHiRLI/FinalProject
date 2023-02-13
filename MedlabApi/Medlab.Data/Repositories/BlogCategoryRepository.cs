using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Data.Repositories
{
    public class BlogCategoryRepository:EntityRepository<BlogCategory> , IBlogCategoryRepository
    {
        private readonly MedlabDbContext _context;

        public BlogCategoryRepository(MedlabDbContext context):base(context)
        {
            this._context = context;
        }
    }
}
