using ApacheTech.Common.DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApacheTech.Common.DependencyInjection;

internal sealed class ServiceScope : IServiceScope
{
    private readonly ServiceProvider _root;
    private readonly Dictionary<ServiceDescriptor, object> _instances = [];

    public IServiceProvider ServiceProvider { get; }

    public ServiceScope(ServiceProvider root)
    {
        _root = root;
        ServiceProvider = new ScopedServiceProvider(root, this, null, null);
    }

    internal bool TryGet(ServiceDescriptor descriptor, out object? instance)
        => _instances.TryGetValue(descriptor, out instance);

    internal void Set(ServiceDescriptor descriptor, object instance)
        => _instances[descriptor] = instance;

    public void Dispose()
    {
        foreach (var instance in _instances.Values.OfType<IDisposable>())
        {
            instance.Dispose();
        }
    }
}