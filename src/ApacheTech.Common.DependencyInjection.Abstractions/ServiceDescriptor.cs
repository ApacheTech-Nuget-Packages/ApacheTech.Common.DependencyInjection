using System;
using System.Diagnostics;
using ApacheTech.Common.DependencyInjection.Abstractions.Extensions;

namespace ApacheTech.Common.DependencyInjection.Abstractions;

/// <summary>
///     Describes a service with its service type, implementation, and lifetime.
/// </summary>
public class ServiceDescriptor
{
    /// <summary>
    ///     Gets the service key, which is used to resolve the service from the container.
    /// </summary>
    public object? ServiceKey { get; }

    /// <summary>
    ///     Gets the type of the service.
    /// </summary>
    /// <value>The <see cref="Type"/> of the service.</value>
    public Type ServiceType { get; }

    /// <summary>
    ///     Gets the type of the implementation.
    /// </summary>
    /// <value>The <see cref="Type"/> of the implementation of the service.</value>
    public Type? ImplementationType { get; }

    /// <summary>
    ///     Gets the concrete implementation, which gets returned to the user.
    /// </summary>
    /// <value>The implementation of the service.</value>
    public object? ImplementationInstance { get; internal set; }

    /// <summary>
    ///     Specifies the lifetime of a service in an <see cref="IServiceCollection" />.
    /// </summary>
    /// <value>The <see cref="ServiceLifetime"/> of the service.</value>
    public ServiceLifetime Lifetime { get; }

    /// <summary>
    ///     A factory used for creating service instances.
    /// </summary>
    /// <value>
    ///     The factory used for creating service instances.
    /// </value>
    public Func<IServiceProvider, object>? ImplementationFactory { get; }

    internal Type? GetImplementationType()
    {
        if (ImplementationType is not null) return ImplementationType;
        if (ImplementationInstance is not null) return ImplementationInstance.GetType();
        if (ImplementationFactory is null) return null;

        var typeArguments = ImplementationFactory.GetType().GenericTypeArguments;
        Debug.Assert(typeArguments.Length == 2);
        return typeArguments[1];
    }

    /// <summary>
    /// 	Initialises a new instance of the <see cref="ServiceDescriptor"/> class.
    /// </summary>
    /// <param name="implementation">The concrete implementation, which gets returned to the user.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    public ServiceDescriptor(object implementation, ServiceLifetime lifetime, object? serviceKey)
    {
        ServiceType = implementation.GetType();
        ImplementationInstance = implementation;
        ImplementationType = implementation.GetType();
        Lifetime = lifetime;
        ServiceKey = serviceKey;
    }

    /// <summary>
    /// 	Initialises a new instance of the <see cref="ServiceDescriptor"/> class.
    /// </summary>
    /// <param name="serviceType">The <see cref="Type"/> of the service.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    public ServiceDescriptor(Type serviceType, ServiceLifetime lifetime, object? serviceKey = null)
    {
        ServiceType = serviceType;
        Lifetime = lifetime;
        ServiceKey = serviceKey;
    }

    /// <summary>
    ///     Initialises a new instance of <see cref="ServiceDescriptor"/> with the specified <paramref name="implementationType"/>.
    /// </summary>
    /// <param name="serviceType">The <see cref="Type"/> of the service.</param>
    /// <param name="implementationType">The <see cref="Type"/> implementing the service.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime, object? serviceKey = null)
    {
        ServiceType = serviceType;
        Lifetime = lifetime;
        ImplementationType = implementationType;
        ServiceKey = serviceKey;
    }

    /// <summary>
    /// 	Initialises a new instance of the <see cref="ServiceDescriptor"/> class.
    /// </summary>
    /// <param name="serviceType">The <see cref="Type"/> of the service.</param>
    /// <param name="implementation">The concrete implementation, which gets returned to the user.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    public ServiceDescriptor(Type serviceType, object implementation, ServiceLifetime lifetime, object? serviceKey = null)
    {
        ServiceType = serviceType;
        Lifetime = lifetime;
        ImplementationType = implementation.GetType();
        ImplementationInstance = implementation;
        ServiceKey = serviceKey;
    }

    /// <summary>
    /// 	Initialises a new instance of the <see cref="ServiceDescriptor"/> class.
    /// </summary>
    /// <param name="serviceType">The <see cref="Type"/> of the service.</param>
    /// <param name="factory">A factory used for creating service instances.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    public ServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime, object? serviceKey = null)
        : this(serviceType, lifetime, serviceKey)
    {
        serviceType.ThrowIfNull();
        factory.ThrowIfNull();
        ImplementationFactory = factory;
    }

