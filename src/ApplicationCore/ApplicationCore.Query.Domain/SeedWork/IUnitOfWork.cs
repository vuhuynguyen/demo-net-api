using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Manually marking an entity as Modified state. 
        /// Useful when updating child entity without changing parent and we want to mark parent as Modified for concurrency check or audit.
        /// </summary>
        void MarkAsChanged<T>(T entity) where T : Entity;

        Task SaveChangesAsync(CancellationToken cancellationToken = default, ConcurrencyResolutionStrategy strategy = ConcurrencyResolutionStrategy.None);
    }
}
