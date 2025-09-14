using System;
using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.DependencyInjection.Abstractions.Extensions;

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
    /// <returns>The <see cref="ServiceProvider"/>.</returns>

    public static ServiceProvider BuildServiceProvider(this IServiceCollection services)
    {
        return BuildServiceProvider(services, ServiceProviderOptions.Default);
    }

    /// <summary>
    ///     Creates a <see cref="ServiceProvider"/> containing services from the provided <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
    /// <param name="disposeImplementations">
    ///     Determines whether to dispose implementations that implement
    ///     <see cref="IDisposable"/>, within the service collection, when disposing the provider.
    /// </param>
    /// <returns>The <see cref="ServiceProvider"/>.</returns>

    public static ServiceProvider BuildServiceProvider(this IServiceCollection services, bool disposeImplementations)
    {
        var options = new ServiceProviderOptions { DisposeImplementations = disposeImplementations };
        return BuildServiceProvider(services, options);
    }

    /// <summary>
    ///     Creates a <see cref="ServiceProvider"/> containing services from the provided <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
    /// <param name="options"> Configures various service provider behaviours.</param>
    /// <returns>The <see cref="ServiceProvider"/>.</returns>

    public static ServiceProvider BuildServiceProvider(this IServiceCollection services, ServiceProviderOptions options)
    {
        services.ThrowIfNull();
        options.ThrowIfNull();
        return new ServiceProvider(services, options);
    }
}
