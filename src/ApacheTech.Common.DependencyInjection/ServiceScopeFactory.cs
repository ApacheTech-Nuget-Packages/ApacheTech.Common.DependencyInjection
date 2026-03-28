using ApacheTech.Common.DependencyInjection.Abstractions;

namespace ApacheTech.Common.DependencyInjection;

internal sealed class ServiceScopeFactory(ServiceProvider root) : IServiceScopeFactory
{
    /// <summary>
    ///     Creates a new <see cref="IServiceScope"/>.
    /// </summary>
    /// <returns>A new scope.</returns>
    public IServiceScope CreateScope() => root.CreateScope();
}