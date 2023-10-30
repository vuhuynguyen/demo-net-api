using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Common.Providers
{
    public interface IConnectionStringProvider
    {
        string Get(string name = "DefaultConnection");
    }
}
