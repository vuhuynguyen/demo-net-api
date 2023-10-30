using ApplicationCore.Common.Providers;
using EnsureThat;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Common.Shared
{
    public class AppSettingConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;

        public AppSettingConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Get(string name = "DefaultConnection")
        {
            var connString = _configuration.GetConnectionString(name);
            Ensure.That(connString, nameof(connString), opts => opts.WithMessage("Cannot find connection string")).IsNotEmptyOrWhiteSpace();

            return connString;
        }
    }
}
