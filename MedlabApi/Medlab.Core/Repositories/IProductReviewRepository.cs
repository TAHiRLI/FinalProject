using Medlab.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Repositories
{
    public interface IProductReviewRepository:IEntityRepository<ProductReview>
    {
        object GetAllReviewsByMonth();
        object GetApprovedReviewsByMonth();
        object GetRejectedReviewsByMonth();

    }
}
