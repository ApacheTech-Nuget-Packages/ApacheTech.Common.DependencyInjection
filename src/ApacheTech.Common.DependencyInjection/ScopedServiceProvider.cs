using ApacheTech.Common.DependencyInjection.Abstractions;
using System;

namespace ApacheTech.Common.DependencyInjection;

internal sealed class ScopedServiceProvider(ServiceProvider root, ServiceScope scope, ServiceLifetime? rootLifetime, ResolutionContext? context) : IKeyedServiceProvider
{
    internal ServiceScope Scope => scope;

    public object? GetService(Type serviceType) 
        => root.ResolveType(serviceType, scope, rootLifetime, context ??= new());

    public object? GetService(Type serviceType, object? serviceKey = null)
        => root.ResolveType(serviceType, scope, rootLifetime, context ??= new(), serviceKey);
}