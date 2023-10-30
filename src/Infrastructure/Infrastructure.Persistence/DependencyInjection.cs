using ApplicationCore.Domain.SeedWork;
using Autofac;
using Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static void Register(ContainerBuilder container)
        {
            // DbContext
            container.RegisterType<DatabaseContext>().InstancePerLifetimeScope();

            // Repositories
            container.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            container.RegisterGeneric(typeof(SimpleRepository<>))
                .As(typeof(ISimpleRepository<>))
                .InstancePerLifetimeScope();
        }
    }
}
