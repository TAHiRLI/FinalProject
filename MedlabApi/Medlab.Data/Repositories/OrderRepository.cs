using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                .Include(x=> x.AppUser)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .FirstOrDefault(x => x.Id == id);
        }

    }
}
