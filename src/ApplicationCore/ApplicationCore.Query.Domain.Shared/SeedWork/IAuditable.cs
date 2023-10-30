using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.SeedWork
{
    public interface IAuditable : ICreationAuditable
    {
        DateTime? ModificationDate { get; set; }
        string ModificationBy { get; set; }
    }

    public interface ICreationAuditable
    {
        DateTime CreationDate { get; set; }

        string CreationBy { get; set; }
    }
}
