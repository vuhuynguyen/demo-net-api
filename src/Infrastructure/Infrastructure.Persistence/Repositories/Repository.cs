using ApplicationCore.Domain.SeedWork;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : Entity, IAggregateRoot
    {
        protected readonly DatabaseContext DbContext;

        public IUnitOfWork UnitOfWork => DbContext;

        public Repository(DatabaseContext dbContext)
        {
            DbContext = dbContext;
        }

        public void Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
        }

        public void AddRange(params T[] entities)
        {
            DbContext.Set<T>().AddRange(entities);
        }

        public void MarkAsChanged(T entity)
        {
            DbContext.Entry<T>(entity).State = EntityState.Modified;
        }

        public Task<int> CountAsync(ISpecification<T> specification = null)
        {
            var query = ApplySpecification(specification);
            return query.CountAsync();
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return DbContext.Set<T>().FindAsync(id).AsTask();
        }

        public async Task<IEnumerable<T>> ListAsync(ISpecification<T> specification = null)
        {
            var query = ApplySpecification(specification);
            return await query.ToListAsync();
        }

        public Task<T> SingleOrDefaultAsync(ISpecification<T> specification = null)
        {
            var query = ApplySpecification(specification);
            return query.SingleOrDefaultAsync();
        }

        public Task<T> SingleAsync(ISpecification<T> specification = null)
        {
            var query = ApplySpecification(specification);
            return query.SingleAsync();
        }

        public Task<T> FirstOrDefaultAsync(ISpecification<T> specification = null)
        {
            var query = ApplySpecification(specification);
            return query.FirstOrDefaultAsync();
        }

        public Task<T> FirstAsync(ISpecification<T> specification = null)
        {
            var query = ApplySpecification(specification);
            return query.FirstAsync();
        }

        public void Remove(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator.Default.GetQuery(DbContext.Set<T>().AsQueryable(), spec);
        }
    }
}
