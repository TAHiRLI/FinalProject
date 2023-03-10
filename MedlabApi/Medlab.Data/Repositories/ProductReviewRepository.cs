using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Data.Repositories
{
    public class ProductReviewRepository : EntityRepository<ProductReview>, IProductReviewRepository
    {
        private readonly MedlabDbContext _context;

        public ProductReviewRepository(MedlabDbContext context) : base(context)
        {
            _context = context;
        }
        public object GetAllReviewsByMonth()
        {
            var reviewCountsByMonth = _context.ProductReviews
                .Where(x=> x.CreatedAt.Year == DateTime.Now.Year)
                .AsEnumerable()
                .OrderBy(x=> x.CreatedAt)
                .GroupBy(r => r.CreatedAt.ToString("MMMM"))
                .ToDictionary(g => g.Key, g => g.Count());

            return reviewCountsByMonth;
        }
        public object GetApprovedReviewsByMonth()
        {
            var reviewCountsByMonth = _context.ProductReviews
                             .Where(x => x.CreatedAt.Year == DateTime.Now.Year)
                             .Where(x => x.IsApproved == true)
                             .AsEnumerable()
                             .OrderBy(x => x.CreatedAt)
                             .GroupBy(r => r.CreatedAt.ToString("MMMM"))
                             .Select(g => new { Month = g.Key, Count = g.Count() })
                             .Union(
                                 Enumerable.Range(1, 12)
                                     .Where(month => !_context.ProductReviews.Any(x => x.CreatedAt.Month == month && x.CreatedAt.Year == DateTime.Now.Year && x.IsApproved == false))
                                     .Select(month => new { Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), Count = 0 })
                             )
                             .OrderBy(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.CurrentCulture))
                             .ToDictionary(g => g.Month, g => g.Count);

            return reviewCountsByMonth;
        }
        public object GetRejectedReviewsByMonth()
        {
            var reviewCountsByMonth = _context.ProductReviews
                            .Where(x => x.CreatedAt.Year == DateTime.Now.Year)
                            .Where(x => x.IsApproved == false)
                            .AsEnumerable()
                            .OrderBy(x => x.CreatedAt)
                            .GroupBy(r => r.CreatedAt.ToString("MMMM"))
                            .Select(g => new { Month = g.Key, Count = g.Count() })
                            .Union(
                                Enumerable.Range(1, 12)
                                    .Where(month => !_context.ProductReviews.Any(x => x.CreatedAt.Month == month && x.CreatedAt.Year == DateTime.Now.Year && x.IsApproved == false))
                                    .Select(month => new { Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), Count = 0 })
                            )
                            .OrderBy(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.CurrentCulture))
                            .ToDictionary(g => g.Month, g => g.Count);


            return reviewCountsByMonth;
        }
    }
}
