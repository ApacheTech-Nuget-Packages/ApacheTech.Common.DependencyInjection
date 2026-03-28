#nullable enable

using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.Extensions.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;

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
        var context = new ResolutionContext();

        if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            var itemType = serviceType.GetGenericArguments()[0];
            return ResolveEnumerable(itemType, null, null, context);
        }

        return ResolveType(serviceType, null, null, context);
    }

    /// <summary>
    ///     Disposes all services that require disposing.
    /// </summary>
    public void Dispose()
    {
        if (!_options.DisposeImplementations) return;

        var assemblies = _options.DisposableAssemblies;
        var filterByAssembly = assemblies is not null && assemblies.Count != 0;
        var descriptors = _serviceDescriptors.Where(d =>
            d.Lifetime == ServiceLifetime.Singleton &&
            d.ImplementationInstance is IDisposable or IAsyncDisposable &&
            (!filterByAssembly || assemblies!.Contains(d.ImplementationInstance.GetType().Assembly)));

        foreach (var descriptor in descriptors)
        {
            switch (descriptor.ImplementationInstance!)
            {
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
                case IAsyncDisposable asyncDisposable:
                    asyncDisposable.DisposeAsync().RunOnce();
                    break;
            }
        }
    }

    /// <summary>
    ///     Creates a new scope for resolving scoped services. Scoped services are disposed when the scope is disposed.
    /// </summary>
    /// <returns>A new <see cref="IServiceScope"/>.</returns>
    public IServiceScope CreateScope() => new ServiceScope(this);

    internal object? ResolveType(Type serviceType, ServiceScope? scope, ServiceLifetime? rootLifetime, ResolutionContext context)
    {
        var descriptor = _serviceDescriptors.SingleOrDefault(p => p.ServiceType == serviceType)
            ?? throw new InvalidOperationException($"Service of type {serviceType.FullName} is not registered.");

        if (descriptor.Lifetime is ServiceLifetime.Singleton && descriptor.ImplementationInstance is not null)
            return descriptor.ImplementationInstance;

        context.Enter(serviceType);
        var effectiveRootLifetime = rootLifetime ?? descriptor.Lifetime;

        try
        {
            // Handle scoped
            if (descriptor.Lifetime is ServiceLifetime.Scoped)
            {
                if (effectiveRootLifetime is ServiceLifetime.Singleton && _options.ValidateScopes)
                {
                    throw new InvalidOperationException(
                        $"Cannot consume scoped service '{serviceType.FullName}' from singleton '{effectiveRootLifetime}'.");
                }

                if (scope is null)
                {
                    if (_options.ValidateScopes)
                        throw new InvalidOperationException(
                            $"Cannot resolve scoped service '{serviceType.FullName}' from root provider.");

                    return CreateInstance(descriptor, null, effectiveRootLifetime, context);
                }

                if (scope.TryGet(descriptor, out var existing))
                    return existing;

                var instance = CreateInstance(descriptor, scope, effectiveRootLifetime, context);
                scope.Set(descriptor, instance);

                return instance;
            }

            // Handle singleton
            if (descriptor.Lifetime is ServiceLifetime.Singleton)
            {
                if (descriptor.ImplementationInstance is not null)
                    return descriptor.ImplementationInstance;

                var instance = CreateInstance(descriptor, scope, effectiveRootLifetime, context);
                descriptor.ImplementationInstance = instance;

                return instance;
            }

            // Transient
            return CreateInstance(descriptor, scope, effectiveRootLifetime, context);
        }
        finally
        {
            context.Exit(serviceType);
        }
    }

    private Array ResolveEnumerable(Type itemType, ServiceScope? scope, ServiceLifetime? rootLifetime, ResolutionContext context)
    {
        if (scope is null && _options.ValidateScopes)
        {
            var hasScoped = _serviceDescriptors.Any(d =>
                d.ServiceType == itemType &&
                d.Lifetime is ServiceLifetime.Scoped);

            if (hasScoped)
            {
                throw new InvalidOperationException(
                    $"Cannot resolve IEnumerable<{itemType.Name}> from root provider because it contains scoped services.");
            }
        }

        var implementations = _serviceDescriptors
            .Where(d => d.ServiceType == itemType)
            .Select(d => ResolveDescriptorDirect(d, scope, rootLifetime, context))
            .ToList();

        var array = Array.CreateInstance(itemType, implementations.Count);

        for (var i = 0; i < implementations.Count; i++)
        {
            array.SetValue(implementations[i], i);
        }

        return array;
    }

    private object? ResolveDescriptorDirect(ServiceDescriptor descriptor, ServiceScope? scope, ServiceLifetime? rootLifetime, ResolutionContext context)
    {
        var effectiveRootLifetime = rootLifetime ?? descriptor.Lifetime;

        if (descriptor.Lifetime is ServiceLifetime.Scoped)
        {
            if (effectiveRootLifetime is ServiceLifetime.Singleton && _options.ValidateScopes)
            {
                throw new InvalidOperationException(
                    $"Cannot consume scoped service '{descriptor.ServiceType.FullName}' from singleton.");
            }

            if (scope is null)
                throw new InvalidOperationException("Cannot resolve scoped service without a scope.");

            if (scope.TryGet(descriptor, out var existing))
                return existing;

            var instance = CreateInstance(descriptor, scope, effectiveRootLifetime, context);
            scope.Set(descriptor, instance);

            return instance;
        }

        if (descriptor.Lifetime is ServiceLifetime.Singleton)
        {
            if (descriptor.ImplementationInstance is not null)
                return descriptor.ImplementationInstance;

            var instance = CreateInstance(descriptor, scope, effectiveRootLifetime, context);
            descriptor.ImplementationInstance = instance;

            return instance;
        }

        return CreateInstance(descriptor, scope, effectiveRootLifetime, context);
    }

    private object CreateInstance(ServiceDescriptor descriptor, ServiceScope? scope, ServiceLifetime? rootLifetime, ResolutionContext context)
    {
        IServiceProvider provider = scope is null
            ? new RootServiceProvider(this, rootLifetime, context)
            : new ScopedServiceProvider(this, scope, rootLifetime, context);
        if (descriptor.ImplementationFactory is not null)
        {
            return descriptor.ImplementationFactory(provider);
        }

        var implementationType = descriptor.ImplementationType ?? descriptor.ServiceType;

        if (implementationType.IsInterface)
            throw new TypeLoadException($"Cannot instantiate interface: {implementationType.FullName}");

        if (implementationType.IsAbstract)
            throw new TypeLoadException($"Cannot instantiate abstract class: {implementationType.FullName}");

        return ActivatorUtilities.CreateInstance(provider, implementationType);
    }
}