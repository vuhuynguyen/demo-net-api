using ApplicationCore.Domain.SeedWork;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.SeedWork
{
    public interface ISimpleRepository<T> : ISimpleRepository<T, int> where T : SimpleEntity
    {
    }

    public interface ISimpleRepository<T, TKey> where T : SimpleEntity<TKey>
    {
        IUnitOfWork UnitOfWork { get; }

        void Add(T entity);

        void Remove(T entity);

        Task<T> GetByIdAsync(TKey id);

        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate);
        Task<T> SingleOrDefaultAsync(ISpecification<T> specification = null);

    }
}
