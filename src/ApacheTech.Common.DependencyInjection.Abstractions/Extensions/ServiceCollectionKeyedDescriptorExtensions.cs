using System;

namespace ApacheTech.Common.DependencyInjection.Abstractions.Extensions;

/// <summary>
///     Extension methods for adding and removing services to an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionKeyedDescriptorExtensions
{
    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Transient"/> service
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    public static void TryAddKeyedTransient(
        this IServiceCollection services,
        object serviceKey,
        Type service)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        serviceKey.ThrowIfNull();

        var descriptor = ServiceDescriptor.Transient(service, service, serviceKey);
        services.TryAdd(descriptor);
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Transient"/> service
    ///     with the <paramref name="implementationType"/> implementation
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    public static void TryAddKeyedTransient(
        this IServiceCollection services,
        object serviceKey,
        Type service,
        Type implementationType)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        implementationType.ThrowIfNull();
        serviceKey.ThrowIfNull();

        var descriptor = ServiceDescriptor.Transient(service, implementationType, serviceKey);
        services.TryAdd(descriptor);
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Transient"/> service
    ///     using the factory specified in <paramref name="implementationFactory"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    public static void TryAddKeyedTransient(
        this IServiceCollection services,
        object serviceKey,
        Type service,
        Func<IServiceProvider, object> implementationFactory)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        implementationFactory.ThrowIfNull();
        serviceKey.ThrowIfNull();

        var descriptor = ServiceDescriptor.Transient(service, implementationFactory, serviceKey);
        services.TryAdd(descriptor);
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Transient"/> service
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    public static void TryAddKeyedTransient<TService>(this IServiceCollection services, object serviceKey)
        where TService : class
    {
        services.ThrowIfNull();
        serviceKey.ThrowIfNull();

        TryAddKeyedTransient(services, serviceKey, typeof(TService), typeof(TService));
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Transient"/> service
    ///     implementation type specified in <typeparamref name="TImplementation"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    public static void TryAddKeyedTransient<TService, TImplementation>(this IServiceCollection services, object serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        services.ThrowIfNull();
        serviceKey.ThrowIfNull();

        TryAddKeyedTransient(services, serviceKey, typeof(TService), typeof(TImplementation));
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Transient"/> service
    ///     using the factory specified in <paramref name="implementationFactory"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    public static void TryAddKeyedTransient<TService>(
        this IServiceCollection services,
        object serviceKey,
        Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        services.ThrowIfNull();
        implementationFactory.ThrowIfNull();
        serviceKey.ThrowIfNull();

        var descriptor = ServiceDescriptor.Transient(implementationFactory, serviceKey);
        services.TryAdd(descriptor);
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Scoped"/> service
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="service">The type of the service to register.</param>
    public static void TryAddKeyedScoped(
        this IServiceCollection services,
        object serviceKey,
        Type service)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        serviceKey.ThrowIfNull();

        var descriptor = ServiceDescriptor.Scoped(service, service, serviceKey);
        services.TryAdd(descriptor);
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Scoped"/> service
    ///     with the <paramref name="implementationType"/> implementation
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    public static void TryAddKeyedScoped(
        this IServiceCollection services,
        object serviceKey,
        Type service,
        Type implementationType)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        implementationType.ThrowIfNull();
        serviceKey.ThrowIfNull();

        var descriptor = ServiceDescriptor.Scoped(service, implementationType, serviceKey);
        services.TryAdd(descriptor);
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Scoped"/> service
    ///     using the factory specified in <paramref name="implementationFactory"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    public static void TryAddKeyedScoped(
        this IServiceCollection services,
        object serviceKey,
        Type service,
        Func<IServiceProvider, object> implementationFactory)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        implementationFactory.ThrowIfNull();
        serviceKey.ThrowIfNull();

        var descriptor = ServiceDescriptor.Scoped(service, implementationFactory, serviceKey);
        services.TryAdd(descriptor);
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Scoped"/> service
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    public static void TryAddKeyedScoped<TService>(this IServiceCollection services, object serviceKey)
        where TService : class
    {
        services.ThrowIfNull();
        serviceKey.ThrowIfNull();

        TryAddKeyedScoped(services, serviceKey, typeof(TService), typeof(TService));
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Scoped"/> service
    ///     implementation type specified in <typeparamref name="TImplementation"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    public static void TryAddKeyedScoped<TService, TImplementation>(this IServiceCollection services, object serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        services.ThrowIfNull();
        serviceKey.ThrowIfNull();

        TryAddKeyedScoped(services, serviceKey, typeof(TService), typeof(TImplementation));
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Scoped"/> service
    ///     using the factory specified in <paramref name="implementationFactory"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    public static void TryAddKeyedScoped<TService>(
        this IServiceCollection services,
        object serviceKey,
        Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        services.ThrowIfNull();
        implementationFactory.ThrowIfNull();
        serviceKey.ThrowIfNull();

        var descriptor = ServiceDescriptor.Scoped(implementationFactory, serviceKey);
        services.TryAdd(descriptor);
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Singleton"/> service
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="service">The type of the service to register.</param>
    public static void TryAddKeyedSingleton(
        this IServiceCollection services,
        object serviceKey,
        Type service)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        serviceKey.ThrowIfNull();

        var descriptor = ServiceDescriptor.Singleton(service, service, serviceKey);
        services.TryAdd(descriptor);
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Singleton"/> service
    ///     with the <paramref name="implementationType"/> implementation
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    public static void TryAddKeyedSingleton(
        this IServiceCollection services,
        object serviceKey,
        Type service,
        Type implementationType)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        implementationType.ThrowIfNull();
        serviceKey.ThrowIfNull();

        var descriptor = ServiceDescriptor.Singleton(service, implementationType, serviceKey);
        services.TryAdd(descriptor);
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Singleton"/> service
    ///     using the factory specified in <paramref name="implementationFactory"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    public static void TryAddKeyedSingleton(
        this IServiceCollection services,
        object serviceKey,
        Type service,
        Func<IServiceProvider, object> implementationFactory)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        implementationFactory.ThrowIfNull();
        serviceKey.ThrowIfNull();

        var descriptor = ServiceDescriptor.Singleton(service, implementationFactory, serviceKey);
        services.TryAdd(descriptor);
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Singleton"/> service
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    public static void TryAddKeyedSingleton<TService>(this IServiceCollection services, object serviceKey)
        where TService : class
    {
        services.ThrowIfNull();
        serviceKey.ThrowIfNull();

        TryAddKeyedSingleton(services, serviceKey, typeof(TService), typeof(TService));
    }

    /// <summary>
    /// Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Singleton"/> service
    /// implementation type specified in <typeparamref name="TImplementation"/>
    /// to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    public static void TryAddKeyedSingleton<TService, TImplementation>(this IServiceCollection services, object serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        services.ThrowIfNull();
        serviceKey.ThrowIfNull();

        TryAddKeyedSingleton(services, serviceKey, typeof(TService), typeof(TImplementation));
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Singleton"/> service
    ///     with an instance specified in <paramref name="instance"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="instance">The instance of the service to add.</param>
    public static void TryAddKeyedSingleton<TService>(this IServiceCollection services, TService instance, object serviceKey)
        where TService : class
    {
        services.ThrowIfNull();
        instance.ThrowIfNull();
        serviceKey.ThrowIfNull();

        var descriptor = ServiceDescriptor.Singleton(typeof(TService), instance, serviceKey);
        services.TryAdd(descriptor);
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Singleton"/> service
    ///     using the factory specified in <paramref name="implementationFactory"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    public static void TryAddKeyedSingleton<TService>(
        this IServiceCollection services,
        object serviceKey,
        Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        services.ThrowIfNull();
        implementationFactory.ThrowIfNull();
        serviceKey.ThrowIfNull();

        services.TryAdd(ServiceDescriptor.Singleton(implementationFactory, serviceKey));
    }
}