using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.SeedWork
{
    public interface IConcurrencyCheck
    {
        byte[] RowVersion { get; set; }
    }

    public static class ConcurrencyCheckExtensions
    {
        /// <summary>
        /// Set row version for concurrency check.
        /// In case the child entity is changed without changing parent. Call MarkAsChanged method of Repository/UnitOfWork for EF to recognize the change.
        /// </summary>
        public static void SetRowVersion(this IConcurrencyCheck entity, string rowVersion)
        {
            if (!string.IsNullOrEmpty(rowVersion))
            {
                entity.RowVersion = Convert.FromBase64String(rowVersion);
            }
        }
    }
}
