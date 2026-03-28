using ApacheTech.Common.DependencyInjection.Abstractions;
using System;

namespace ApacheTech.Common.DependencyInjection;

internal sealed class ScopedServiceProvider(ServiceProvider root, ServiceScope scope, ServiceLifetime? rootLifetime, ResolutionContext? context) : IServiceProvider
{
    internal ServiceScope Scope => scope;
    public object? GetService(Type serviceType) => root.ResolveType(serviceType, scope, rootLifetime, context ??= new());
}