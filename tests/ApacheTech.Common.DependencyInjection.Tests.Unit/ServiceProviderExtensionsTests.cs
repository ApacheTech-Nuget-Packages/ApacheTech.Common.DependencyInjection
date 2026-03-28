using ApacheTech.Common.DependencyInjection.Abstractions.Extensions;
using ApacheTech.Common.DependencyInjection.Extensions;
using ApacheTech.Common.DependencyInjection.Tests.Unit.Domain;

namespace ApacheTech.Common.DependencyInjection.Tests.Unit;

public class ServiceProviderExtensionsTests
{
    [Fact]
    public void GetService_Generic_ReturnsRegisteredService()
    {
        var services = new ServiceCollection();
        services.AddSingleton<SingletonService>();

        var provider = services.BuildServiceProvider();

        var result = provider.GetService<SingletonService>();

        Assert.NotNull(result);
    }

    [Fact]
    public void GetServices_Generic_ReturnsAllRegisteredServices()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITestService, SingletonService>();
        services.AddTransient<ITestService, TransientService>();

        var provider = services.BuildServiceProvider();

        var results = provider.GetServices<ITestService>();
        var list = results.ToList();

        Assert.Equal(2, list.Count);
        Assert.Contains(list, x => x is SingletonService);
        Assert.Contains(list, x => x is TransientService);
    }

    private sealed class CtorService(ITestService inner)
    {
        public ITestService Inner { get; } = inner;
    }

    [Fact]
    public void CreateInstance_UsesContainerForDependencies()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITestService, TransientService>();

        var provider = services.BuildServiceProvider();

        var instance = (CtorService)provider.CreateInstance(typeof(CtorService));

        Assert.NotNull(instance.Inner);
        Assert.IsType<TransientService>(instance.Inner);
    }

    [Fact]
    public void CreateInstance_Generic_UsesContainerForDependencies()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITestService, TransientService>();

        var provider = services.BuildServiceProvider();

        var instance = provider.CreateInstance<CtorService>();

        Assert.NotNull(instance.Inner);
        Assert.IsType<TransientService>(instance.Inner);
    }

    [Fact]
    public void Resolve_ReturnsRequiredService()
    {
        var services = new ServiceCollection();
        services.AddSingleton<SingletonService>();

        var provider = services.BuildServiceProvider();

        var instance = provider.Resolve<SingletonService>();

        Assert.NotNull(instance);
    }
}
