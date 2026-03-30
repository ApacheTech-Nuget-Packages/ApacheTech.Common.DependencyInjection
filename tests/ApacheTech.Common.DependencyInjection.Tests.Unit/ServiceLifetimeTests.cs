#pragma warning disable CS9113 // Parameter is unread.

using ApacheTech.Common.DependencyInjection.Abstractions.Extensions;
using ApacheTech.Common.DependencyInjection.Extensions;
using ApacheTech.Common.DependencyInjection.Tests.Unit.Domain;

namespace ApacheTech.Common.DependencyInjection.Tests.Unit;

public class ServiceLifetimeTests
{
    [Fact]
    public void Singleton_ReturnsSameInstance()
    {
        var services = new ServiceCollection();
        services.AddSingleton<SingletonService>();

        var provider = services.BuildServiceProvider();

        var a = provider.GetService(typeof(SingletonService));
        var b = provider.GetService(typeof(SingletonService));

        Assert.Same(a, b);
    }

    [Fact]
    public void Transient_ReturnsDifferentInstances()
    {
        var services = new ServiceCollection();
        services.AddTransient<TransientService>();

        var provider = services.BuildServiceProvider();

        var a = provider.GetService(typeof(TransientService));
        var b = provider.GetService(typeof(TransientService));

        Assert.NotSame(a, b);
    }

    [Fact]
    public void Enumerable_ReturnsAllRegistrations()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITestService, SingletonService>();
        services.AddTransient<ITestService, TransientService>();

        var provider = services.BuildServiceProvider();

        var result = (IEnumerable<ITestService>)provider.GetService(typeof(IEnumerable<ITestService>))!;

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void Scoped_SameScope_ReturnsSameInstance()
    {
        var services = new ServiceCollection();
        services.AddScoped<ScopedService>();

        var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();

        var a = scope.ServiceProvider.GetService(typeof(ScopedService));
        var b = scope.ServiceProvider.GetService(typeof(ScopedService));

        Assert.Same(a, b);
    }

    [Fact]
    public void Scoped_DifferentScopes_ReturnDifferentInstances()
    {
        var services = new ServiceCollection();
        services.AddScoped<ScopedService>();

        var provider = services.BuildServiceProvider();

        ScopedService a;
        ScopedService b;

        using (var scope1 = provider.CreateScope())
        {
            a = (ScopedService)scope1.ServiceProvider.GetService(typeof(ScopedService))!;
        }

        using (var scope2 = provider.CreateScope())
        {
            b = (ScopedService)scope2.ServiceProvider.GetService(typeof(ScopedService))!;
        }

        Assert.NotSame(a, b);
    }

