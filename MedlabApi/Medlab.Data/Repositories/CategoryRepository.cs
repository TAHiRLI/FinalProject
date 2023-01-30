using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;

namespace Medlab.Data.Repositories
{
    public class CategoryRepository:EntityRepository<Category>, ICategoryRepository
    {
        private readonly MedlabDbContext _context;

        public CategoryRepository(MedlabDbContext context) : base(context)
        {
            this._context = context;
        }

    }
}
