using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Data.Repositories
{
    public class OrderRepository : EntityRepository<Order>, IOrderRepository
    {
        private readonly MedlabDbContext _context;

        public OrderRepository(MedlabDbContext context) : base(context)
        {
            _context = context;
        }
        public List<Order> GetOrdersWithProducts(string userId)
        {
            return _context.Orders
                .Where(x => x.AppUserId == userId)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .ToList();
        }
        public Order? GetOrderById(int id)
        {
            return _context.Orders
                .Include(x => x.AppUser)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .FirstOrDefault(x => x.Id == id);
        }

        public object GetSalesByMonth()
        {
            var result = _context.Orders
                .Include(x => x.OrderItems)
                .Where(x => x.CreatedAt.Year == DateTime.Now.Year)
                .Where(x => x.OrderStatus == true)
                .AsEnumerable()
                .OrderBy(x => x.CreatedAt)
                .GroupBy(x => x.CreatedAt.ToString("MMMM"))
                .Select(g => new { Month = g.Key, Total = g.Sum(x => x.OrderItems.Sum(x => (x.SalePrice - x.CostPrice) * (1 - x.DiscountPercent / 100) * x.Count)) })
                .Union(
                    Enumerable.Range(1, 12)
                        .Where(month => !_context.Orders.Any(x => x.CreatedAt.Month == month && x.CreatedAt.Year == DateTime.Now.Year && x.OrderStatus == true))
                        .Select(month => new { Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), Total = 0m })
                )
                .OrderBy(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.CurrentCulture))
                .ToDictionary(g => g.Month, g => g.Total); return result;
        }
        public object GetSalesSummary()
        {
            var orderItems = _context.OrderItems
      .Where(x => x.Order.OrderStatus == true)
      .Where(x => x.Order.CreatedAt.Year == DateTime.Now.Year)
      .ToList();

            var result = new
            {
                TotalSale = orderItems.Sum(x => x.SalePrice * (100 - x.DiscountPercent) / 100 * x.Count),
                TotalRevenue = orderItems.Sum(x => (x.SalePrice * (100 - x.DiscountPercent) / 100 - x.CostPrice) * x.Count),
                AverageOrder = orderItems.Average(x => x.SalePrice * (100 - x.DiscountPercent) / 100 * x.Count).ToString("0.000")
            };
            return result;
        }

    }
}
