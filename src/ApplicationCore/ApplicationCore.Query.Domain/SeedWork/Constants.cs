using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.SeedWork
{
    public enum ConcurrencyResolutionStrategy
    {
        /// <summary>
        /// Throws exception
        /// </summary>
        None,

        /// <summary>
        /// Uses database values
        /// </summary>
        DatabaseWin,

        /// <summary>
        /// Uses client values
        /// </summary>
        ClientWin
    }
}
