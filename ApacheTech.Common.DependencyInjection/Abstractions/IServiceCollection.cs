using System;
using System.Collections.Generic;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.Common.DependencyInjection.Abstractions
{
    /// <summary>
    ///     An IOC Container, which holds references to Added types of services, and their instances.
    /// </summary>
    public interface IServiceCollection : IList<ServiceDescriptor>
    {
    }
}