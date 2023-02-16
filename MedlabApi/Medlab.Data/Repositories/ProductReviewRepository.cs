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
    public class ProductReviewRepository:EntityRepository<ProductReview> , IProductReviewRepository
    {
        private readonly MedlabDbContext _context;

        public ProductReviewRepository(MedlabDbContext context):base(context)
        {
            _context = context;
        }
    }
}
