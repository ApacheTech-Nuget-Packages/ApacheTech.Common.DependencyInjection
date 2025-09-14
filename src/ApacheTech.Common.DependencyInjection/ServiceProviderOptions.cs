#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ApacheTech.Common.DependencyInjection;

/// <summary>
///     Options for configuring various behaviors of the default <see cref="IServiceProvider"/> implementation.
/// </summary>
public class ServiceProviderOptions
{
    // Avoid allocating objects in the default case
    internal static ServiceProviderOptions Default { get; } = new();

    /// <summary>
    ///     Determines whether to dispose implementations that implement
    ///     <see cref="IDisposable"/>, within the service collection, when disposing the provider.
    /// </summary>
    /// <value>
    ///   <c>true</c> if implementations should be disposed; otherwise, <c>false</c>.
    /// </value>
    public bool DisposeImplementations { get; set; } = true;

    /// <summary>
    ///     If <see cref="DisposeImplementations"/> is <see langword="true"/>, and this property
    ///      is not null, only implementations from the specified assemblies will be disposed.
    /// </summary>
    public List<Assembly>? DisposableAssemblies { get; set; }
}