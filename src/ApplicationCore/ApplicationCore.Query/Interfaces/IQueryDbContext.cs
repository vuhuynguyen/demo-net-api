using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Query.Interfaces
{
    public interface IQueryDbContext
    {
        DbSet<T> Set<T>() where T : class;

        /// <summary>
        /// If deleted data should be included in the query result
        /// </summary>
        bool IncludeDeleted { get; set; }
    }
}
