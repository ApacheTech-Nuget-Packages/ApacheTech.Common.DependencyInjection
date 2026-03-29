using System;
using System.Diagnostics.CodeAnalysis;

namespace ApacheTech.Common.DependencyInjection.Abstractions.Extensions;

/// <summary>
///     Extension methods for adding services to an <see cref="IServiceCollection" />.
/// </summary>
public static partial class ServiceCollectionKeyedServiceExtensions
{
    /// <summary>
    ///     Adds a keyed transient service of the type specified in <paramref name="serviceType"/> with an
    ///     implementation of the type specified in <paramref name="implementationType"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Transient"/>
    public static IServiceCollection AddKeyedTransient(
        this IServiceCollection services,
        object serviceKey,
        Type serviceType,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(serviceType);
        ArgumentNullException.ThrowIfNull(implementationType);

        return AddKeyed(services, serviceKey, serviceType, implementationType, ServiceLifetime.Transient);
    }

    /// <summary>
    ///     Adds a keyed transient service of the type specified in <paramref name="serviceType"/> with a
    ///     factory specified in <paramref name="implementationFactory"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Transient"/>
    public static IServiceCollection AddKeyedTransient(
        this IServiceCollection services,
        object serviceKey,
        Type serviceType,
        Func<IServiceProvider, object> implementationFactory)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(serviceType);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        return AddKeyed(services, serviceKey, serviceType, implementationFactory, ServiceLifetime.Transient);
    }

    /// <summary>
    ///     Adds a keyed transient service of the type specified in <typeparamref name="TService"/> with an
    ///     implementation type specified in <typeparamref name="TImplementation"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Transient"/>
    public static IServiceCollection AddKeyedTransient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services, object serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        return services.AddKeyedTransient(serviceKey, typeof(TService), typeof(TImplementation));
    }

    /// <summary>
    ///     Adds a keyed transient service of the type specified in <paramref name="serviceType"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Transient"/>
    public static IServiceCollection AddKeyedTransient(
        this IServiceCollection services,
        object serviceKey,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(serviceType);

        return services.AddKeyedTransient(serviceKey, serviceType, serviceType);
    }

    /// <summary>
    ///     Adds a keyed transient service of the type specified in <typeparamref name="TService"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Transient"/>
    public static IServiceCollection AddKeyedTransient<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services, object serviceKey)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);

        return services.AddKeyedTransient(serviceKey, typeof(TService));
    }

    /// <summary>
    ///     Adds a keyed transient service of the type specified in <typeparamref name="TService"/> with a
    ///     factory specified in <paramref name="implementationFactory"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Transient"/>
    public static IServiceCollection AddKeyedTransient<TService>(
        this IServiceCollection services,
        object serviceKey,
        Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        return services.AddKeyedTransient(serviceKey, typeof(TService), implementationFactory);
    }

    /// <summary>
    ///     Adds a keyed transient service of the type specified in <typeparamref name="TService"/> with an
    ///     implementation type specified in <typeparamref name="TImplementation" /> using the
    ///     factory specified in <paramref name="implementationFactory"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Transient"/>
    public static IServiceCollection AddKeyedTransient<TService, TImplementation>(
        this IServiceCollection services,
        object serviceKey,
        Func<IServiceProvider, TImplementation> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        return services.AddKeyedTransient(serviceKey, typeof(TService), implementationFactory);
    }

    /// <summary>
    ///     Adds a keyed scoped service of the type specified in <paramref name="serviceType"/> with an
    ///     implementation of the type specified in <paramref name="implementationType"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Scoped"/>
    public static IServiceCollection AddKeyedScoped(
        this IServiceCollection services,
        object serviceKey,
        Type serviceType,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(serviceType);
        ArgumentNullException.ThrowIfNull(implementationType);

        return AddKeyed(services, serviceKey, serviceType, implementationType, ServiceLifetime.Scoped);
    }

    /// <summary>
    ///     Adds a keyed scoped service of the type specified in <paramref name="serviceType"/> with a
    ///     factory specified in <paramref name="implementationFactory"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Scoped"/>
    public static IServiceCollection AddKeyedScoped(
        this IServiceCollection services,
        object serviceKey,
        Type serviceType,
        Func<IServiceProvider, object> implementationFactory)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceType);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        return AddKeyed(services, serviceKey, serviceType, implementationFactory, ServiceLifetime.Scoped);
    }

    /// <summary>
    ///     Adds a keyed scoped service of the type specified in <typeparamref name="TService"/> with an
    ///     implementation type specified in <typeparamref name="TImplementation"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Scoped"/>
    public static IServiceCollection AddKeyedScoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services, object serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);

        return services.AddKeyedScoped(serviceKey, typeof(TService), typeof(TImplementation));
    }

    /// <summary>
    ///     Adds a keyed scoped service of the type specified in <paramref name="serviceType"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Scoped"/>
    public static IServiceCollection AddKeyedScoped(
        this IServiceCollection services,
        object serviceKey,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceType);

        return services.AddKeyedScoped(serviceKey, serviceType, serviceType);
    }

    /// <summary>
    ///     Adds a keyed scoped service of the type specified in <typeparamref name="TService"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Scoped"/>
    public static IServiceCollection AddKeyedScoped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services, object serviceKey)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);

        return services.AddKeyedScoped(serviceKey, typeof(TService));
    }

    /// <summary>
    ///     Adds a keyed scoped service of the type specified in <typeparamref name="TService"/> with a
    ///     factory specified in <paramref name="implementationFactory"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Scoped"/>
    public static IServiceCollection AddKeyedScoped<TService>(
        this IServiceCollection services,
        object serviceKey,
        Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        return services.AddKeyedScoped(serviceKey, typeof(TService), implementationFactory);
    }

    /// <summary>
    ///     Adds a keyed scoped service of the type specified in <typeparamref name="TService"/> with an
    ///     implementation type specified in <typeparamref name="TImplementation" /> using the
    ///     factory specified in <paramref name="implementationFactory"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Scoped"/>
    public static IServiceCollection AddKeyedScoped<TService, TImplementation>(
        this IServiceCollection services,
        object serviceKey,
        Func<IServiceProvider, TImplementation> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        return services.AddKeyedScoped(serviceKey, typeof(TService), implementationFactory);
    }


    /// <summary>
    ///     Adds a keyed singleton service of the type specified in <paramref name="serviceType"/> with an
    ///     implementation of the type specified in <paramref name="implementationType"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static IServiceCollection AddKeyedSingleton(
        this IServiceCollection services,
        object serviceKey,
        Type serviceType,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(serviceType);
        ArgumentNullException.ThrowIfNull(implementationType);

        return AddKeyed(services, serviceKey, serviceType, implementationType, ServiceLifetime.Singleton);
    }

    /// <summary>
    ///     Adds a keyed singleton service of the type specified in <paramref name="serviceType"/> with a
    ///     factory specified in <paramref name="implementationFactory"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static IServiceCollection AddKeyedSingleton(
        this IServiceCollection services,
        object serviceKey,
        Type serviceType,
        Func<IServiceProvider, object> implementationFactory)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(serviceType);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        return AddKeyed(services, serviceKey, serviceType, implementationFactory, ServiceLifetime.Singleton);
    }

    /// <summary>
    ///     Adds a keyed singleton service of the type specified in <typeparamref name="TService"/> with an
    ///     implementation type specified in <typeparamref name="TImplementation"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static IServiceCollection AddKeyedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection services, object serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);

        return services.AddKeyedSingleton(serviceKey, typeof(TService), typeof(TImplementation));
    }

    /// <summary>
    ///     Adds a keyed singleton service of the type specified in <paramref name="serviceType"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static IServiceCollection AddKeyedSingleton(
        this IServiceCollection services,
        object serviceKey,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type serviceType)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(serviceType);

        return services.AddKeyedSingleton(serviceKey, serviceType, serviceType);
    }

    /// <summary>
    ///     Adds a keyed singleton service of the type specified in <typeparamref name="TService"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static IServiceCollection AddKeyedSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(this IServiceCollection services, object serviceKey)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);

        return services.AddKeyedSingleton(serviceKey, typeof(TService));
    }

    /// <summary>
    ///     Adds a keyed singleton service of the type specified in <typeparamref name="TService"/> with a
    ///     factory specified in <paramref name="implementationFactory"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static IServiceCollection AddKeyedSingleton<TService>(
        this IServiceCollection services,
        object serviceKey,
        Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        return services.AddKeyedSingleton(serviceKey, typeof(TService), implementationFactory);
    }

    /// <summary>
    ///     Adds a keyed singleton service of the type specified in <typeparamref name="TService"/> with an
    ///     implementation type specified in <typeparamref name="TImplementation" /> using the
    ///     factory specified in <paramref name="implementationFactory"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static IServiceCollection AddKeyedSingleton<TService, TImplementation>(
        this IServiceCollection services,
        object serviceKey,
        Func<IServiceProvider, TImplementation> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        return services.AddKeyedSingleton(serviceKey, typeof(TService), implementationFactory);
    }

    /// <summary>
    ///     Adds a keyed singleton service of the type specified in <paramref name="serviceType"/> with an
    ///     instance specified in <paramref name="implementationInstance"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationInstance">The instance of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static IServiceCollection AddKeyedSingleton(
        this IServiceCollection services,
        object serviceKey,
        Type serviceType,
        object implementationInstance)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(serviceType);
        ArgumentNullException.ThrowIfNull(implementationInstance);

        var serviceDescriptor = new ServiceDescriptor(serviceType, implementationInstance, serviceKey);
        services.Add(serviceDescriptor);
        return services;
    }

    /// <summary>
    ///     Adds a keyed singleton service of the type specified in <typeparamref name="TService" /> with an
    ///     instance specified in <paramref name="implementationInstance"/> to the
    ///     specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="serviceKey">The key to identify the service.</param>
    /// <param name="implementationInstance">The instance of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    ///     This method always adds a new registration to the <see cref="IServiceCollection"/>, even if a service of the same type has already been registered.
    ///     When multiple registrations exist, <see cref="IServiceProvider.GetService"/> returns the last registered service.
    ///     Use <see cref="ServiceProviderServiceExtensions.GetServices{T}(IServiceProvider)"/> to retrieve all registered services.
    /// </remarks>
    /// <seealso cref="ServiceLifetime.Singleton"/>
    public static IServiceCollection AddKeyedSingleton<TService>(
        this IServiceCollection services,
        object serviceKey,
        TService implementationInstance)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);
        ArgumentNullException.ThrowIfNull(implementationInstance);

        return services.AddKeyedSingleton(serviceKey, typeof(TService), implementationInstance);
    }

    private static IServiceCollection AddKeyed(
        this IServiceCollection services,
        object serviceKey,
        Type serviceType,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType,
        ServiceLifetime lifetime)
    {
        ArgumentNullException.ThrowIfNull(serviceKey);

        var descriptor = new ServiceDescriptor(serviceType, implementationType, lifetime, serviceKey);
        services.Add(descriptor);
        return services;
    }

    private static IServiceCollection AddKeyed(
        this IServiceCollection services,
        object serviceKey,
        Type serviceType,
        Func<IServiceProvider, object> implementationFactory,
        ServiceLifetime lifetime)
    {
        ArgumentNullException.ThrowIfNull(serviceKey);

        var descriptor = new ServiceDescriptor(serviceType, implementationFactory, lifetime, serviceKey);
        services.Add(descriptor);
        return services;
    }
}