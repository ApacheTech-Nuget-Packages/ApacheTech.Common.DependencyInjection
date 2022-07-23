using System;
using ApacheTech.Common.DependencyInjection.Abstractions;

// ReSharper disable UnusedType.Global

namespace ApacheTech.Common.DependencyInjection
{
    /// <summary>
    ///     An IOC Container, which holds references to registered types of services, and their instances.
    /// </summary>
    /// <seealso cref="IServiceCollection" />
    /// ROADMAP: Remove class in v2.0.0.
    public partial class ServiceCollection
    {
        /// <summary>
        ///     Registers a raw service descriptor, pre-populated with meta-data for the service.
        /// </summary>
        /// <param name="descriptor">The pre-populated descriptor for the service to register.</param>
        /// <seealso cref="ServiceDescriptor" />
        [Obsolete("Use Add() instead. Method will be removed during in the major release cycle.")]
        public void Register(ServiceDescriptor descriptor)
        {
            Add(descriptor);
        }

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Singleton" />
        [Obsolete("Use AddSingleton() instead. Method will be removed during in the major release cycle.")]
        public void RegisterSingleton(Type implementationType)
        {
            AddSingleton(implementationType);
        }

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <param name="serviceType">The type of service to register.</param>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Singleton" />
        [Obsolete("Use AddSingleton() instead. Method will be removed during in the major release cycle.")]
        public void RegisterSingleton(Type serviceType, Type implementationType)
        {
            AddSingleton(serviceType, implementationType);
        }

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        [Obsolete("Use AddSingleton() instead. Method will be removed during in the major release cycle.")]
        public void RegisterSingleton<TService>() where TService : class
        {
            AddSingleton<TService>();
        }

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <typeparam name="TImplementation">The type of implementation to use.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        [Obsolete("Use AddSingleton() instead. Method will be removed during in the major release cycle.")]
        public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService
        {
            AddSingleton<TService, TImplementation>();
        }

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        [Obsolete("Use AddSingleton() instead. Method will be removed during in the major release cycle.")]
        public void RegisterSingleton<TService>(Func<IServiceResolver, TService> implementationFactory) where TService : class
        {
            AddSingleton(implementationFactory);
        }

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The instance to register.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        [Obsolete("Use AddSingleton() instead. Method will be removed during in the major release cycle.")]
        public void RegisterSingleton<TService>(TService instance)
        {
            AddSingleton(instance);
        }

        /// <summary>
        /// Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Transient" />
        [Obsolete("Use AddTransient() instead. Method will be removed during in the major release cycle.")]
        public void RegisterTransient(Type implementationType)
        {
            AddTransient(implementationType);
        }

        /// <summary>
        /// Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <param name="serviceType">The type of service to register.</param>
        /// <param name="implementationType">The type of implementation to use.</param>
        /// <seealso cref="ServiceLifetime.Transient" />
        [Obsolete("Use AddTransient() instead. Method will be removed during in the major release cycle.")]
        public void RegisterTransient(Type serviceType, Type implementationType)
        {
            AddTransient(serviceType, implementationType);
        }

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <seealso cref="ServiceLifetime.Transient"/>
        [Obsolete("Use AddTransient() instead. Method will be removed during in the major release cycle.")]
        public void RegisterTransient<TService>() where TService : class
        {
            AddTransient<TService>();
        }

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <typeparam name="TImplementation">The type of implementation to use.</typeparam>
        /// <seealso cref="ServiceLifetime.Transient"/>
        [Obsolete("Use AddTransient() instead. Method will be removed during in the major release cycle.")]
        public void RegisterTransient<TService, TImplementation>() where TImplementation : TService
        {
            AddTransient<TService, TImplementation>();
        }

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <seealso cref="ServiceLifetime.Transient"/>
        [Obsolete("Use AddTransient() instead. Method will be removed during in the major release cycle.")]
        public void RegisterTransient<TService>(Func<IServiceResolver, TService> implementationFactory) where TService : class
        {
            AddTransient(implementationFactory);
        }
    }
}