using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.DependencyInjection.Abstractions.Extensions;
using ApacheTech.Common.DependencyInjection.Extensions;

namespace ApacheTech.Common.DependencyInjection.Tests.Unit;

/// <summary>
///     Unit tests covering keyed and non-keyed service resolution across all lifetimes and behaviours.
/// </summary>
public class KeyedServiceProviderTests
{
    /// <summary>
    ///     Ensures that a transient service can be resolved without a key.
    /// </summary>
    [Fact]
    public void Transient_Unkeyed_Resolves()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITestService, TestServiceA>();

        var provider = services.BuildServiceProvider();

        var instance = provider.GetService(typeof(ITestService));

        Assert.NotNull(instance);
        Assert.IsType<TestServiceA>(instance);
    }

    /// <summary>
    ///     Ensures that a keyed transient resolves correctly.
    /// </summary>
    [Fact]
    public void Transient_Keyed_Resolves_Correct_Instance()
    {
        var services = new ServiceCollection();
        services.AddKeyedTransient<ITestService, TestServiceA>("a");
        services.AddKeyedTransient<ITestService, TestServiceB>("b");

        var provider = services.BuildServiceProvider();

        var a = provider.GetService(typeof(ITestService), "a");
        var b = provider.GetService(typeof(ITestService), "b");

        Assert.IsType<TestServiceA>(a);
        Assert.IsType<TestServiceB>(b);
    }

    /// <summary>
    ///     Ensures that unkeyed resolution does not return keyed services.
    /// </summary>
    [Fact]
    public void Unkeyed_Does_Not_Return_Keyed()
    {
        var services = new ServiceCollection();
        services.AddKeyedTransient<ITestService, TestServiceA>("a");

        var provider = services.BuildServiceProvider();

        var result = provider.GetService(typeof(ITestService));

        Assert.Null(result);
    }

    /// <summary>
    ///     Ensures singleton services are cached per key.
    /// </summary>
    [Fact]
    public void Singleton_Keyed_Caches_Per_Key()
    {
        var services = new ServiceCollection();
        services.AddKeyedSingleton<ITestService, TestServiceA>("a");
        services.AddKeyedSingleton<ITestService, TestServiceA>("b");

        var provider = services.BuildServiceProvider();

        var a1 = provider.GetKeyedService<ITestService>("a");
        var a2 = provider.GetKeyedService<ITestService>("a");
        var b1 = provider.GetKeyedService<ITestService>("b");

        Assert.Same(a1, a2);
        Assert.NotSame(a1, b1);
    }

    /// <summary>
    ///     Ensures transient services are not cached.
    /// </summary>
    [Fact]
    public void Transient_Creates_New_Instance()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITestService, TestServiceA>();

        var provider = services.BuildServiceProvider();

        var a = provider.GetService(typeof(ITestService));
        var b = provider.GetService(typeof(ITestService));

        Assert.NotSame(a, b);
    }

    /// <summary>
    ///     Ensures scoped services are unique per scope.
    /// </summary>
    [Fact]
    public void Scoped_Unique_Per_Scope()
    {
        var services = new ServiceCollection();
        services.AddScoped<ITestService, TestServiceA>();

        var provider = services.BuildServiceProvider();

        using var scope1 = provider.CreateScope();
        using var scope2 = provider.CreateScope();

        var s1a = scope1.ServiceProvider.GetService(typeof(ITestService));
        var s1b = scope1.ServiceProvider.GetService(typeof(ITestService));
        var s2 = scope2.ServiceProvider.GetService(typeof(ITestService));

        Assert.Same(s1a, s1b);
        Assert.NotSame(s1a, s2);
    }

    /// <summary>
    ///     Ensures keyed scoped services respect scope boundaries.
    /// </summary>
    [Fact]
    public void Scoped_Keyed_Unique_Per_Scope()
    {
        var services = new ServiceCollection();
        services.AddKeyedScoped<ITestService, TestServiceA>("a");

        var provider = services.BuildServiceProvider();

        using var scope1 = provider.CreateScope();
        using var scope2 = provider.CreateScope();

        var p1 = scope1.ServiceProvider;
        var p2 = scope2.ServiceProvider;

        var s1 = p1.GetKeyedService<ITestService>("a");
        var s2 = p1.GetKeyedService<ITestService>("a");
        var s3 = p2.GetKeyedService<ITestService>("a");

        Assert.Same(s1, s2);
        Assert.NotSame(s1, s3);
    }

    /// <summary>
    ///     Ensures IEnumerable resolves only unkeyed services.
    /// </summary>
    [Fact]
    public void IEnumerable_Unkeyed_Only_Returns_Unkeyed()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITestService, TestServiceA>();
        services.AddKeyedTransient<ITestService, TestServiceB>("b");

        var provider = services.BuildServiceProvider();

        var result = (IEnumerable<ITestService>)provider.GetService(typeof(IEnumerable<ITestService>))!;

        Assert.Single(result);
        Assert.IsType<TestServiceA>(Assert.Single(result));
    }

    /// <summary>
    ///     Ensures keyed IEnumerable returns only matching key.
    /// </summary>
    [Fact]
    public void IEnumerable_Keyed_Returns_Matching_Key()
    {
        var services = new ServiceCollection();
        services.AddKeyedTransient<ITestService, TestServiceA>("a");
        services.AddKeyedTransient<ITestService, TestServiceB>("a");
        services.AddKeyedTransient<ITestService, TestServiceB>("b");

        var provider = services.BuildServiceProvider();

        var result = provider.GetKeyedService<IEnumerable<ITestService>>("a")!;

        Assert.Equal(2, new List<ITestService>(result).Count);
    }

    /// <summary>
    ///     Ensures resolving unknown keyed service throws.
    /// </summary>
    [Fact]
    public void Unknown_Key_Throws()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITestService, TestServiceA>();

        var provider = services.BuildServiceProvider();

        var service = provider.GetKeyedService<ITestService>("missing");
    
        Assert.Null(service);
    }

    /// <summary>
    ///     Ensures resolving unknown unkeyed service throws.
    /// </summary>
    [Fact]
    public void Unknown_Unkeyed_Throws()
    {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();

        var service = provider.GetService(typeof(ITestService));

        Assert.Null(service);
    }

    private interface ITestService;

    private sealed class TestServiceA : ITestService;

    private sealed class TestServiceB : ITestService;
}