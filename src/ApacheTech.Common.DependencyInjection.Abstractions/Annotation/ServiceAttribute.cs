using System;

namespace ApacheTech.Common.DependencyInjection.Abstractions.Annotation;

/// <summary>
///     Denotes that this class should be registered as a service within the IOC container.
/// </summary>
/// <seealso cref="System.Attribute" />
/// <param name="serviceLifetime">The service lifetime.</param>
/// <param name="serviceType">The type of the representation of the registered class within the IOC Container.</param>
[AttributeUsage(AttributeTargets.Class)]
public abstract class ServiceAttribute(ServiceLifetime serviceLifetime, Type? serviceType = null) : Attribute
{
    private readonly ServiceLifetime _serviceLifetime = serviceLifetime;
    private readonly Type? _serviceType = serviceType;

    /// <summary>
    ///     Creates an instance of <see cref="ServiceDescriptor"/> with the specified <paramref name="implementationType"/>.
    /// </summary>
    /// <param name="implementationType">The type of the implementation.</param>
    public ServiceDescriptor Describe(Type implementationType) 
        => ServiceDescriptor.Describe(_serviceType ?? implementationType, implementationType, _serviceLifetime);
}