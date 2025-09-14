using System;

namespace ApacheTech.Common.DependencyInjection.Abstractions.Annotation;

/// <summary>
///     Denotes that this class should be registered as a singleton service within the IOC container.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute(Type? serviceType = null) 
    : ServiceAttribute(ServiceLifetime.Singleton, serviceType);