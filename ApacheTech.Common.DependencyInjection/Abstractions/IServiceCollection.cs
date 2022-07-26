using System;
using System.Collections.Generic;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.Common.DependencyInjection.Abstractions
{
    /// <summary>
    ///     An IOC Container, which holds references to Added types of services, and their instances.
    /// </summary>
    public partial interface IServiceCollection
    {
        /// <summary>
        ///     Adds a raw service descriptor, pre-populated with meta-data for the service.
        /// </summary>
        /// <param name="descriptor">The pre-populated descriptor for the service to add.</param>
        /// <seealso cref="ServiceDescriptor"/>
        void Add(ServiceDescriptor descriptor);

        /// <summary>
        ///     Adds raw service descriptors, pre-populated with meta-data for the service.
        /// </summary>
        /// <param name="descriptors">The pre-populated descriptors for the service to add.</param>
        /// <seealso cref="ServiceDescriptor"/>
        void Add(IEnumerable<ServiceDescriptor> descriptors);

        /// <summary>
        ///     Adds a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        void AddSingleton(Type implementationType);

        /// <summary>
        ///     Adds a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <param name="serviceType">The type of service to add.</param>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        void AddSingleton(Type serviceType, Type implementationType);

        /// <summary>
        ///     Adds a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to add.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        /// 
        void AddSingleton<TService>() where TService : class;

        /// <summary>
        ///     Adds a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of implementation to use.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        void AddSingleton<TService, TImplementation>() where TImplementation : TService;

        /// <summary>
        ///     Adds a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of implementation to use.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        void AddSingleton<TService, TImplementation>(Func<IServiceResolver, TService> implementationFactory) 
            where TImplementation : TService 
            where TService : class;

        /// <summary>
        ///     Adds a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        void AddSingleton<TService>(Func<IServiceResolver, TService> implementationFactory) where TService : class;

        /// <summary>
        ///     Adds a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The instance to add.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        void AddSingleton<TService>(TService implementation);

        /// <summary>
        ///     Adds a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Transient"/>
        void AddTransient(Type implementationType);

        /// <summary>
        ///     Adds a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <param name="serviceType">The type of service to add.</param>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Transient"/>
        void AddTransient(Type serviceType, Type implementationType);

        /// <summary>
        ///     Adds a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to add.</typeparam>
        /// <seealso cref="ServiceLifetime.Transient"/>
        void AddTransient<TService>() where TService : class;

        /// <summary>
        ///     Adds a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of implementation to use.</typeparam>
        /// <seealso cref="ServiceLifetime.Transient"/>
        void AddTransient<TService, TImplementation>() where TImplementation : TService;

        /// <summary>
        ///     Adds a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <seealso cref="ServiceLifetime.Transient"/>
        void AddTransient<TService>(Func<IServiceResolver, TService> implementationFactory) where TService : class;

        /// <summary>
        ///     Build a service resolver, to access services within this collection.
        /// </summary>
        /// <returns>An IOC Service Resolver.</returns>
        IServiceResolver Build();
    }
}