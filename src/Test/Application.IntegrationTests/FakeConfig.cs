using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ApplicationCore.Common.Shared.ApplicationContext;

namespace Application.IntegrationTests
{
    public static class FakePrincipals
    {
        public static readonly UserPrincipal Admin = new("admin", "admin", "admin");
    }
}