    /// <summary>
    ///     Initialises a new instance of <see cref="ServiceDescriptor"/> with the specified <paramref name="instance"/>
    /// as a <see cref="ServiceLifetime.Singleton"/>.
    /// </summary>
    /// <param name="serviceType">The <see cref="Type"/> of the service.</param>
    /// <param name="instance">The instance implementing the service.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    public ServiceDescriptor(Type serviceType, object instance, object? serviceKey = null)
        : this(serviceType, ServiceLifetime.Singleton, serviceKey)
    {
        serviceType.ThrowIfNull();
        instance.ThrowIfNull();
        ImplementationInstance = instance;
    }

#region Static Factory Constructors

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
    ///     and the <see cref="ServiceLifetime.Transient"/> lifetime.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Transient<TService, TImplementation>(object? serviceKey = null)
        where TService : class
        where TImplementation : class, TService
    {
        return Describe<TService, TImplementation>(ServiceLifetime.Transient, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <paramref name="service"/> and <paramref name="implementationType"/>
    ///     and the <see cref="ServiceLifetime.Transient"/> lifetime.
    /// </summary>
    /// <param name="service">The type of the service.</param>
    /// <param name="implementationType">The type of the implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Transient(Type service, Type implementationType, object? serviceKey = null)
    {
        service.ThrowIfNull();
        implementationType.ThrowIfNull();
        return Describe(service, implementationType, ServiceLifetime.Transient, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
    ///     <paramref name="implementationFactory"/>,
    ///     and the <see cref="ServiceLifetime.Transient"/> lifetime.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
    /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Transient<TService, TImplementation>(
        Func<IServiceProvider, TImplementation> implementationFactory, object? serviceKey = null)
        where TService : class
        where TImplementation : class, TService
    {
        implementationFactory.ThrowIfNull();
        return Describe(typeof(TService), implementationFactory, ServiceLifetime.Transient, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <typeparamref name="TService"/>, <paramref name="implementationFactory"/>,
    ///     and the <see cref="ServiceLifetime.Transient"/> lifetime.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Transient<TService>(Func<IServiceProvider, TService> implementationFactory, object? serviceKey = null)
        where TService : class
    {
        implementationFactory.ThrowIfNull();
        return Describe(typeof(TService), implementationFactory, ServiceLifetime.Transient, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <paramref name="service"/>, <paramref name="implementationFactory"/>,
    ///     and the <see cref="ServiceLifetime.Transient"/> lifetime.
    /// </summary>
    /// <param name="service">The type of the service.</param>
    /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Transient(Type service, Func<IServiceProvider, object> implementationFactory, object? serviceKey = null)
    {
        service.ThrowIfNull();
        implementationFactory.ThrowIfNull();
        return Describe(service, implementationFactory, ServiceLifetime.Transient, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
    ///     and the <see cref="ServiceLifetime.Scoped"/> lifetime.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Scoped<TService, TImplementation>(object? serviceKey = null)
        where TService : class
        where TImplementation : class, TService
    {
        return Describe<TService, TImplementation>(ServiceLifetime.Scoped, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <paramref name="service"/> and <paramref name="implementationType"/>
    ///     and the <see cref="ServiceLifetime.Scoped"/> lifetime.
    /// </summary>
    /// <param name="service">The type of the service.</param>
    /// <param name="implementationType">The type of the implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Scoped(Type service, Type implementationType, object? serviceKey = null)
    {
        service.ThrowIfNull();
        implementationType.ThrowIfNull();
        return Describe(service, implementationType, ServiceLifetime.Scoped, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
    ///     <paramref name="implementationFactory"/>,
    ///     and the <see cref="ServiceLifetime.Scoped"/> lifetime.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
    /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Scoped<TService, TImplementation>(
        Func<IServiceProvider, TImplementation> implementationFactory, object? serviceKey = null)
        where TService : class
        where TImplementation : class, TService
    {
        implementationFactory.ThrowIfNull();
        return Describe(typeof(TService), implementationFactory, ServiceLifetime.Scoped, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <typeparamref name="TService"/>, <paramref name="implementationFactory"/>,
    ///     and the <see cref="ServiceLifetime.Scoped"/> lifetime.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Scoped<TService>(Func<IServiceProvider, TService> implementationFactory, object? serviceKey = null)
        where TService : class
    {
        implementationFactory.ThrowIfNull();
        return Describe(typeof(TService), implementationFactory, ServiceLifetime.Scoped, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <paramref name="service"/>, <paramref name="implementationFactory"/>,
    ///     and the <see cref="ServiceLifetime.Scoped"/> lifetime.
    /// </summary>
    /// <param name="service">The type of the service.</param>
    /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Scoped(Type service, Func<IServiceProvider, object> implementationFactory, object? serviceKey = null)
    {
        service.ThrowIfNull();
        implementationFactory.ThrowIfNull();
        return Describe(service, implementationFactory, ServiceLifetime.Scoped, serviceKey);
    }
    
    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
    ///     and the <see cref="ServiceLifetime.Singleton"/> lifetime.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Singleton<TService, TImplementation>(object? serviceKey = null)
        where TService : class
        where TImplementation : class, TService
    {
        return Describe<TService, TImplementation>(ServiceLifetime.Singleton, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <paramref name="service"/> and <paramref name="implementationType"/>
    ///     and the <see cref="ServiceLifetime.Singleton"/> lifetime.
    /// </summary>
    /// <param name="service">The type of the service.</param>
    /// <param name="implementationType">The type of the implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Singleton(Type service, Type implementationType, object? serviceKey = null)
    {
        service.ThrowIfNull();
        implementationType.ThrowIfNull();
        return Describe(service, implementationType, ServiceLifetime.Singleton, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <typeparamref name="TService"/>, <typeparamref name="TImplementation"/>,
    ///     <paramref name="implementationFactory"/>,
    ///     and the <see cref="ServiceLifetime.Singleton"/> lifetime.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
    /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Singleton<TService, TImplementation>(
        Func<IServiceProvider, TImplementation> implementationFactory, object? serviceKey = null)
        where TService : class
        where TImplementation : class, TService
    {
        implementationFactory.ThrowIfNull();
        return Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <typeparamref name="TService"/>, <paramref name="implementationFactory"/>,
    ///     and the <see cref="ServiceLifetime.Singleton"/> lifetime.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Singleton<TService>(Func<IServiceProvider, TService> implementationFactory, object? serviceKey = null)
        where TService : class
    {
        implementationFactory.ThrowIfNull();
        return Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <paramref name="serviceType"/>, <paramref name="implementationFactory"/>,
    ///     and the <see cref="ServiceLifetime.Singleton"/> lifetime.
    /// </summary>
    /// <param name="serviceType">The type of the service.</param>
    /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Singleton(
        Type serviceType,
        Func<IServiceProvider, object> implementationFactory, object? serviceKey = null)
    {
        implementationFactory.ThrowIfNull();
        return Describe(serviceType, implementationFactory, ServiceLifetime.Singleton, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <typeparamref name="TService"/>, <paramref name="implementationInstance"/>,
    ///     and the <see cref="ServiceLifetime.Singleton"/> lifetime.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="implementationInstance">The instance of the implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Singleton<TService>(TService implementationInstance, object? serviceKey = null)
        where TService : class
    {
        implementationInstance.ThrowIfNull();
        return Singleton(typeof(TService), implementationInstance, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <paramref name="serviceType"/>, <paramref name="implementationInstance"/>,
    ///     and the <see cref="ServiceLifetime.Singleton"/> lifetime.
    /// </summary>
    /// <param name="serviceType">The type of the service.</param>
    /// <param name="implementationInstance">The instance of the implementation.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Singleton(
        Type serviceType,
        object implementationInstance, object? serviceKey = null)
    {
        serviceType.ThrowIfNull();
        implementationInstance.ThrowIfNull();
        return new ServiceDescriptor(serviceType, implementationInstance, serviceKey);
    }

    private static ServiceDescriptor Describe<TService, TImplementation>(ServiceLifetime lifetime, object? serviceKey = null)
        where TService : class
        where TImplementation : class, TService
    {
        return Describe(
            typeof(TService),
            typeof(TImplementation),
            lifetime: lifetime,
            serviceKey: serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <paramref name="serviceType"/>, <paramref name="implementationType"/>,
    ///     and <paramref name="lifetime"/>.
    /// </summary>
    /// <param name="serviceType">The type of the service.</param>
    /// <param name="implementationType">The type of the implementation.</param>
    /// <param name="lifetime">The lifetime of the service.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Describe(Type serviceType, Type implementationType, ServiceLifetime lifetime, object? serviceKey = null)
    {
        return new ServiceDescriptor(serviceType, implementationType, lifetime, serviceKey);
    }

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified
    ///     <paramref name="serviceType"/>, <paramref name="implementationFactory"/>,
    ///     and <paramref name="lifetime"/>.
    /// </summary>
    /// <param name="serviceType">The type of the service.</param>
    /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
    /// <param name="lifetime">The lifetime of the service.</param>
    /// <param name="serviceKey">The key used to resolve the service from the container.</param>
    /// <returns>A new instance of <see cref="ServiceDescriptor"/>.</returns>
    public static ServiceDescriptor Describe(Type serviceType, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime, object? serviceKey = null)
    {
        return new ServiceDescriptor(serviceType, implementationFactory, lifetime, serviceKey);
    }

#endregion
}