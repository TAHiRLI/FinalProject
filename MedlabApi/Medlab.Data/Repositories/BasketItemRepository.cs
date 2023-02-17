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
    public class BasketItemRepository:EntityRepository<BasketItem>, IBasketItemRepository
    {
        private readonly MedlabDbContext _context;

        public BasketItemRepository( MedlabDbContext context):base(context)
        {
            _context = context;
        }

        public List<BasketItem> GetBasketItemsWithProduct()
        {
            return _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).ToList() ;
        }
    }
}
