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
    /// <param name="configure"> Configures various service provider behaviours.</param>
    /// <returns>The <see cref="ServiceProvider"/>.</returns>
    public static ServiceProvider BuildServiceProvider(this IServiceCollection services, Action<ServiceProviderOptions>? configure = null)
    {
        services.ThrowIfNull();

        // Default registrations that are always present in the service provider, regardless of whether the user has registered them or not.
        services.AddSingleton(services); // Register the service collection itself for ease of use in factories and such.
        services.AddSingleton(sp => sp); // Register the service provider itself for ease of use in factories and such.
        services.AddSingleton<IServiceScopeFactory, ServiceScopeFactory>(); // Register the scope factory for creating scopes.

        services.AddScoped<IServiceScope>(sp =>
        {
            if (sp is ScopedServiceProvider scoped) return scoped.Scope;
            throw new InvalidOperationException("No active scope.");
        });

        var options = ServiceProviderOptions.Default;
        configure?.Invoke(options);

        return new ServiceProvider(services, options);
    }
}