using BookHeaven.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookHeaven.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureCustomServices(this IServiceCollection services)
        {
            Type serviceInterFaceType = typeof(IService);
            Assembly servicesAssembly = Assembly.GetAssembly(serviceInterFaceType);
            IEnumerable<Type> servicesInterfaces = servicesAssembly
                .GetTypes()
                .Where(s => s.IsInterface && s.GetInterfaces().Contains(serviceInterFaceType));
            foreach (Type serviceInterface in servicesInterfaces)
            {
                var serviceImplementation = servicesAssembly
                    .GetTypes()
                    .FirstOrDefault(t => t.GetInterfaces().Contains(serviceInterface) && t.IsClass);

                services.AddTransient(serviceInterface, serviceImplementation);
            }
        }
    }
}