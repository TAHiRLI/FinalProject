using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Data.Repositories
{
    public class ProductRepository : EntityRepository<Product>, IProductRepository
    {
        private readonly MedlabDbContext _context;

        public ProductRepository(MedlabDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<Product?> GetProductForDetails(int id)
        {
            return await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductCategory)
                .Include(x => x.ProductReviews)
                .ThenInclude(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public IEnumerable<Object> GetTopSoldProducts()
        {
            var result = _context.OrderItems
                 .Include(oi => oi.Product)
                     .AsEnumerable()
                 .GroupBy(oi => oi.Product)
                 .Select(g => new TopSoldProduct
                 {
                     Product = g.Key,
                     CountOfOrders = g.Sum(oi=> oi.Count),
                     TotalIncome = g.Sum(x => (x.SalePrice - x.CostPrice) * (1 - x.DiscountPercent / 100)*x.Count)
                 })
                 .OrderByDescending(x=> x.CountOfOrders);

            return result;
        }

    }
    public class TopSoldProduct
    {
        public Product? Product { get; set; }
        public int CountOfOrders { get; set; }
        public decimal TotalIncome { get; set; }
    }
}
