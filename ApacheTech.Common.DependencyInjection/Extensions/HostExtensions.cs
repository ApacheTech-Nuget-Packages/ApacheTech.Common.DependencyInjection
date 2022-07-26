using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.DependencyInjection.Annotation;
using ApacheTech.Common.Extensions.Reflection;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.Common.DependencyInjection.Extensions
{
    /// <summary>
    ///     Provides extension methods for hosting services.
    /// </summary>
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
        public static void AddAnnotatedServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly
                .GetTypesWithAttribute<RegisteredServiceAttribute>()
                .ToList();

            foreach (var (type, attribute) in types)
            {
                var descriptor = new ServiceDescriptor(attribute.ServiceType, type, attribute.ServiceScope);
                services.Add(descriptor);
            }
        }

        /// <summary>
        ///     Performs custom configuration for the given <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to use.</param>
        /// <param name="serviceRegistrationFactory">A factory method, allowing custom service registration.</param>
        /// <returns>Returns the same instance of <see cref="IServiceCollection" /> that it was passed.</returns>
        public static IServiceCollection Configure(this IServiceCollection services, Action<IServiceCollection> serviceRegistrationFactory)
        {
            serviceRegistrationFactory(services);
            return services;
        }

        /// <summary>
        ///     Attempts to add an element, with the provided key and value, to the <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c>, if the element was successfully added to the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">dictionary</exception>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
            if (dictionary.ContainsKey(key)) return false;
            dictionary.Add(key, value);
            return true;

        }
    }
}