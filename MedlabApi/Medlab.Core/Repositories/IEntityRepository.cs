using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Repositories
{
    public interface EntityRepository<TEntity>
    {
        Task AddAsync(TEntity eentity);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> exp, params string[] Includings);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp, params string[] Includings);
        bool Any(Expression<Func<TEntity, bool>> exp, params string[] Includings);
        void Delete(TEntity entity);
        Task<int> CommitAsync();
        int Commit();
    }
}
