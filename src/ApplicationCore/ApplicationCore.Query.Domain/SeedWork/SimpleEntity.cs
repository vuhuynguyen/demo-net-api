using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.SeedWork
{
    public abstract class SimpleEntity : SimpleEntity<int>
    {
    }

    public abstract class SimpleEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
