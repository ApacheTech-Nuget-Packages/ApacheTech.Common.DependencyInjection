#nullable enable

using ApacheTech.Common.DependencyInjection.Abstractions;
using System;

namespace ApacheTech.Common.DependencyInjection;

internal sealed class RootServiceProvider(ServiceProvider root, ServiceLifetime? rootLifetime, ResolutionContext context) : IServiceProvider
{
    public object? GetService(Type serviceType) => root.ResolveType(serviceType, null, rootLifetime, context);
}