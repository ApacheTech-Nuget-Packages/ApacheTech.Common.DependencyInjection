using System;

namespace ApacheTech.Common.DependencyInjection.Abstractions;

/// <summary>
///     Defines a disposable service scope.
/// </summary>
/// <remarks>
///     The <see cref="System.IDisposable.Dispose"/> method ends the scope lifetime. Once Dispose
///     is called, any scoped services and any transient services that have been resolved from
///     <see cref="ApacheTech.Common.DependencyInjection.Abstractions.IServiceScope.ServiceProvider"/> will be
///     disposed.
/// </remarks>
public interface IServiceScope : IDisposable
{
    /// <summary>
    ///     Gets the <see cref="System.IServiceProvider"/> used to resolve dependencies from the scope.
    /// </summary>
    IServiceProvider ServiceProvider { get; }
}