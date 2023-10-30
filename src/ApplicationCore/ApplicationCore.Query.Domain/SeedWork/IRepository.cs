using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.SeedWork
{
    public interface IRepository<T> where T : Entity, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        void Add(T entity);
        void AddRange(params T[] entities);

        /// <summary>
        /// Manually marking an entity as Modified state. 
        /// Useful when updating child entity without changing parent and we want to mark parent as Modified for concurrency check or audit.
        /// </summary>
        void MarkAsChanged(T entity);

        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> ListAsync(ISpecification<T> specification = null);
        Task<int> CountAsync(ISpecification<T> specification = null);
        Task<T> SingleOrDefaultAsync(ISpecification<T> specification = null);
        Task<T> SingleAsync(ISpecification<T> specification = null);
        Task<T> FirstOrDefaultAsync(ISpecification<T> specification = null);
        Task<T> FirstAsync(ISpecification<T> specification = null);
        void Remove(T entity);
    }
}
