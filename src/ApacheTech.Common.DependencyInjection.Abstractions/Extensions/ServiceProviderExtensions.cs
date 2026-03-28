using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ApacheTech.Common.DependencyInjection.Abstractions.Extensions;

/// <summary>
///     Extension methods for getting services from an <see cref="IServiceProvider" />.
/// </summary>
public static class ServiceProviderServiceExtensions
{
    /// <summary>
    ///     Creates an object of a specified type, using the IOC Container to resolve dependencies.
    /// </summary>
    /// <param name="serviceProvider">And object that provides access to the service collection.</param>
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <param name="args">An optional list of arguments, sent the constructor of the instantiated class.</param>
    /// <returns>A service object of type <paramref name="serviceType" />.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> if no object of type <paramref name="serviceType" /> can be instantiated from the service collection.</returns>
    public static object CreateInstance(this IServiceProvider serviceProvider, Type serviceType, params object[] args)
        => ActivatorUtilities.CreateInstance(serviceProvider, serviceType, args);

    /// <summary>
    ///     Creates an object of a specified type, using the IOC Container to resolve dependencies.
    /// </summary>
    /// <typeparam name="T">The type of object to create.</typeparam>
    /// <param name="serviceProvider">And object that provides access to the service collection.</param>
    /// <param name="args">An optional list of arguments, sent the constructor of the instantiated class.</param>
    /// <returns>An object of type <typeparamref name="T" />.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> if there is no object of type <typeparamref name="T" /> can be instantiated from the service collection.</returns>
    public static T CreateInstance<T>(this IServiceProvider serviceProvider, params object[] args) where T : class
        => (T)serviceProvider.CreateInstance(typeof(T), args);

    /// <summary>
    ///     Get service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the service object from.</param>
    /// <returns>A service object of type <typeparamref name="T"/> or null if there is no such service.</returns>
    public static T? GetService<T>(this IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        return (T?)provider.GetService(typeof(T));
    }

    /// <summary>
    ///     Gets the service object of the specified type.
    /// </summary>
    /// <param name="serviceProvider">And object that provides access to the service collection.</param>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <returns>A service object of type <typeparamref name="T" />.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> if there is no service object of type <typeparamref name="T" />.</returns>
    public static T Resolve<T>(this IServiceProvider serviceProvider) where T : notnull
        => serviceProvider.GetRequiredService<T>();

    /// <summary>
    ///     Get service of type <paramref name="serviceType"/> from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the service object from.</param>
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <returns>A service object of type <paramref name="serviceType"/>.</returns>
    /// <exception cref="System.InvalidOperationException">There is no service of type <paramref name="serviceType"/>.</exception>
    public static object GetRequiredService(this IServiceProvider provider, Type serviceType)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(serviceType);
        return provider.GetRequiredService(serviceType);
    }

    /// <summary>
    ///     Get service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the service object from.</param>
    /// <returns>A service object of type <typeparamref name="T"/>.</returns>
    /// <exception cref="System.InvalidOperationException">There is no service of type <typeparamref name="T"/>.</exception>
    public static T GetRequiredService<T>(this IServiceProvider provider) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(provider);

        return (T)provider.GetRequiredService(typeof(T));
    }

    /// <summary>
    ///     Get an enumeration of services of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the services from.</param>
    /// <returns>An enumeration of services of type <typeparamref name="T"/>.</returns>
    public static IEnumerable<T> GetServices<T>(this IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        return provider.GetRequiredService<IEnumerable<T>>();
    }

    /// <summary>
    ///     Get an enumeration of services of type <paramref name="serviceType"/> from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the services from.</param>
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <returns>An enumeration of services of type <paramref name="serviceType"/>.</returns>
    [RequiresDynamicCode("The native code for an IEnumerable<serviceType> might not be available at runtime.")]
    public static IEnumerable<object?> GetServices(this IServiceProvider provider, Type serviceType)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(serviceType);

        Type? genericEnumerable = typeof(IEnumerable<>).MakeGenericType(serviceType);
        return (IEnumerable<object>)provider.GetRequiredService(genericEnumerable);
    }

    /// <summary>
    ///     Creates a new <see cref="IServiceScope"/> that can be used to resolve scoped services.
    /// </summary>
    /// <param name="provider">The <see cref="IServiceProvider"/> to create the scope from.</param>
    /// <returns>A <see cref="IServiceScope"/> that can be used to resolve scoped services.</returns>
    public static IServiceScope CreateScope(this IServiceProvider provider)
    {
        return provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
    }
}