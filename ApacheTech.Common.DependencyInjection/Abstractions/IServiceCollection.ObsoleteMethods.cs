using System;

namespace ApacheTech.Common.DependencyInjection.Abstractions
{
    public partial interface IServiceCollection
    {
        /// <summary>
        ///     Registers a raw service descriptor, pre-populated with meta-data for the service.
        /// </summary>
        /// <param name="descriptor">The pre-populated descriptor for the service to register.</param>
        /// <seealso cref="ServiceDescriptor"/>
        [Obsolete("Use Add() instead. Method will be removed during in the major release cycle.")]
        void Register(ServiceDescriptor descriptor);

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        [Obsolete("Use AddSingleton() instead. Method will be removed during in the major release cycle.")]
        void RegisterSingleton(Type implementationType);

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <param name="serviceType">The type of service to register.</param>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        [Obsolete("Use AddSingleton() instead. Method will be removed during in the major release cycle.")]
        void RegisterSingleton(Type serviceType, Type implementationType);

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        [Obsolete("Use AddSingleton() instead. Method will be removed during in the major release cycle.")]
        void RegisterSingleton<TService>() where TService : class;

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <typeparam name="TImplementation">The type of implementation to use.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        [Obsolete("Use AddSingleton() instead. Method will be removed during in the major release cycle.")]
        void RegisterSingleton<TService, TImplementation>() where TImplementation : TService;

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        [Obsolete("Use AddSingleton() instead. Method will be removed during in the major release cycle.")]
        void RegisterSingleton<TService>(Func<IServiceResolver, TService> implementationFactory) where TService : class;

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The instance to register.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        [Obsolete("Use AddSingleton() instead. Method will be removed during in the major release cycle.")]
        void RegisterSingleton<TService>(TService implementation);

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Transient"/>
        [Obsolete("Use AddTransient() instead. Method will be removed during in the major release cycle.")]
        void RegisterTransient(Type implementationType);

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <param name="serviceType">The type of service to register.</param>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Transient"/>
        [Obsolete("Use AddTransient() instead. Method will be removed during in the major release cycle.")]
        void RegisterTransient(Type serviceType, Type implementationType);

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <seealso cref="ServiceLifetime.Transient"/>
        [Obsolete("Use AddTransient() instead. Method will be removed during in the major release cycle.")]
        void RegisterTransient<TService>() where TService : class;

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <typeparam name="TImplementation">The type of implementation to use.</typeparam>
        /// <seealso cref="ServiceLifetime.Transient"/>
        [Obsolete("Use AddTransient() instead. Method will be removed during in the major release cycle.")]
        void RegisterTransient<TService, TImplementation>() where TImplementation : TService;

        /// <summary>
        /// Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <seealso cref="ServiceLifetime.Transient" />
        [Obsolete("Use AddTransient() instead. Method will be removed during in the major release cycle.")]
        void RegisterTransient<TService>(Func<IServiceResolver, TService> implementationFactory) where TService : class;
    }
}