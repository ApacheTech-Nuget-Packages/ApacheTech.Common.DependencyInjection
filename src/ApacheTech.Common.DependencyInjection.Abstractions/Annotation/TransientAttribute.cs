using System;

namespace ApacheTech.Common.DependencyInjection.Abstractions.Annotation;

/// <summary>
///     Denotes that this class should be registered as a transient service within the IOC container.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute(Type? serviceType = null)
    : ServiceAttribute(ServiceLifetime.Transient, serviceType);