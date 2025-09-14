using System.Collections.Generic;

namespace ApacheTech.Common.DependencyInjection.Abstractions;

/// <summary>
///     An IOC Container, which holds references to Added types of services, and their instances.
/// </summary>
public interface IServiceCollection : IList<ServiceDescriptor>
{
}