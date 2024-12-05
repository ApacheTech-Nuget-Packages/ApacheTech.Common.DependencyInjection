#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using ApacheTech.Common.DependencyInjection.Abstractions;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.Common.DependencyInjection
{
    /// <summary>
    /// The default IServiceProvider.
    /// </summary>
    public sealed class ServiceProvider : IServiceProvider, IDisposable
    {
        private readonly IEnumerable<ServiceDescriptor> _serviceDescriptors;
        private readonly ServiceProviderOptions _options;

        /// <summary>
        ///     Initialises a new instance of the <see cref="ServiceProvider"/> class.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
        /// <param name="options"> Configures various service provider behaviours.</param>
        public ServiceProvider(IEnumerable<ServiceDescriptor> services, ServiceProviderOptions options)
        {
            _serviceDescriptors = services;
            _options = options;
        }

        /// <summary>
        ///     Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        ///     A service object of type <paramref name="serviceType">serviceType</paramref>.   -or-  null if there is no service object of type <paramref name="serviceType">serviceType</paramref>.
        /// </returns>
        public object? GetService(Type serviceType)
        {
            // Check for explicitly registered IEnumerable<T>
            var descriptor = _serviceDescriptors.SingleOrDefault(p => p.ServiceType == serviceType);
            if (descriptor is not null)
            {
                return ResolveDescriptor(descriptor);
            }

            // Handle IEnumerable<T> when no explicit registration exists
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var itemType = serviceType.GetGenericArguments()[0];
                return ResolveEnumerable(itemType);
            }

            return null;
        }


        /// <summary>
        ///     Disposes all services that require disposing.
        /// </summary>
        public void Dispose()
        {
            if (!_options.DisposeImplementations) return;
            
            var disposableImplementations = _serviceDescriptors
                .Where(p => p.Implementation is IDisposable)
                .Select(x => x.Implementation as IDisposable);

            foreach (var implementation in disposableImplementations)
            {
                implementation?.Dispose();
            }
        }

        private object? ResolveDescriptor(ServiceDescriptor descriptor)
        {
            if (descriptor.Implementation is not null)
            {
                return descriptor.Implementation;
            }

            if (descriptor.ImplementationFactory is not null)
            {
                return CacheService(descriptor, descriptor.ImplementationFactory(this));
            }

            var implementationType = descriptor.ImplementationType ?? descriptor.ServiceType;

            if (implementationType.IsInterface)
            {
                throw new TypeLoadException($"Cannot instantiate interface: {implementationType.FullName}");
            }

            if (implementationType.IsAbstract)
            {
                throw new TypeLoadException($"Cannot instantiate abstract class: {implementationType.FullName}");
            }

            var implementation = ActivatorUtilities.CreateInstance(this, implementationType);
            return CacheService(descriptor, implementation);
        }

        private object ResolveEnumerable(Type itemType)
        {
            var implementations = _serviceDescriptors
                .Where(d => d.ServiceType == itemType)
                .Select(ResolveDescriptor)
                .Where(instance => instance != null)
                .ToList();

            var array = Array.CreateInstance(itemType, implementations.Count);
            for (int i = 0; i < implementations.Count; i++)
            {
                array.SetValue(implementations[i], i);
            }

            return array;
        }

        private static object CacheService(ServiceDescriptor descriptor, object implementation)
        {
            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                descriptor.Implementation = implementation;
            }
            return implementation;
        }
        
    }
}