using ApplicationCore.Query.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public static class DependencyInjection
    {
        public static void Register(ContainerBuilder container)
        {
            // DbContext
            container.RegisterType<QueryDbContext>().As<IQueryDbContext>().InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
