using System;
using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.DependencyInjection.Abstractions.Extensions;
using ApacheTech.Common.Extensions.DotNet;

namespace ApacheTech.Common.DependencyInjection.Extensions;

/// <summary>
///     Extension methods to aid the building of an <see cref="IServiceProvider"/> from an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionBuilderExtensions
{
    /// <summary>
    ///     Creates a <see cref="ServiceProvider"/> containing services from the provided <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
    /// <param name="options"> Configures various service provider behaviours.</param>
    /// <returns>The <see cref="ServiceProvider"/>.</returns>

    public static ServiceProvider BuildServiceProvider(this IServiceCollection services, Action<ServiceProviderOptions>? options = null)
    {
        services.ThrowIfNull();
        options.ThrowIfNull();
        return new ServiceProvider(services, ServiceProviderOptions.Default.With(options));
    }
}