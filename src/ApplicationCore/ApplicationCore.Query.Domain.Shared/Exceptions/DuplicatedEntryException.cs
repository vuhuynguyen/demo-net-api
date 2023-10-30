using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Shared.Exceptions
{
    public class DuplicatedEntryException : BusinessException
    {
        public DuplicatedEntryException(string entity) : base($"{entity} has already existed")
        {
        }
    }
}
