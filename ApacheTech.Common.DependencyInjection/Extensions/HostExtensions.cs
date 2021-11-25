using System;
using System.Linq;
using System.Reflection;
using ApacheTech.Common.Extensions.Abstractions;
using ApacheTech.Common.Extensions.Annotation;
using ApacheTech.Common.Extensions.Reflection;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.Common.Extensions.Extensions
{
    public static class HostExtensions
    {
        /// <summary>
        ///     Registers classes, annotate with <see cref="RegisteredServiceAttribute"/>, within the given assembly.
        /// </summary>
        /// <remarks>
        ///     If no specific type is supplied, the class will be registered via convention:<br/><br/>
        ///
        ///      • If the class implements an interface, the interface will be be used as the representation.<br/>
        ///      • If the class implements more than one interface, the first interface will be be used as the representation.<br/>
        ///      • If the class does not implement an interface, it will be registered as itself.
        /// </remarks>
        /// <param name="services">The service collection to register the services with.</param>
        /// <param name="assembly">The assembly to scan for annotated service classes.</param>
        public static void RegisterAnnotatedServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly
                .GetTypesWithAttribute<RegisteredServiceAttribute>()
                .ToList();

            foreach (var (type, attribute) in types)
            {
                var descriptor = new ServiceDescriptor(attribute.ServiceType, type, attribute.ServiceScope);
                services.Register(descriptor);
            }
        }

        /// <summary>
        ///     performs custom configuration for the given <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to use.</param>
        /// <param name="serviceRegistrationFactory">A factory method, allowing custom service registration.</param>
        /// <returns>Returns the same instance of <see cref="IServiceCollection" /> that it was passed.</returns>
        public static IServiceCollection Configure(this IServiceCollection services, Action<IServiceCollection> serviceRegistrationFactory)
        {
            serviceRegistrationFactory(services);
            return services;
        }
    }
}