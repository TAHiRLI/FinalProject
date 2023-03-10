using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Data.Repositories
{
    public class ProductCategoryRepository : EntityRepository<ProductCategory>, IProductCategoryRepository
    {
        private readonly MedlabDbContext _context;

        public ProductCategoryRepository(MedlabDbContext context) : base(context)
        {
            this._context = context;
        }
        public IQueryable<Object> GetTopSoldCategory()
        {
            var result = _context.Orders
                .Where(x=> x.OrderStatus == true)
              .Join(_context.OrderItems, order => order.Id, orderItem => orderItem.OrderId, (order, orderItem) => new { order, orderItem })
              .Join(_context.Products, oi => oi.orderItem.ProductId, product => product.Id, (oi, product) => new { oi.order, oi.orderItem, product })
              .Join(_context.ProductCategories, p => p.product.ProductCategoryId, category => category.Id, (p, category) => new { category.Name, p.order.Id, p.orderItem.SalePrice, p.orderItem.CostPrice, p.orderItem.DiscountPercent, p.orderItem.Count })
              .GroupBy(c => c.Name)
              .OrderByDescending(g => g.Count())
              .Select(g => new TopSoldCategory
              {
                  CategoryName = g.Key,
                  OrderCount = g.Sum(x=> x.Count),
                  TotalRevenue = g.Sum(x => (x.SalePrice - x.CostPrice) * (1 - x.DiscountPercent / 100) * x.Count)
              });
            return result;
        }


    }
    public class TopSoldCategory
    {
        public string CategoryName { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
