using System;

namespace ApacheTech.Common.DependencyInjection.Abstractions;

/// <summary>
///     Creates instances of <see cref="IServiceScope"/>, which is used to create
///     services within a scope.
/// </summary>
public interface IServiceScopeFactory
{
    /// <summary>
    ///     Create an <see cref="ApacheTech.Common.DependencyInjection.Abstractions.IServiceScope"/> that
    ///     contains an <see cref="System.IServiceProvider"/> used to resolve dependencies from a
    ///     newly created scope.
    /// </summary>
    /// <returns>
    ///     An <see cref="ApacheTech.Common.DependencyInjection.Abstractions.IServiceScope"/> controlling the
    ///     lifetime of the scope. Once this is disposed, any scoped services and any transient services
    ///     that have been resolved from the
    ///     <see cref="ApacheTech.Common.DependencyInjection.Abstractions.IServiceScope.ServiceProvider"/> will also be disposed.
    /// </returns>
    IServiceScope CreateScope();
}