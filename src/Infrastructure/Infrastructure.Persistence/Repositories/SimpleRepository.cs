using ApplicationCore.Domain.SeedWork;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class SimpleRepository<T> : SimpleRepository<T, int>, ISimpleRepository<T> where T : SimpleEntity
    {
        public SimpleRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }
    }

    public class SimpleRepository<T, TKey> : ISimpleRepository<T, TKey> where T : SimpleEntity<TKey>
    {
        protected readonly DatabaseContext DbContext;

        public IUnitOfWork UnitOfWork => DbContext;

        public SimpleRepository(DatabaseContext dbContext)
        {
            DbContext = dbContext;
        }

        public void Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
        }

        public async Task<T> GetByIdAsync(TKey id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public void Remove(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public Task<T> SingleOrDefaultAsync(ISpecification<T> specification = null)
        {
            var query = ApplySpecification(specification);
            return query.SingleOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator.Default.GetQuery(DbContext.Set<T>().AsQueryable(), spec);
        }
    }
}
