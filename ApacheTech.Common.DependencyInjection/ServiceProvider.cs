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
        /// A service object of type <paramref name="serviceType">serviceType</paramref>.   -or-  null if there is no service object of type <paramref name="serviceType">serviceType</paramref>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object GetService(Type serviceType)
        {
            var descriptor = _serviceDescriptors.SingleOrDefault(p => p.ServiceType == serviceType);
            if (descriptor is null)
            {
                throw new KeyNotFoundException($"No service of type {serviceType.Name} has been registered.");
            }
            if (descriptor.Implementation is not null)
            {
                return descriptor.Implementation;
            }

            var implementationType = descriptor.ImplementationType ?? descriptor.ServiceType;

            object implementation;
            if (descriptor.ImplementationFactory is not null)
            {
                implementation = descriptor.ImplementationFactory(this);
                return CacheService(descriptor, implementation);
            }

            if (implementationType.IsInterface)
            {
                throw new TypeLoadException("Cannot instantiate interfaces.");
            }
            if (implementationType.IsAbstract)
            {
                throw new TypeLoadException("Cannot instantiate abstract classes.");
            }
            implementation = ActivatorUtilities.CreateInstance(this, implementationType);
            return CacheService(descriptor, implementation);
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