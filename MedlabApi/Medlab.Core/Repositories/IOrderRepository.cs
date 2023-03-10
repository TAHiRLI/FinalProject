using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Repositories
{
    public interface IOrderRepository : IEntityRepository<Order>
    {
        List<Order> GetOrdersWithProducts(string userId);
        Order? GetOrderById(int id);
        object GetSalesByMonth();
        object GetSalesSummary();


    }
}
