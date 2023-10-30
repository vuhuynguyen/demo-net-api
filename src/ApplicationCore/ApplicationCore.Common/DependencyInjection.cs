using ApplicationCore.Common.Behaviors;
using ApplicationCore.Common.Providers;
using ApplicationCore.Common.Shared;
using Autofac;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Common
{
    public static class DependencyInjection
    {
        public static void Register(ContainerBuilder container, IConfiguration configuration)
        {
            // Register Mediator itself
            container.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

            // Register Mediator request and notification handlers
            container.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            // Register Mediator behaviors 
            container.RegisterGeneric(typeof(ValidationBehavior<,>)).AsImplementedInterfaces().InstancePerDependency();

            // Application Context
            container.RegisterType<ApplicationContext>().AsSelf().InstancePerLifetimeScope();

            // CommandHandlers
            container.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .InstancePerDependency();

            container.RegisterType<AppSettingConnectionStringProvider>().As<IConnectionStringProvider>().SingleInstance();
        }
    }
}
