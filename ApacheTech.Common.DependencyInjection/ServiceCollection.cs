using System;
using System.Collections.Generic;
using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.Extensions.System;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.Common.DependencyInjection
{
    /// <summary>
    ///     An IOC Container, which holds references to registered types of services, and their instances.
    /// </summary>
    /// <seealso cref="IServiceCollection" />
    public partial class ServiceCollection : IServiceCollection, IDisposable
    {
        private readonly List<ServiceDescriptor> _serviceDescriptors = new();
        private readonly Dictionary<Type, Func<IServiceResolver, object>> _factories = new();
        private readonly IServiceResolver _resolver;

        /// <summary>
        /// 	Initialises a new instance of the <see cref="ServiceCollection" /> class.
        /// </summary>
        public ServiceCollection()
        {
            _resolver = new ServiceResolver(_serviceDescriptors, _factories);
        }

        /// <summary>
        ///     Registers a raw service descriptor, pre-populated with meta-data for the service.
        /// </summary>
        /// <param name="descriptor">The pre-populated descriptor for the service to register.</param>
        /// <seealso cref="ServiceDescriptor" />
        public void Add(ServiceDescriptor descriptor)
        {
            _serviceDescriptors.AddIfNotPresent(descriptor);
        }

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Singleton" />
        public void AddSingleton(Type implementationType)
        {
            _serviceDescriptors.AddIfNotPresent(new ServiceDescriptor(implementationType, ServiceLifetime.Singleton));
        }

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <param name="serviceType">The type of service to register.</param>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Singleton" />
        public void AddSingleton(Type serviceType, Type implementationType)
        {
            _serviceDescriptors.AddIfNotPresent(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
        }

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        public void AddSingleton<TService>() where TService : class
        {
            _serviceDescriptors.AddIfNotPresent(new ServiceDescriptor(typeof(TService), ServiceLifetime.Singleton));
        }

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <typeparam name="TImplementation">The type of implementation to use.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        public void AddSingleton<TService, TImplementation>() where TImplementation : TService
        {
            _serviceDescriptors.AddIfNotPresent(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
        }

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        public void AddSingleton<TService>(Func<IServiceResolver, TService> implementationFactory) where TService : class
        {
            _factories.Add(typeof(TService), implementationFactory);
            _serviceDescriptors.AddIfNotPresent(new ServiceDescriptor(typeof(TService), ServiceLifetime.Singleton));
        }

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The instance to register.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        public void AddSingleton<TService>(TService instance)
        {
            _serviceDescriptors.AddIfNotPresent(new ServiceDescriptor(typeof(TService), instance, ServiceLifetime.Singleton));
        }

        /// <summary>
        /// Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Transient" />
        public void AddTransient(Type implementationType)
        {
            _serviceDescriptors.AddIfNotPresent(new ServiceDescriptor(implementationType, ServiceLifetime.Transient));
        }

        /// <summary>
        /// Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <param name="serviceType">The type of service to register.</param>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Transient" />
        public void AddTransient(Type serviceType, Type implementationType)
        {
            _serviceDescriptors.AddIfNotPresent(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));
        }

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <seealso cref="ServiceLifetime.Transient"/>
        public void AddTransient<TService>() where TService : class
        {
            _serviceDescriptors.AddIfNotPresent(new ServiceDescriptor(typeof(TService), ServiceLifetime.Transient));
        }

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <typeparam name="TImplementation">The type of implementation to use.</typeparam>
        /// <seealso cref="ServiceLifetime.Transient"/>
        public void AddTransient<TService, TImplementation>() where TImplementation : TService
        {
            _serviceDescriptors.AddIfNotPresent(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
        }

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <seealso cref="ServiceLifetime.Transient"/>
        public void AddTransient<TService>(Func<IServiceResolver, TService> implementationFactory) where TService : class
        {
            _factories.Add(typeof(TService), implementationFactory);
            _serviceDescriptors.AddIfNotPresent(new ServiceDescriptor(typeof(TService), ServiceLifetime.Transient));
        }

        /// <summary>
        ///     Build a service resolver, to access services within this collection.
        /// </summary>
        /// <returns>An IOC Service Resolver.</returns>
        public IServiceResolver Build()
        {
            return _resolver;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _resolver?.Dispose();
        }
    }
}