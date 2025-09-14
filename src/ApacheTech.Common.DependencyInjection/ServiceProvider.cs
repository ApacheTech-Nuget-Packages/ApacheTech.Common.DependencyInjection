#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using ApacheTech.Common.DependencyInjection.Abstractions;

namespace ApacheTech.Common.DependencyInjection;

/// <summary>
///     The default IServiceProvider.
/// </summary>
/// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
/// <param name="options"> Configures various service provider behaviours.</param>
public sealed class ServiceProvider(IEnumerable<ServiceDescriptor> services, ServiceProviderOptions options) : IServiceProvider, IDisposable
{
    private readonly IEnumerable<ServiceDescriptor> _serviceDescriptors = services;
    private readonly ServiceProviderOptions _options = options;

    /// <summary>
    ///     Gets the service object of the specified type.
    /// </summary>
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <returns>
    ///     A service object of type <paramref name="serviceType">serviceType</paramref>.   -or-  null if there is no service object of type <paramref name="serviceType">serviceType</paramref>.
    /// </returns>
    public object? GetService(Type serviceType)
    {
        // Check for explicitly registered IEnumerable<T>
        var descriptor = _serviceDescriptors.SingleOrDefault(p => p.ServiceType == serviceType);
        if (descriptor is not null)
        {
            return ResolveDescriptor(descriptor);
        }

        // Handle IEnumerable<T> when no explicit registration exists
        if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            var itemType = serviceType.GetGenericArguments()[0];
            return ResolveEnumerable(itemType);
        }

        return null;
    }

    /// <summary>
    ///     Disposes all services that require disposing.
    /// </summary>
    public void Dispose()
    {
        if (!_options.DisposeImplementations) return;

        var assemblies = _options.DisposableAssemblies;
        var filterByAssembly = assemblies is not null && assemblies.Any();
        var descriptors = _serviceDescriptors.Where(d =>
            d.Lifetime == ServiceLifetime.Singleton &&
            d.Implementation is IDisposable or IAsyncDisposable &&
            (!filterByAssembly || assemblies!.Contains(d.Implementation.GetType().Assembly)));

        foreach (var descriptor in descriptors)
        {
            switch (descriptor.Implementation!)
            {
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
                case IAsyncDisposable asyncDisposable:
                    asyncDisposable.DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    break;
            }
        }
    }

    private object? ResolveDescriptor(ServiceDescriptor descriptor)
    {
        if (descriptor.Implementation is not null)
        {
            return descriptor.Implementation;
        }

        if (descriptor.ImplementationFactory is not null)
        {
            return CacheService(descriptor, descriptor.ImplementationFactory(this));
        }

        var implementationType = descriptor.ImplementationType ?? descriptor.ServiceType;

        if (implementationType.IsInterface)
        {
            throw new TypeLoadException($"Cannot instantiate interface: {implementationType.FullName}");
        }

        if (implementationType.IsAbstract)
        {
            throw new TypeLoadException($"Cannot instantiate abstract class: {implementationType.FullName}");
        }

        var implementation = ActivatorUtilities.CreateInstance(this, implementationType);
        return CacheService(descriptor, implementation);
    }

    private object ResolveEnumerable(Type itemType)
    {
        var implementations = _serviceDescriptors
            .Where(d => d.ServiceType == itemType)
            .Select(ResolveDescriptor)
            .Where(instance => instance != null)
            .ToList();

        var array = Array.CreateInstance(itemType, implementations.Count);
        for (int i = 0; i < implementations.Count; i++)
        {
            array.SetValue(implementations[i], i);
        }

        return array;
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