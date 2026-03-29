using System;

namespace ApacheTech.Common.DependencyInjection.Abstractions;

/// <summary> 
///     Defines a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
/// </summary>
public interface IKeyedServiceProvider : IServiceProvider 
{ 
    /// <summary>
    ///     Gets the service object of the specified type.
    /// </summary> 
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <param name="serviceKey">The key associated with the service object to get.</param> 
    /// <returns> 
    ///     A service object of type <paramref name="serviceType">serviceType</paramref>. -or- null if there is no service object of type <paramref name="serviceType">serviceType</paramref>.
    /// </returns>
    object? GetService(Type serviceType, object? serviceKey = null); 
}