using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Shared.Exceptions
{
    public class DbConcurrencyException : Exception
    {
        public string ModificationBy { get; set; }

        public DbConcurrencyException() { }

        public DbConcurrencyException(string modificationBy) : base($"Data was updated by {modificationBy}")
        {
            ModificationBy = modificationBy;
        }
    }
}
