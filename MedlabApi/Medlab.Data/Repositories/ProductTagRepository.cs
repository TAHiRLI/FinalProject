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
    public class ProductTagRepository:EntityRepository<ProductTag>, IProductTagRepository
    {
        private readonly MedlabDbContext _context;

        public ProductTagRepository(MedlabDbContext context):base(context)
        {
            this._context = context;
        }
    }
}
