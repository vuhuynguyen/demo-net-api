using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Query
{
    public class DependencyInjection
    {
        public static void Register(ContainerBuilder container)
        {
            // AutoMapper
            container.RegisterAutoMapper(Assembly.GetExecutingAssembly());

            // CommandHandlers
            container.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .InstancePerDependency();
        }
    }
}
