using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Medlab.Data.Repositories
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class
    {
        private readonly MedlabDbContext _context;

        public EntityRepository(MedlabDbContext context)
        {
            this._context = context;
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public bool Any(Expression<Func<TEntity, bool>> exp, params string[] Includings)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if(Includings != null)
            {
                foreach (var prop in Includings)
                {
                    query = query.Include(prop);
                }
            }

            return query.Any(exp);
        }
        public int Commit()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> exp, params string[] Includings)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if(Includings != null)
            {
                foreach (var prop in Includings)
                {
                    query = query.Include(prop);
                }
            }

            return query.Where(exp);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp, params string[] Includings)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (Includings != null)
            {
                foreach (var prop in Includings)
                {
                    query = query.Include(prop);
                }
            }

            return await query.FirstOrDefaultAsync(exp);
        }
    }
}
