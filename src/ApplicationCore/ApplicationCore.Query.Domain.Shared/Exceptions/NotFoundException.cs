using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Shared.Exceptions
{
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string entity) : base($"{entity} was not found")
        {
        }
    }
}
