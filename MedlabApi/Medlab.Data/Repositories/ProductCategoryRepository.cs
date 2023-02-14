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
    public class ProductCategoryRepository:EntityRepository<ProductCategory>, IProductCategoryRepository
    {
        private readonly MedlabDbContext _context;

        public ProductCategoryRepository(MedlabDbContext context):base(context)
        {
            this._context = context;
        }
    }
}
