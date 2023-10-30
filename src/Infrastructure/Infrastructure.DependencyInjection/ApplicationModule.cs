using Autofac;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DependencyInjection
{
    public class ApplicationModule : Module
    {
        private readonly IConfiguration _configuration;

        public ApplicationModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder container)
        {
            ApplicationCore.Common.DependencyInjection.Register(container, _configuration);
            ApplicationCore.Command.DependencyInjection.Register(container);
            ApplicationCore.Query.DependencyInjection.Register(container);
            Persistence.DependencyInjection.Register(container);
            Query.DependencyInjection.Register(container);
        }
    }
}