    [Fact]
    public void Scoped_FromRoot_Throws()
    {
        var services = new ServiceCollection();
        services.AddScoped<ScopedService>();

        var provider = services.BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() =>
            provider.GetService(typeof(ScopedService)));
    }

    [Fact]
    public void Scoped_InObjectGraph_IsSameInstanceWithinScope()
    {
        var services = new ServiceCollection();
        services.AddScoped<ScopedService>();
        services.AddTransient<DependentService>();

        var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();

        var a = (DependentService)scope.ServiceProvider.GetService(typeof(DependentService))!;
        var b = (DependentService)scope.ServiceProvider.GetService(typeof(DependentService))!;

        Assert.Same(a.Scoped, b.Scoped);
    }

    [Fact]
    public void Scoped_ThroughTransient_PreservesScope()
    {
        var services = new ServiceCollection();
        services.AddScoped<ScopedService>();
        services.AddTransient<DependentService>();

        var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();

        var dependent = (DependentService)scope.ServiceProvider.GetService(typeof(DependentService))!;

        var direct = (ScopedService)scope.ServiceProvider.GetService(typeof(ScopedService))!;

        Assert.Same(direct, dependent.Scoped);
    }

    [Fact]
    public void Scoped_DisposesServicesOnScopeDispose()
    {
        var services = new ServiceCollection();
        services.AddScoped<DisposableService>();

        var provider = services.BuildServiceProvider();

        DisposableService instance;

        var scope = provider.CreateScope();
        instance = (DisposableService)scope.ServiceProvider.GetService(typeof(DisposableService))!;

        scope.Dispose();

        Assert.True(instance.Disposed);
    }

    [Fact]
    public void Scoped_FromRoot_WithValidation_Throws()
    {
        var services = new ServiceCollection();
        services.AddScoped<ScopedService>();

        var provider = services.BuildServiceProvider(o => o.ValidateScopes = true);

        Assert.Throws<InvalidOperationException>(() =>
            provider.GetService(typeof(ScopedService)));
    }

    [Fact]
    public void Scoped_FromRoot_WithoutValidation_DoesNotThrow()
    {
        var services = new ServiceCollection();
        services.AddScoped<ScopedService>();

        var provider = services.BuildServiceProvider(o => o.ValidateScopes = false);

        var result = provider.GetService(typeof(ScopedService));

        Assert.NotNull(result);
    }

    [Fact]
    public void Enumerable_WithScoped_FromRoot_Throws()
    {
        var services = new ServiceCollection();
        services.AddScoped<ITestService, ScopedService>();

        var provider = services.BuildServiceProvider(o => o.ValidateScopes = true);

        Assert.Throws<InvalidOperationException>(() =>
            provider.GetService(typeof(IEnumerable<ITestService>)));
    }

    [Fact]
    public void Dispose_WhenDisabled_DoesNotDispose()
    {
        var services = new ServiceCollection();
        services.AddSingleton<DisposableService>();

        var provider = services.BuildServiceProvider(o =>
        {
            o.DisposeImplementations = false;
        });

        var instance = (DisposableService)provider.GetService(typeof(DisposableService))!;

        provider.Dispose();

        Assert.False(instance.Disposed);
    }

    [Fact]
    public void Dispose_WhenEnabled_DisposesSingleton()
    {
        var services = new ServiceCollection();
        services.AddSingleton<DisposableService>();

        var provider = services.BuildServiceProvider();

        var instance = (DisposableService)provider.GetService(typeof(DisposableService))!;

        provider.Dispose();

        Assert.True(instance.Disposed);
    }

    [Fact]
    public void Dispose_OnlyDisposesMatchingAssemblies()
    {
        var services = new ServiceCollection();
        services.AddSingleton<DisposableService>();

        var provider = services.BuildServiceProvider(o =>
        {
            o.DisposableAssemblies = []; // empty = nothing allowed
        });

        var instance = (DisposableService)provider.GetService(typeof(DisposableService))!;

        provider.Dispose();

        Assert.False(instance.Disposed);
    }

    [Fact]
    public void GetService_ResolvesLastOrDefaultForMultipleMatches()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITestService, SingletonService>();
        services.AddSingleton<ITestService, ScopedService>();

        var provider = services.BuildServiceProvider();

        var instance = (ITestService)provider.GetService(typeof(ITestService))!;

        Assert.IsType<ScopedService>(instance);
    }

    [Fact]
    public void GetRequiredService_ResolvesLastOrDefaultForMultipleMatches()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITestService, SingletonService>();
        services.AddSingleton<ITestService, ScopedService>();

        var provider = services.BuildServiceProvider();

        var instance = (ITestService)provider.GetRequiredService(typeof(ITestService))!;

        Assert.IsType<ScopedService>(instance);
    }

    [Fact]
    public void Dispose_DisposesWhenAssemblyMatches()
    {
        var services = new ServiceCollection();
        services.AddSingleton<DisposableService>();

        var provider = services.BuildServiceProvider(o =>
        {
            o.DisposableAssemblies = []; // empty = nothing allowed
        });

        var instance = (DisposableService)provider.GetService(typeof(DisposableService))!;

        provider.Dispose();

        Assert.True(instance.Disposed);
    }

    [Fact]
    public void Scoped_InjectedIntoSingleton_Throws()
    {
        var services = new ServiceCollection();
        services.AddScoped<ScopedService>();
        services.AddSingleton<DependentService>(); // depends on ScopedService

        var provider = services.BuildServiceProvider(o => o.ValidateScopes = true);

        Assert.Throws<InvalidOperationException>(() =>
            provider.GetService(typeof(DependentService)));
    }

    [Fact]
    public void Scoped_InjectedIntoTransient_IsAllowed()
    {
        var services = new ServiceCollection();
        services.AddScoped<ScopedService>();
        services.AddTransient<DependentService>();

        var provider = services.BuildServiceProvider();

        using var scope = provider.CreateScope();

        var instance = scope.ServiceProvider.GetService(typeof(DependentService));

        Assert.NotNull(instance);
    }

    private class Level1(Level2 l2);
    private class Level2(ScopedService scoped);

    [Fact]
    public void Nested_Scoped_In_Singleton_Throws()
    {
        var services = new ServiceCollection();
        services.AddScoped<ScopedService>();
        services.AddTransient<Level2>();
        services.AddSingleton<Level1>();

        var provider = services.BuildServiceProvider(o => o.ValidateScopes = true);

        Assert.Throws<InvalidOperationException>(() =>
            provider.GetService(typeof(Level1)));
    }

    private class A(B b);
    private class B(A a);
    private class C(A a);

    [Fact]
    public void CircularDependency_Throws()
    {
        var services = new ServiceCollection();
        services.AddTransient<A>();
        services.AddTransient<B>();

        var provider = services.BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() =>
            provider.GetService(typeof(A)));
    }


    [Fact]
    public void IndirectCircularDependency_Throws()
    {
        var services = new ServiceCollection();
        services.AddTransient<A>();
        services.AddTransient<B>();
        services.AddTransient<C>();

        var provider = services.BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() =>
            provider.GetService(typeof(A)));
    }
}