using System;
using System.Collections.Generic;
using System.Linq;

namespace ApacheTech.Common.DependencyInjection.Abstractions.Extensions;

/// <summary>
///     Extension methods for adding and removing services to an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionDescriptorExtensions
{
    /// <summary>
    ///     Adds the specified <paramref name="descriptor"/> to the <paramref name="services"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="descriptor">The <see cref="ServiceDescriptor"/> to add.</param>
    /// <returns>A reference to the current instance of <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection Add(
        this IServiceCollection services,
        ServiceDescriptor descriptor)
    {
        services.ThrowIfNull();
        descriptor.ThrowIfNull();

        services.Add(descriptor);
        return services;
    }

    /// <summary>
    ///     Adds a sequence of <see cref="ServiceDescriptor"/> to the <paramref name="services"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="descriptors">The <see cref="ServiceDescriptor"/>s to add.</param>
    /// <returns>A reference to the current instance of <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection Add(
        this IServiceCollection services,
        IEnumerable<ServiceDescriptor> descriptors)
    {
        services.ThrowIfNull();
        descriptors.ThrowIfNull();

        foreach (var descriptor in descriptors.Where(d => d is not null))
        {
            services.Add(descriptor);
        }

        return services;
    }

    /// <summary>
    ///     Adds the specified <paramref name="descriptor"/> to the <paramref name="services"/> if the
    ///     service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="descriptor">The <see cref="ServiceDescriptor"/> to add.</param>
    public static IServiceCollection TryAdd(
        this IServiceCollection services,
        ServiceDescriptor descriptor)
    {
        services.ThrowIfNull();
        descriptor.ThrowIfNull();

        var count = services.Count;
        for (var i = 0; i < count; i++)
        {
            if (services[i].ServiceType == descriptor.ServiceType)
            {
                // Already added
                return services;
            }
        }

        services.Add(descriptor);
        return services;
    }

    /// <summary>
    ///     Adds the specified <paramref name="descriptors"/> to the <paramref name="services"/> if the
    ///     service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="descriptors">The <see cref="ServiceDescriptor"/>s to add.</param>
    public static IServiceCollection TryAdd(
        this IServiceCollection services,
        IEnumerable<ServiceDescriptor> descriptors)
    {
        services.ThrowIfNull();
        descriptors.ThrowIfNull();

        foreach (var d in descriptors.Where(d => d is not null))
        {
            services.TryAdd(d);
        }
        return services;
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Transient"/> service
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="service">The type of the service to register.</param>
    public static IServiceCollection TryAddTransient(
        this IServiceCollection services, Type service)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();

        var descriptor = ServiceDescriptor.Transient(service, service);
        TryAdd(services, descriptor);
        return services;
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Transient"/> service
    ///     with the <paramref name="implementationType"/> implementation
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    public static IServiceCollection TryAddTransient(
        this IServiceCollection services,
        Type service,
        Type implementationType)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        implementationType.ThrowIfNull();

        var descriptor = ServiceDescriptor.Transient(service, implementationType);
        TryAdd(services, descriptor);
        return services;
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Transient"/> service
    ///     using the factory specified in <paramref name="implementationFactory"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    public static IServiceCollection TryAddTransient(
        this IServiceCollection services,
        Type service,
        Func<IServiceProvider, object> implementationFactory)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        implementationFactory.ThrowIfNull();

        var descriptor = ServiceDescriptor.Transient(service, implementationFactory);
        TryAdd(services, descriptor);
        return services;
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Transient"/> service
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static IServiceCollection TryAddTransient<TService>(this IServiceCollection services)
        where TService : class
    {
        services.ThrowIfNull();

        TryAddTransient(services, typeof(TService), typeof(TService));
        return services;
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Transient"/> service
    ///     implementation type specified in <typeparamref name="TImplementation"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static IServiceCollection TryAddTransient<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.ThrowIfNull();

        TryAddTransient(services, typeof(TService), typeof(TImplementation));
        return services;
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Transient"/> service
    ///     using the factory specified in <paramref name="implementationFactory"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    public static IServiceCollection TryAddTransient<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        services.TryAdd(ServiceDescriptor.Transient(implementationFactory));
        return services;
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Scoped"/> service
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="service">The type of the service to register.</param>
    public static IServiceCollection TryAddScoped(
        this IServiceCollection services, Type service)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();

        var descriptor = ServiceDescriptor.Scoped(service, service);
        TryAdd(services, descriptor);
        return services;
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Scoped"/> service
    ///     with the <paramref name="implementationType"/> implementation
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    public static IServiceCollection TryAddScoped(
        this IServiceCollection services,
        Type service,
        Type implementationType)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        implementationType.ThrowIfNull();

        var descriptor = ServiceDescriptor.Scoped(service, implementationType);
        TryAdd(services, descriptor);
        return services;
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Scoped"/> service
    ///     using the factory specified in <paramref name="implementationFactory"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    public static IServiceCollection TryAddScoped(
        this IServiceCollection services,
        Type service,
        Func<IServiceProvider, object> implementationFactory)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        implementationFactory.ThrowIfNull();

        var descriptor = ServiceDescriptor.Scoped(service, implementationFactory);
        TryAdd(services, descriptor);
        return services;
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Scoped"/> service
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static IServiceCollection TryAddScoped<TService>(this IServiceCollection services)
        where TService : class
    {
        services.ThrowIfNull();

        TryAddScoped(services, typeof(TService), typeof(TService));
        return services;
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Scoped"/> service
    ///     implementation type specified in <typeparamref name="TImplementation"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static IServiceCollection TryAddScoped<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.ThrowIfNull();

        TryAddScoped(services, typeof(TService), typeof(TImplementation));
        return services;
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Scoped"/> service
    ///     using the factory specified in <paramref name="implementationFactory"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    public static IServiceCollection TryAddScoped<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        services.TryAdd(ServiceDescriptor.Scoped(implementationFactory));
        return services;
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Singleton"/> service
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="service">The type of the service to register.</param>
    public static IServiceCollection TryAddSingleton(
        this IServiceCollection services,
        Type service)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();

        var descriptor = ServiceDescriptor.Singleton(service, service);
        TryAdd(services, descriptor);
        return services;
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Singleton"/> service
    ///     with the <paramref name="implementationType"/> implementation
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    public static IServiceCollection TryAddSingleton(
        this IServiceCollection services,
        Type service,
        Type implementationType)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        implementationType.ThrowIfNull();

        var descriptor = ServiceDescriptor.Singleton(service, implementationType);
        TryAdd(services, descriptor);
        return services;
    }

    /// <summary>
    ///     Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Singleton"/> service
    ///     using the factory specified in <paramref name="implementationFactory"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    public static IServiceCollection TryAddSingleton(
        this IServiceCollection services,
        Type service,
        Func<IServiceProvider, object> implementationFactory)
    {
        services.ThrowIfNull();
        service.ThrowIfNull();
        implementationFactory.ThrowIfNull();

        var descriptor = ServiceDescriptor.Singleton(service, implementationFactory);
        TryAdd(services, descriptor);
        return services;
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Singleton"/> service
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection services)
        where TService : class
    {
        services.ThrowIfNull();

        TryAddSingleton(services, typeof(TService), typeof(TService));
        return services;
    }

    /// <summary>
    /// Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Singleton"/> service
    /// implementation type specified in <typeparamref name="TImplementation"/>
    /// to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static IServiceCollection TryAddSingleton<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.ThrowIfNull();

        TryAddSingleton(services, typeof(TService), typeof(TImplementation));
        return services;
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Singleton"/> service
    ///     with an instance specified in <paramref name="instance"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="instance">The instance of the service to add.</param>
    public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection services, TService instance)
        where TService : class
    {
        services.ThrowIfNull();
        instance.ThrowIfNull();

        var descriptor = ServiceDescriptor.Singleton(typeof(TService), instance);
        TryAdd(services, descriptor);
        return services;
    }

    /// <summary>
    ///     Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Singleton"/> service
    ///     using the factory specified in <paramref name="implementationFactory"/>
    ///     to the <paramref name="services"/> if the service type hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    public static IServiceCollection TryAddSingleton<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        services.TryAdd(ServiceDescriptor.Singleton(implementationFactory));
        return services;
    }

    /// <summary>
    ///     Adds a <see cref="ServiceDescriptor"/> if an existing descriptor with the same
    ///     <see cref="ServiceDescriptor.ServiceType"/> and an implementation that does not already exist
    ///     in <paramref name="services."/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="descriptor">The <see cref="ServiceDescriptor"/>.</param>
    /// <remarks>
    /// Use <see cref="TryAddEnumerable(IServiceCollection, ServiceDescriptor)"/> when registering a service implementation of a
    /// service type that
    /// supports multiple registrations of the same service type. Using
    /// <see cref="Add(IServiceCollection, ServiceDescriptor)"/> is not idempotent and can add
    /// duplicate
    /// <see cref="ServiceDescriptor"/> instances if called twice. Using
    /// <see cref="TryAddEnumerable(IServiceCollection, ServiceDescriptor)"/> will prevent registration
    /// of multiple implementation types.
    /// </remarks>
    public static IServiceCollection TryAddEnumerable(
        this IServiceCollection services,
        ServiceDescriptor descriptor)
    {
        services.ThrowIfNull();
        descriptor.ThrowIfNull();

        var implementationType = descriptor.GetImplementationType();
        
        if (implementationType == typeof(object) ||
            implementationType == descriptor.ServiceType)
        {
            throw new TypeLoadException(
                $"Cannot add {nameof(descriptor)} to the service services. Cannot determine implementation type.");
        }

        var count = services.Count;
        for (var i = 0; i < count; i++)
        {
            var service = services[i];
            if (service.ServiceType == descriptor.ServiceType &&
                service.GetImplementationType() == implementationType)
            {
                // Already added
                return services;
            }
        }

        services.Add(descriptor);
        return services;
    }

    /// <summary>
    ///     Adds the specified <see cref="ServiceDescriptor"/>s if an existing descriptor with the same
    ///     <see cref="ServiceDescriptor.ServiceType"/> and an implementation that does not already exist
    ///     in <paramref name="services."/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="descriptors">The <see cref="ServiceDescriptor"/>s.</param>
    /// <remarks>
    /// Use <see cref="TryAddEnumerable(IServiceCollection, ServiceDescriptor)"/> when registering a service
    /// implementation of a service type that
    /// supports multiple registrations of the same service type. Using
    /// <see cref="Add(IServiceCollection, ServiceDescriptor)"/> is not idempotent and can add
    /// duplicate
    /// <see cref="ServiceDescriptor"/> instances if called twice. Using
    /// <see cref="TryAddEnumerable(IServiceCollection, ServiceDescriptor)"/> will prevent registration
    /// of multiple implementation types.
    /// </remarks>
    public static IServiceCollection TryAddEnumerable(
        this IServiceCollection services,
        IEnumerable<ServiceDescriptor> descriptors)
    {
        services.ThrowIfNull();
        descriptors.ThrowIfNull();

        foreach (var d in descriptors)
        {
            services.TryAddEnumerable(d);
        }
        return services;
    }

    /// <summary>
    ///     Removes the first service in <see cref="IServiceCollection"/> with the same service type
    ///     as <paramref name="descriptor"/> and adds <paramref name="descriptor"/> to the services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="descriptor">The <see cref="ServiceDescriptor"/> to replace with.</param>
    /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection Replace(
        this IServiceCollection services,
        ServiceDescriptor descriptor)
    {
        services.ThrowIfNull();
        descriptor.ThrowIfNull();

        // Remove existing
        var count = services.Count;
        for (var i = 0; i < count; i++)
        {
            if (services[i].ServiceType != descriptor.ServiceType) continue;
            services.RemoveAt(i);
            break;
        }

        services.Add(descriptor);
        return services;
    }

    /// <summary>
    ///     Removes all services of type <typeparamref name="T"/> in <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection RemoveAll<T>(this IServiceCollection services)
    {
        return RemoveAll(services, typeof(T));
    }

    /// <summary>
    ///     Removes all services of type <paramref name="serviceType"/> in <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceType">The service type to remove.</param>
    /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection RemoveAll(this IServiceCollection services, Type serviceType)
    {
        serviceType.ThrowIfNull();

        for (var i = services.Count - 1; i >= 0; i--)
        {
            var descriptor = services[i];
            if (descriptor.ServiceType == serviceType)
            {
                services.RemoveAt(i);
            }
        }

        return services;
    }
}